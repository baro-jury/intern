using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController instance;

    private const string HIGH_SCORE = "High Score";
    void _MakeSingleInstance()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void _IsFirstTimePlay()
    {
        if (PlayerPrefs.HasKey("_IsFirstTimePlay") == true)
        {
            PlayerPrefs.SetInt(HIGH_SCORE, 0);
            PlayerPrefs.SetInt("_IsFirstTimePlay", 0);
        }
    }

    void Awake()
    {
        _MakeSingleInstance();
        _IsFirstTimePlay();
    }

    public void _SetHighScore(int score)
    {
        PlayerPrefs.SetInt(HIGH_SCORE, score);
    }

    public int _GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE);
    }
}
