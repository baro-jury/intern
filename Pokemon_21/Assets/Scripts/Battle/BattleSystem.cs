using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, ActionSelection, MoveSelection, RunningTurn, Busy, PartyScreen, BattleOver }
public enum BattleAction { Move, SwitchPokemon, UseItem, Run }

public class BattleSystem : MonoBehaviour
{   
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    [SerializeField] BattleDialogBox dialogBox;

    [SerializeField] PartyScreen partyScreen;

    PokemonParty playerParty;
    Pokemon wildPokemon;

    BattleState state;
    BattleState? prevState;
    int currentAction;
    int currentMove;
    int currentMember;

    public event Action<bool> OnBattleOver; //true = win battle, false = lose battle

    bool timerFlag;
    IEnumerator SkippableWaitTimer(float time)
    {
        timerFlag = false;
        yield return new WaitForSeconds(time);
        timerFlag = true;
    }
    IEnumerator SkippableWait(float time)
    {
        yield return new WaitForSeconds(Time.deltaTime);

        IEnumerator timer = SkippableWaitTimer(time);
        StartCoroutine(timer);
        while (!timerFlag)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                timerFlag = true;
                StopCoroutine(timer);
                yield break;
            }
            yield return null;
        }
    }

    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        state = BattleState.Start;

        int tempMember = 0;
        playerUnit.Setup(playerParty.GetHealthyPokemon(ref tempMember));
        enemyUnit.Setup(wildPokemon);

        currentAction = 0;
        currentMove = 0;
        currentMember = tempMember;

        partyScreen.Init();

        dialogBox.EnableActionSelector(false);
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");
        yield return SkippableWait(1f);

        StartCoroutine(ActionSelection());
    }

    IEnumerator ActionSelection()
    {
        yield return dialogBox.TypeDialog("Choose an action.");

        state = BattleState.ActionSelection;
        dialogBox.EnableActionSelector(true);
    }

    void BackToActionSelection()
    {
        state = BattleState.ActionSelection;

        dialogBox.EnableDialogText(true);
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        partyScreen.gameObject.SetActive(false);

        dialogBox.SetDialog("Choose an action.");
    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;

        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
    }

    void OpenPartyScreen()
    {
        //print("Party Screen");
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty._PokemonParty);
        partyScreen.gameObject.SetActive(true);
    }

    IEnumerator RunTurn(BattleAction playerAction)
    {
        state = BattleState.RunningTurn;

        if (playerAction == BattleAction.Move)
        {
            playerUnit.Pokemon.CurrentMove = playerUnit.Pokemon.Moves[currentMove];
            enemyUnit.Pokemon.CurrentMove = enemyUnit.Pokemon.GetRandomMove();

            int playerMovePriority = playerUnit.Pokemon.CurrentMove.Base.Priority;
            int enemyMovePriority = enemyUnit.Pokemon.CurrentMove.Base.Priority;

            //Check who goes first
            bool playerGoesFirst = true;
            if (playerMovePriority > enemyMovePriority)
                playerGoesFirst = true;
            else if (enemyMovePriority > playerMovePriority)
                playerGoesFirst = false;
            else if (enemyMovePriority == playerMovePriority)
                playerGoesFirst = playerUnit.Pokemon.Speed >= enemyUnit.Pokemon.Speed;
            
            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            var secondPokemon = secondUnit.Pokemon;

            //First turn
            yield return RunMove(firstUnit, secondUnit, firstUnit.Pokemon.CurrentMove);
            yield return RunAfterTurn(firstUnit);
            if (state == BattleState.BattleOver) yield break;

            if (secondPokemon.Hp > 0) //TODO: Bug - If first pokemon faints on its own turn (e.g due to status condition), the second pokemon still attacks
                                      //-> Visual bug, attack the next pokemon
            {
                //Second turn
                yield return RunMove(secondUnit, firstUnit, secondUnit.Pokemon.CurrentMove);
                yield return RunAfterTurn(secondUnit); //TODO: If enemy pokemon has status condition and defeats player pokemon (using attacking move)
                                                       //-> the effect only activate after sending out next pokemon - Not what we want
                if (state == BattleState.BattleOver) yield break;
            }
        }
        else
        {
            if (playerAction == BattleAction.SwitchPokemon)
            {
                var selectedPokemon = playerParty._PokemonParty[currentMember];
                state = BattleState.Busy;
                StartCoroutine(SwitchPokemon(selectedPokemon));
            }

            yield return new WaitUntil(() => state == BattleState.RunningTurn);
            //Enemy Turn
            var enemyMove = enemyUnit.Pokemon.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);
            if (state == BattleState.BattleOver) yield break;
        }

        if (state != BattleState.BattleOver)
            StartCoroutine(ActionSelection());
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Pokemon.OnBeforeMove();
        if (!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.Pokemon);
            yield return sourceUnit.Hud.UpdateHP();
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Pokemon);

        --move.Pp;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} used {move.Base.Name}.");
        yield return new WaitForSeconds(1f);

        if (CheckIfMoveHit(move, sourceUnit.Pokemon, targetUnit.Pokemon))
        {

            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);

            targetUnit.PlayHitAnimation();

            if (move.Base.Category == MoveCategory.Status)
            {
                yield return RunMoveEffects(sourceUnit.Pokemon, targetUnit.Pokemon, move.Base.Effects, move.Base.Target);
            }
            else
            {
                var damageDetails = targetUnit.Pokemon.TakeDamage(sourceUnit.Pokemon, move);
                yield return targetUnit.Hud.UpdateHP();
                yield return ShowDamageDetails(damageDetails);
            }

            if (move.Base.Secondaries != null && move.Base.Secondaries.Count > 0 && targetUnit.Pokemon.Hp > 0)
            {
                foreach (var secondary in move.Base.Secondaries)
                {
                    var rnd = UnityEngine.Random.Range(1, 101);
                    if (rnd <= secondary.Chance)
                        yield return RunMoveEffects(sourceUnit.Pokemon, targetUnit.Pokemon, secondary, secondary.Target);
                }
            }

            if (targetUnit.Pokemon.Hp <= 0)
            {
                yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} fainted.");
                targetUnit.PlayFaintAnimation();

                yield return new WaitForSeconds(2f);
                CheckForBattleOver(targetUnit);
            }
        }
        else
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name}'s attack missed!");
            yield return SkippableWait(1f);
        }
        
    }

    IEnumerator RunMoveEffects(Pokemon sourcePokemon, Pokemon targetPokemon, MoveEffects effects, MoveTarget target)
    {
        //Stat Boosting
        if (effects.Boosts != null)
        {
            if (target == MoveTarget.Self)
                sourcePokemon.ApplyBoost(effects.Boosts);
            else if (target == MoveTarget.Foe)
                targetPokemon.ApplyBoost(effects.Boosts);
        }

        //Status Condition
        if (effects.Status != ConditionID.none)
        {
            targetPokemon.SetStatus(effects.Status);
        }

        //Volatile Status Condition
        if (effects.VolatileStatus != ConditionID.none)
        {
            targetPokemon.SetVolatileStatus(effects.VolatileStatus);
        }

        yield return ShowStatusChanges(sourcePokemon);
        yield return ShowStatusChanges(targetPokemon);
    }

    IEnumerator RunAfterTurn(BattleUnit sourceUnit)
    {
        if (state == BattleState.BattleOver) yield break;
        yield return new WaitUntil(() => state == BattleState.RunningTurn);

        //Status like PSN or BRN will hurt the pokemon after attacking (not really what we want)
        //TODO: Status like PSN or BRN will hurt the pokemon after both turns end
        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Pokemon);
        yield return sourceUnit.Hud.UpdateHP();
        if (sourceUnit.Pokemon.Hp <= 0)
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} fainted.");
            sourceUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            CheckForBattleOver(sourceUnit);
        }
    }

    bool CheckIfMoveHit(Move move, Pokemon source, Pokemon target)
    {
        if (move.Base.AlwaysHit)
            return true;
        
        float moveAccuracy = move.Base.Accuracy;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];
        var boostValues = new float[] { 3f / 3f, 4f / 3f, 5f / 3f, 6f / 3f, 7f / 3f, 8f / 3f, 9f / 3f };

        if (accuracy >= 0)
            moveAccuracy *= boostValues[accuracy];
        else
            moveAccuracy /= boostValues[-accuracy];

        if (evasion >= 0)
            moveAccuracy /= boostValues[evasion];
        else
            moveAccuracy *= boostValues[-evasion];

        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.StatusChanges.Count > 0)
        {
            var message = pokemon.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
            yield return SkippableWait(1f);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                OpenPartyScreen();
            }
            else
            {
                BattleOver(false); //lost battle
            }
        }
        else
        {
            BattleOver(true); //won battle
        }
    }

    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;

        //Reset stat boost + cure volatile status after battle
        playerParty._PokemonParty.ForEach(p => p.OnBattleOver());

        OnBattleOver(won);
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1)
        {
            yield return dialogBox.TypeDialog("A critical hit!");
            yield return SkippableWait(1f);
        }

        if (damageDetails.TypeEffectiveness > 1)
        {
            yield return dialogBox.TypeDialog("It's super effective!");
            yield return SkippableWait(1f);
        }
        else if (damageDetails.TypeEffectiveness < 1)
        {
            yield return dialogBox.TypeDialog("It's not very effective...");
            yield return SkippableWait(1f);
        }
    }

    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();   
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction == 0 || currentAction == 2)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction == 1 || currentAction == 3)
                --currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 2)
                currentAction += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 1)
                currentAction -= 2;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                //Fight
                MoveSelection();
            }
            else if (currentAction == 1)
            {
                //Bag
            }
            else if (currentAction == 2)
            {
                //Pokemon
                prevState = state;
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                //Run
                BattleOver(true);
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 1 && currentMove % 2 < 1)
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0 && currentMove % 2 > 0)
                --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
                currentMove -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var move = playerUnit.Pokemon.Moves[currentMove];
            if (move.Pp == 0) return;
            
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            dialogBox.SetDialog("");
            StartCoroutine(RunTurn(BattleAction.Move));
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            BackToActionSelection();
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMember < playerParty._PokemonParty.Count - 1 && currentMember % 2 < 1)
                ++currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMember > 0 && currentMember % 2 > 0)
                --currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMember < playerParty._PokemonParty.Count - 2)
                currentMember += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMember > 1)
                currentMember -= 2;
        }

        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty._PokemonParty[currentMember];
            if (selectedMember.Hp <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted Pokemon.");
                return;
            }
            if (selectedMember == playerUnit.Pokemon)
            {
                partyScreen.SetMessageText("You can't select the same Pokemon.");
                return;
            }

            partyScreen.gameObject.SetActive(false);
            dialogBox.EnableActionSelector(false);

            if (prevState == BattleState.ActionSelection)
            {
                prevState = null;
                StartCoroutine(RunTurn(BattleAction.SwitchPokemon));
            }
            else
            {
                state = BattleState.Busy;
                StartCoroutine(SwitchPokemon(selectedMember));
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (playerUnit.Pokemon.Hp <= 0) return;
            BackToActionSelection();
        }
    }

    IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        //Reset stat boost after switching
        playerUnit.Pokemon.ResetStatBoost();
        //Cure volatile status after switching
        playerUnit.Pokemon.CureVolatileStatus();

        if (playerUnit.Pokemon.Hp > 0)
        {
            yield return dialogBox.TypeDialog($"Come back {playerUnit.Pokemon.Base.Name}!");
            playerUnit.PlaySwitchAnimation();
            yield return new WaitForSeconds(1f);
            yield return SkippableWait(1f);
        }

        playerUnit.Setup(newPokemon);
        currentMove = 0;

        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}!");
        yield return SkippableWait(1f);

        state = BattleState.RunningTurn;
    }

}
