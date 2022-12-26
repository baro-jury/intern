using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayer : MonoBehaviour
{
    public static GlobalPlayer instance;

    public float minX, maxX, minY, maxY;
    public float minSpeed, timeWait;

    private void Awake()
    {
        CreateInstance();
    }

    void CreateInstance()
    {
        if (!instance)
            instance = this;
    }

}
