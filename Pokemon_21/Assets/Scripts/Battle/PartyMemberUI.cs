using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Pokemon _pokemon;

    [SerializeField] Color highlightedColor;
    Image background;
    [SerializeField] Sprite normalBackground, highlightedBackground;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lv " + pokemon.Level;
        hpBar.SetHP((float)pokemon.Hp / pokemon.MaxHp);

        background = GetComponent<Image>();
    }

    public void HighlightSelected(bool selected)
    {
        if (selected)
        {
            nameText.color = highlightedColor;
            levelText.color = highlightedColor;
            background.sprite = highlightedBackground;
        }
        else
        {
            nameText.color = Color.black;
            levelText.color = Color.black;
            background.sprite = normalBackground;
        }
    }

}
