using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{   
    PartyMemberUI[] memberSlots;

    [SerializeField] Text messageText;

    List<Pokemon> pokemonParty;

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Pokemon> pokemonParty)
    {
        for (int i = 0; i < memberSlots.Length; ++i)
        {
            if (i < pokemonParty.Count)
                memberSlots[i].SetData(pokemonParty[i]);
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        messageText.text = "Choose a Pokemon.";

        this.pokemonParty = pokemonParty;
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < pokemonParty.Count; ++i)
        {
            if (i == selectedMember)
            {
                memberSlots[i].HighlightSelected(true);
            }
            else memberSlots[i].HighlightSelected(false);
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
