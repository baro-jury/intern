using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text dialogText;
    [SerializeField] int letterPerSecond;
    [SerializeField] Color highlightedColor;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    //public IEnumerator TypeDialog(string dialog)
    //{
    //    dialogText.text = "";
    //    foreach (var letter in dialog)
    //    {
    //        dialogText.text += letter;
    //        yield return new WaitForSeconds(1f / letterPerSecond);
    //    }
    //}

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";

        IEnumerator skipTextCoroutine = SkipText(dialog);
        StartCoroutine(skipTextCoroutine);

        foreach (var letter in dialog)
        {
            if (dialogText.text.Equals(dialog)) break;

            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }

        StopCoroutine(skipTextCoroutine);
        yield return new WaitForSeconds(Time.deltaTime);
    }

    IEnumerator SkipText(string dialog)
    {
        yield return new WaitForSeconds(Time.deltaTime);
        while (!Input.GetKeyDown(KeyCode.Z))
            yield return null;

        SetDialog(dialog);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if (i == selectedAction)
                actionTexts[i].color = highlightedColor;
            else actionTexts[i].color = Color.black;
        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == selectedMove)
                moveTexts[i].color = highlightedColor;
            else moveTexts[i].color = Color.black;
        }

        ppText.text = $"PP {move.Pp}/{move.Base.Pp}";
        typeText.text = move.Base.Type.ToString();

        if (move.Pp == 0)
            ppText.color = Color.red;
        else ppText.color = Color.black;
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < moves.Count)
                moveTexts[i].text = moves[i].Base.Name;
            else moveTexts[i].text = "-";
        }
    }

}
