using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalWinLose : MonoBehaviour
{
    public static GlobalWinLose instance;

    [HideInInspector]
    public bool winFlag, loseFlag;

    // Start is called before the first frame update
    void Awake()
    {
        winFlag = false;
        loseFlag = false;

        InstanceCreation();
    }

    void InstanceCreation()
    {
        if (!instance)
            instance = this;
    }

    private void Update()
    {
        //Debug.Log(winFlag + " " + loseFlag);
    }

}
