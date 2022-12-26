using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseResume : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseResumeButton;
    [SerializeField]
    private Sprite pauseSprite, playSprite;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PauseResumeGame();
    }

   
    void PauseResumeGame()
    {
        if (Input.GetKeyDown("space"))
        {
            _PauseResume();
        }
    }

    public void _PauseResume()
    {
        if ((!GlobalWinLose.instance.loseFlag) && (!GlobalWinLose.instance.winFlag))
        {
            if (Time.timeScale == 0)
            {
                pauseResumeButton.GetComponent<Image>().sprite = playSprite;
                Time.timeScale = 1;
            }
            else
            {
                pauseResumeButton.GetComponent<Image>().sprite = pauseSprite;
                Time.timeScale = 0;
            }
        }
    }
}
