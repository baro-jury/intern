using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new Pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] new string name; //Used "new" to hide inherited member Object.name

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    //Base Stats
    [SerializeField] int hp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;

    public string Name { get => name; }
    public string Description { get => description; }

    public Sprite FrontSprite { get => frontSprite; }
    public Sprite BackSprite { get => backSprite; }

    public PokemonType Type1 { get => type1; }
    public PokemonType Type2 { get => type2; }

    public int Hp { get => hp; }
    public int Attack { get => attack; }
    public int Defense { get => defense; }
    public int SpAttack { get => spAttack; }
    public int SpDefense {get => spDefense; }
    public int Speed { get => speed; }

    public List<LearnableMove> LearnableMoves { get => learnableMoves; }

}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base { get => moveBase; }
    public int Level { get => level; }
}

public enum PokemonType
{
    None,
    Normal, 
    Fire, 
    Water, 
    Grass, 
    Flying, 
    Fighting, 
    Poison, 
    Electric, 
    Ground, 
    Rock, 
    Psychic, 
    Ice, 
    Bug, 
    Ghost, 
    Steel, 
    Dragon, 
    Dark,
    Fairy
}

//Boostable stats only
public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,
    
    //These 2 are not actual stats, they're used to boost the moveAccuracy
    Accuracy,
    Evasion
}

public class TypeChart
{
    static float[][] chart =
    {
        //                      NOR   FIR   WAT   GRA   FIG   FLY   POI   ELE   GRO   ROC   PSY   ICE   BUG   GHO   STE   DRA   DAR   FAI
        /*NOR*/   new float[] {  1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f, 0.5f,   1f,   1f,   1f,   0f, 0.5f,   1f,   1f,   1f },
        /*FIR*/   new float[] {  1f, 0.5f, 0.5f,   2f,   1f,   1f,   1f,   1f,   1f, 0.5f,   1f,   2f,   2f,   1f,   2f, 0.5f,   1f,   1f },
        /*WAT*/   new float[] {  1f,   2f, 0.5f, 0.5f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f, 0.5f,   1f,   1f },
        /*GRA*/   new float[] {  1f, 0.5f,   2f, 0.5f,   1f, 0.5f, 0.5f,   1f,   2f,   2f,   1f,   1f, 0.5f,   1f, 0.5f, 0.5f,   1f,   1f },
        /*FIG*/   new float[] {  2f,   1f,   1f,   1f,   1f, 0.5f, 0.5f,   1f,   1f,   2f, 0.5f,   2f, 0.5f,   0f,   2f,   1f,   2f, 0.5f },
        /*FLY*/   new float[] {  1f,   1f,   1f,   2f,   2f,   1f,   1f, 0.5f,   1f, 0.5f,   1f,   1f,   2f,   1f, 0.5f,   1f,   1f,   1f },
        /*POI*/   new float[] {  1f,   1f,   1f,   2f,   1f,   1f, 0.5f,   1f, 0.5f, 0.5f,   1f,   1f,   1f, 0.5f,   0f,   1f,   1f,   2f },
        /*ELE*/   new float[] {  1f,   1f,   2f, 0.5f,   1f,   2f,   1f, 0.5f,   0f,   1f,   1f,   1f,   1f,   1f,   1f, 0.5f,   1f,   1f },
        /*GRO*/   new float[] {  1f,   2f,   1f, 0.5f,   1f,   1f,   2f,   2f,   1f,   2f,   1f,   1f, 0.5f,   1f,   2f,   1f,   1f,   1f },
        /*ROC*/   new float[] {  1f,   2f,   1f,   1f, 0.5f,   2f,   1f,   1f, 0.5f,   1f,   1f,   2f,   2f,   1f, 0.5f,   1f,   1f,   1f },
        /*PSY*/   new float[] {  1f,   1f,   1f,   1f,   2f,   1f,   2f,   1f,   1f,   1f, 0.5f,   1f,   1f,   1f, 0.5f,   1f,   0f,   1f },
        /*ICE*/   new float[] {  1f, 0.5f, 0.5f,   2f,   1f,   2f,   1f,   1f,   2f,   1f,   1f, 0.5f,   1f,   1f, 0.5f,   2f,   1f,   1f },
        /*BUG*/   new float[] {  1f, 0.5f,   1f,   2f, 0.5f, 0.5f, 0.5f,   1f,   1f,   1f,   2f,   1f,   1f, 0.5f, 0.5f,   1f,   2f, 0.5f },
        /*GHO*/   new float[] {  0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   1f, 0.5f,   1f },
        /*STE*/   new float[] {  1f, 0.5f, 0.5f,   1f,   1f,   1f,   1f, 0.5f,   1f,   2f,   1f,   2f,   1f,   1f, 0.5f,   1f,   1f,   2f },
        /*DRA*/   new float[] {  1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f, 0.5f,   2f,   1f,   0f },
        /*DAR*/   new float[] {  1f,   1f,   1f,   1f, 0.5f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   1f, 0.5f, 0.5f },
        /*FAI*/   new float[] {  1f, 0.5f,   1f,   1f,   2f,   1f, 0.5f,   1f,   1f,   1f,   1f,   1f,   1f,   1f, 0.5f,   2f,   2f,   1f },
    };

    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if (attackType == PokemonType.None || defenseType == PokemonType.None)
            return 1;

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;
        return chart[row][col];
    }
}


