using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemonParty;

    public List<Pokemon> _PokemonParty { get => pokemonParty; }

    private void Start()
    {
        foreach (var pokemon in pokemonParty)
        {
            pokemon.Init();
        }
    }

    public Pokemon GetHealthyPokemon()
    {
        return pokemonParty.FirstOrDefault(x => x.Hp > 0);
    }

    public Pokemon GetHealthyPokemon(ref int index)
    {
        Pokemon result = pokemonParty.FirstOrDefault(x => x.Hp > 0);
        index = pokemonParty.FindIndex(x => x.Equals(result));

        return result;
    }
}
