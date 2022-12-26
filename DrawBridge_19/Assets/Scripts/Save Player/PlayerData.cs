using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentLevel;
    public string currentVehicle;

    public int maxLevel;
    public bool[] passedLevels;
    public int maxVehicle;

    public bool bgmOn;
    public bool audioOn;
    
    public PlayerData (Controller data)
    {
        currentLevel = data.currentLevel;
        currentVehicle = data.currentVehicle;

        maxLevel = data.maxLevel;
        passedLevels = data.passedLevels;
        maxVehicle = data.maxVehicle;

        bgmOn = data.bgmOn;
        audioOn = data.audioOn;
    }
}
