using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    public static LoadLevel instance;

    GameObject levelPrefab;
    GameObject vehiclePrefab;

    [SerializeField] GameObject grid;
    [SerializeField] GameObject vehicleParent;

    GameObject level, vehicle;

    [SerializeField] Text[] levelNames;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    //void Start()
    //{
    //    Time.timeScale = 0;
    //    LoadingLevel(Controller.instance.currentLevel);
    //}

    public void StartNewLevel()
    {
        Time.timeScale = 0;
        
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Line");
        foreach (GameObject line in lines)
            Destroy(line);
        
        Destroy(level);
        Destroy(vehicle);

        GlobalWinLose.instance.winFlag = false;
        GlobalWinLose.instance.loseFlag = false;

        Controller.instance.playingState = PlayingState.Draw;
    }

    public void LoadingLevel(int currentLevel)
    {
        levelPrefab = Resources.Load<GameObject>("Map/" + currentLevel);
        level = Instantiate(levelPrefab, grid.transform);

        vehiclePrefab = Resources.Load<GameObject>("Vehicle/" + Controller.instance.currentVehicle);
        vehicle = Instantiate(vehiclePrefab, vehicleParent.transform, false);

        ChangeLevelNameText(currentLevel);
    }

    public void _ToNextLevel()
    {
        Controller.instance.PlayButtonSound();

        ++Controller.instance.currentLevel;
        Controller.instance.maxLevel = Mathf.Max(Controller.instance.maxLevel, Controller.instance.currentLevel);

        if (Resources.Load<GameObject>("Map/" + Controller.instance.currentLevel) == null)
        {
            --Controller.instance.currentLevel;
            return;
        }

        StartNewLevel();
        LoadingLevel(Controller.instance.currentLevel);
    }

    public void _RestartLevel()
    {
        Controller.instance.PlayButtonSound();

        StartNewLevel();
        LoadingLevel(Controller.instance.currentLevel);
    }

    void ChangeLevelNameText(int level)
    {
        foreach (Text name in levelNames)
            name.text = "Level " + level;
    }
}
