using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D myBody;

    private void Update()
    {
        if (Controller.instance.playingState != PlayingState.BikeMove) return;
        
        OutOfBound();   
    }

    private void FixedUpdate()
    {
        //Debug.Log(myBody.velocity.x);
        NoSpeed();
    }

    void OutOfBound()
    {
        Vector3 position = myBody.transform.position;

        if (position.x < GlobalPlayer.instance.minX || position.x > GlobalPlayer.instance.maxX ||
            position.y < GlobalPlayer.instance.minY || position.y > GlobalPlayer.instance.maxY)
        {
            GlobalWinLose.instance.loseFlag = true;
        }
    }

    void NoSpeed()
    {
        //if (myBody.velocity.magnitude < minSpeed)
        if (myBody.velocity.x < GlobalPlayer.instance.minSpeed)
        {
            StartCoroutine("NoSpeedTimer");
        }
        else
        {
            StopCoroutine("NoSpeedTimer");
        }
    }

    IEnumerator NoSpeedTimer()
    {
        yield return new WaitForSeconds(GlobalPlayer.instance.timeWait);

        GlobalWinLose.instance.loseFlag = true;
    }

}
