using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDatabase
{
    public static void Init()
    {
        foreach (var keyValuePair in Conditions)
        {
            var conditionId = keyValuePair.Key;
            var condition = keyValuePair.Value;

            condition.Id = conditionId;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "was poisoned.",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHp(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} was hurt by poison!");
                }
            }
        },
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "was burned.",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHp(pokemon.MaxHp / 16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} was hurt by its burn!");
                },
            }
        },
        {
            ConditionID.par,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage = "was paralyzed! It may not be able to move!",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (Random.Range(1, 5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is paralyzed! It can't move!");
                        return false;
                    }
                    return true;
                },
            }
        },
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Frozen",
                StartMessage = "was frozen.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (Random.Range(1, 6) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} thawed out.");
                        return true;
                    }
                    else
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is frozen solid.");
                        return false;
                    }
                },
            }
        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "fell asleep.",
                OnStart = (Pokemon pokemon) =>
                {
                    //Sleep for 1-7 turns
                    pokemon.StatusTime = Random.Range(1, 8);
                    Debug.Log($"Will be sleeping for {pokemon.StatusTime} turns");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.StatusTime <= 0)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} woke up!");
                        pokemon.CureStatus();
                        return true;
                    }

                    --pokemon.StatusTime;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is fast asleep.");
                    return false;
                },
            }
        },
        
        //Volatile Status Conditions 
        {
            ConditionID.confusion,
            new Condition()
            {
                Name = "Confusion",
                StartMessage = "is confused.",
                OnStart = (Pokemon pokemon) =>
                {
                    //Confused for 1-4 turns
                    pokemon.VolatileStatusTime = Random.Range(1, 5);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} turns");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} snapped out of its confusion!");
                        pokemon.CureVolatileStatus();
                        return true;
                    }
                    --pokemon.VolatileStatusTime;

                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused.");
                    //50% to hurt itself
                    if (Random.Range(1, 3) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"It hurts itself in its confusion.");
                        pokemon.UpdateHp(pokemon.MaxHp / 8); //TODO: Use the correct confusion damage formula
                        return false;
                    }
                    return true;
                },
            }
        }
    };
}

public enum ConditionID
{
    none, psn, brn, slp, par, frz,
    confusion
}
