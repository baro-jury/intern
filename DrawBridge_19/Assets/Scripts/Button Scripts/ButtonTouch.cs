using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ButtonFunction { FallSpike, SpinSpike, MoveGroundUp, MoveGroundUpDown, TurnOnOffLaser }

public class ButtonTouch : MonoBehaviour
{
    [SerializeField] ButtonFunction function;

    [SerializeField] Sprite activatedButton;

    [SerializeField] GameObject[] spikes;
    [SerializeField] GameObject[] movableGrounds, movableGrounds2;
    [SerializeField] GameObject[] lasers;

    [SerializeField] float moveDistance;

    bool activated = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (activated) return;

        if (function == ButtonFunction.FallSpike)
            TouchButtonSpikeFall();
        else if (function == ButtonFunction.SpinSpike)
            TouchButtonSpinSpike();
        else if (function == ButtonFunction.MoveGroundUp)
            TouchButtonMoveGroundUp();
        else if (function == ButtonFunction.MoveGroundUpDown)
            TouchButtonMoveGroundUpDown();
        else if (function == ButtonFunction.TurnOnOffLaser)
            TouchButtonTurnOnOffLaser();

        activated = true;
        //transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f);
        GetComponent<SpriteRenderer>().sprite = activatedButton;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void TouchButtonSpikeFall()
    {
        foreach (GameObject spike in spikes)
            spike.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    IEnumerator SpinSpike(GameObject spike, float goal, float time)
    {
        float distance = Mathf.Abs(moveDistance * Time.fixedDeltaTime / time);

        Quaternion goalRotation = Quaternion.Euler(spike.transform.eulerAngles.x, spike.transform.eulerAngles.y, goal);

        //while (spike.transform.rotation != goalRotation)
        //{
        //    spike.transform.rotation = Quaternion.RotateTowards(spike.transform.rotation, goalRotation, distance);
        //    //Rigidbody2D spikeBody = spike.GetComponent<Rigidbody2D>();
        //    //spikeBody.MoveRotation(spike.transform.eulerAngles.z);

        //    yield return new WaitForSeconds(Time.fixedDeltaTime);
        //}

        Quaternion tempRotation = spike.transform.rotation;
        while (tempRotation != goalRotation)
        {
            tempRotation = Quaternion.RotateTowards(tempRotation, goalRotation, distance);
            Rigidbody2D spikeBody = spike.GetComponent<Rigidbody2D>();
            spikeBody.MoveRotation(tempRotation.eulerAngles.z);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    void TouchButtonSpinSpike()
    {
        foreach (GameObject spike in spikes)
        {
            StartCoroutine(SpinSpike(spike, spike.transform.eulerAngles.z + moveDistance, 0.1f));
        }
    }

    IEnumerator MoveGround(GameObject ground, Vector3 goal, float time)
    {
        float distance = (goal - ground.transform.position).magnitude * Time.fixedDeltaTime / time;
        Vector3 tempPosi = ground.transform.position;

        while (tempPosi != goal)
        {
            tempPosi = Vector3.MoveTowards(tempPosi, goal, distance);
            Rigidbody2D groundBody = ground.GetComponent<Rigidbody2D>();
            groundBody.MovePosition(tempPosi);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    void TouchButtonMoveGroundUp()
    {
        foreach (GameObject movableGround in movableGrounds)
        {
            Vector3 goal = movableGround.transform.position;
            goal.y += moveDistance;

            StartCoroutine(MoveGround(movableGround, goal, 0.1f));
        }
    }

    void TouchButtonMoveGroundUpDown()
    {
        foreach (GameObject movableGround in movableGrounds)
        {
            Vector3 goalDown = movableGround.transform.position;
            goalDown.y -= moveDistance;
            StartCoroutine(MoveGround(movableGround, goalDown, 0.1f));
        }

        foreach (GameObject movableGround2 in movableGrounds2)
        {
            Vector3 goalUp = movableGround2.transform.position;
            goalUp.y += moveDistance;
            StartCoroutine(MoveGround(movableGround2, goalUp, 0.1f));
        }
    }

    void TouchButtonTurnOnOffLaser()
    {
        foreach (GameObject laser in lasers)
        {
            if (laser.activeSelf)
                laser.SetActive(false);
            else laser.SetActive(true);
        }
    }
}
