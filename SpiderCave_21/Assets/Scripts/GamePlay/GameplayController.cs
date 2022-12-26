using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    [SerializeField]
    private GameObject gameoverPanel;

    [SerializeField]
    private GameObject completedPanel;

    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private Button resumeButton;

    void _MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Awake()
    {
        _MakeInstance();
    }

    public void _Pause()
    {
        resumeButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void _Resume()
    {
        resumeButton.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void _GameOver()
    {
        Time.timeScale = 0;
        gameoverPanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void _CompleteLevel()
    {
        Time.timeScale = 0;
        completedPanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void _Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void _GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void _BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
