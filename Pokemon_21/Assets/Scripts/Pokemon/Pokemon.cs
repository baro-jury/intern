using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase Base { get => _base; }
    public int Level { get => level; }

    public int Hp { get; set; }

    public List<Move> Moves { get; set; }
    public Move CurrentMove { get; set; }

    public Dictionary<Stat, int> NormalStats { get; private set; }
    
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Condition Status { get; private set; }
    public int StatusTime { get; set; }
    public Condition VolatileStatus { get; private set; }
    public int VolatileStatusTime { get; set; }

    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();
    public bool HpChanged { get; set; }
    public event System.Action OnStatusChanged;

    public void Init()
    {
        //Generate moves
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }

        CalculateStats();
        Hp = MaxHp;

        ResetStatBoost();
        Status = null;
        VolatileStatus = null;
    }

    void CalculateStats()
    {
        NormalStats = new Dictionary<Stat, int>();
        NormalStats.Add(Stat.Attack, Mathf.FloorToInt((2 * Base.Attack * Level) / 100f) + 5);
        NormalStats.Add(Stat.Defense, Mathf.FloorToInt((2 * Base.Defense * Level) / 100f) + 5);
        NormalStats.Add(Stat.SpAttack, Mathf.FloorToInt((2 * Base.SpAttack * Level) / 100f) + 5);
        NormalStats.Add(Stat.SpDefense, Mathf.FloorToInt((2 * Base.SpDefense * Level) / 100f) + 5);
        NormalStats.Add(Stat.Speed, Mathf.FloorToInt((2 * Base.Speed * Level) / 100f) + 5);

        MaxHp = Mathf.FloorToInt((2 * Base.Hp * Level) / 100f) + Level + 10;
    }

    public void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0 },
            {Stat.Defense, 0 },
            {Stat.SpAttack, 0 },
            {Stat.SpDefense, 0 },
            {Stat.Speed, 0 },

            {Stat.Accuracy, 0 },
            {Stat.Evasion, 0 }
        };
    }

    float GetStat(Stat stat)
    {
        float statValue = NormalStats[stat];

        //Apply stat boost
        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };
        
        if (boost >= 0)
            statValue *= boostValues[boost];
        else statValue /= boostValues[-boost];

        return statValue;
    }

    public void ApplyBoost(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, - 6, 6);

            if (boost > 0)
                StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
            else
                StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");

            Debug.Log($"{Base.Name}'s {stat} has been boosted to {StatBoosts[stat]}");
        }
    }

    public int MaxHp { get; private set; }
    public float Attack { get { return GetStat(Stat.Attack); } }
    public float Defense { get { return GetStat(Stat.Defense); } }
    public float SpAttack { get { return GetStat(Stat.SpAttack); } }
    public float SpDefense { get { return GetStat(Stat.SpDefense); } }
    public float Speed { get { return GetStat(Stat.Speed); } }

    public DamageDetails TakeDamage(Pokemon attacker, Move move)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;
        
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            //Fainted = false
        };

        float atk = move.Base.Category == MoveCategory.Special ? attacker.SpAttack : attacker.Attack;
        float def = move.Base.Category == MoveCategory.Special ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * (atk / def) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHp(damage);

        return damageDetails;
    }

    public void UpdateHp(int damage)
    {
        if (damage < 1) damage = 1;
        Hp = Mathf.Clamp(Hp - damage, 0, MaxHp);
        HpChanged = true;
    }

    public void SetStatus(ConditionID conditionId)
    {
        if (Status != null) return; //TODO: Show a message "No effect!" too if already have status condition
        
        Status = ConditionDatabase.Conditions[conditionId];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name} {Status.StartMessage}");

        OnStatusChanged?.Invoke();
    }

    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }

    public void SetVolatileStatus(ConditionID conditionId)
    {
        if (VolatileStatus != null) return; //TODO: Can have multiple different volatile statuses at the same time. If already have the same volatile status,
                                            //show message

        VolatileStatus = ConditionDatabase.Conditions[conditionId];
        VolatileStatus?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name} {VolatileStatus.StartMessage}");
    }

    public void CureVolatileStatus()
    {
        VolatileStatus = null;
    }

    public Move GetRandomMove()
    {
        List<Move> movesWithPP = Moves.Where(x => x.Pp > 0).ToList();
        
        int r = Random.Range(0, movesWithPP.Count);
        return movesWithPP[r];
    }

    public bool OnBeforeMove()
    {
        bool canPerformMove = true;
        
        if (Status?.OnBeforeMove != null)
        {
            if (!Status.OnBeforeMove(this))
                canPerformMove = false;
        }

        if (VolatileStatus?.OnBeforeMove != null)
        {
            if (!VolatileStatus.OnBeforeMove(this))
                canPerformMove = false;
        }

        return canPerformMove;
    }

    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }

    public void OnBattleOver()
    {
        CureVolatileStatus();
        ResetStatBoost();
    }
}

public class DamageDetails
{
    //public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}

