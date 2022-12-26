using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] Text levelNumber;

    public void _SelectLevel()
    {
        int number = int.Parse(levelNumber.text);
        Controller.instance.currentLevel = number;

        Controller.instance._FromLevelMenuToCurrentLevel();
    }
}
