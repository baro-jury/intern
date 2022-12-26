using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private Button resumeButton;

    [SerializeField]
    private Text scoreText, endScoreText, bestScoreText;

    [SerializeField]
    private GameObject gameoverPanel;

    public GameObject spawner;

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
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void _TapToStart()
    {
        //SceneManager.LoadScene(1); //build index cua scene
        SceneManager.LoadScene("GamePlay");
    }

    public void _TapToPlay()
    {
        Time.timeScale = 1;
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void _TapToFly()
    {
        BirdController.instance._FlyButton();
    }

    public void _SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void _SetEndScore(int score)
    {
        endScoreText.text = score.ToString();
    }

    public void _SetBestScore(int score)
    {
        bestScoreText.text = score.ToString();
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

    public void _GameOver(int score)
    {
        gameoverPanel.SetActive(true);
        endScoreText.text = score.ToString();
        if (score > DataController.instance._GetHighScore())
        {
            DataController.instance._SetHighScore(score);
        }
        bestScoreText.text = DataController.instance._GetHighScore().ToString();
        pauseButton.gameObject.SetActive(false);
    }

    public void _GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void _Replay()
    {
        //Application.LoadLevel("GamePlay"); //game co 1 level(screen)
        //Application.LoadLevel(Application.loadedLevel); //game co nhieu level, replay level hien tai
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
