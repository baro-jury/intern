using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLose : MonoBehaviour
{
    [SerializeField]
    GameObject darkBackground, homeButton, winObject, loseObject;

    [SerializeField] ParticleSystem[] winningParticles;

    const int MAX_VEHICLE = 8;
    
    // Update is called once per frame
    void Update()
    {
        WinLoseCheck();
    }

    bool winActivated = false;
    bool loseActivated = false;
    void WinLoseCheck()
    {
        if (GlobalWinLose.instance.winFlag)
        {
            if (!winActivated)
            {
                winActivated = true;
                Controller.instance.playingState = PlayingState.Finish;
                StartCoroutine(WaitUntilShowWinPanel(1.8f));

                AudioManager.instance.Play("Firework");
                foreach (ParticleSystem particle in winningParticles)
                    particle.Play();

                Controller.instance.passedLevels[Controller.instance.currentLevel] = true;
                Controller.instance.maxLevel = Mathf.Max(Controller.instance.maxLevel, Controller.instance.currentLevel + 1);
            }
            
        }
        else if (GlobalWinLose.instance.loseFlag)
        {
            if (!loseActivated)
            {
                loseActivated = true;
                Controller.instance.playingState = PlayingState.Finish;
                Time.timeScale = 0;

                darkBackground.SetActive(true);
                AudioManager.instance.Play("Lose");
                StartCoroutine(Controller.instance.ZoomOutPanel(loseObject));
            }
        }
        else if (!GlobalWinLose.instance.winFlag && !GlobalWinLose.instance.loseFlag)
        {
            darkBackground.SetActive(false);
            winObject.SetActive(false);
            loseObject.SetActive(false);
            winActivated = false;
            loseActivated = false;
        }
    }

    IEnumerator WaitUntilShowWinPanel(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0;

        darkBackground.SetActive(true);

        if (Controller.instance.maxVehicle < MAX_VEHICLE)
        {
            int rand = Random.Range(1, 4);
            //Debug.Log(rand);
            if (rand == 1)
            {
                homeButton.SetActive(false);
                
                GameObject vehicleGiftPanel = Instantiate(Resources.Load("Vehicle Gift Panel/" + (Controller.instance.maxVehicle + 1)), transform) as GameObject;
                AudioManager.instance.Play("Gift");
                yield return Controller.instance.ZoomOutPanel(vehicleGiftPanel);
                while (vehicleGiftPanel != null)
                    yield return null;

                homeButton.SetActive(true);
            }
        }

        AudioManager.instance.Play("Win");
        yield return Controller.instance.ZoomOutPanel(winObject);
    }

}
