using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerController : MonoBehaviour
{
    [SerializeField] GameObject finger, start, end;

    bool timerStart = false;
    bool timerEnd = false;
    bool started = false;

    float moveTime = 1.5f;
    float speed;

    private void Start()
    {
        speed = (end.transform.position - start.transform.position).magnitude / moveTime * Time.unscaledDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.instance.gameState == GameState.Playing && Controller.instance.playingState == PlayingState.Draw)
        {
            if (!started)
            {
                started = true;
                finger.transform.position = start.transform.position;
                finger.SetActive(true);
            }
        }
        else
        {
            finger.SetActive(false);
            started = false;
            return;
        }

        if (finger.transform.position == end.transform.position)
        {
            if (!timerStart)
                StartCoroutine(Timer(0.5f));
            if (timerEnd)
            {
                finger.transform.position = start.transform.position;
                timerStart = false;
                timerEnd = false;
            }
        }
        else
            finger.transform.position = Vector3.MoveTowards(finger.transform.position, end.transform.position, speed);
    }

    IEnumerator Timer(float time)
    {
        timerStart = true;
        yield return new WaitForSecondsRealtime(time);
        timerEnd = true;
    }
}
