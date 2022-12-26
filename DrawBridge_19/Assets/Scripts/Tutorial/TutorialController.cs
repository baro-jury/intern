using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    
    // Update is called once per frame
    void Update()
    {
        if (Controller.instance.gameState == GameState.Playing || Controller.instance.gameState == GameState.HomeToPlaying)
            tutorial.SetActive(true);
        else tutorial.SetActive(false);
    }
}
