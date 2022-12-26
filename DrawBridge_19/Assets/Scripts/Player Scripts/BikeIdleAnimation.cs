using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeIdleAnimation : MonoBehaviour
{
    [SerializeField] GameObject frontWheel, hindWheel;
    Vector3 originalPosi;
    Vector3 frontWheelPosi, hindWheelPosi;

    Vector3 lowPosi, highPosi;
    bool moveDown = true;

    float moveDistance = 0.02f;
    float moveSpeed = 0.005f;

    bool changedNotIdle = false;

    // Start is called before the first frame update
    void Awake()
    {
        originalPosi = transform.localPosition;
        frontWheelPosi = frontWheel.transform.position;
        hindWheelPosi = hindWheel.transform.position;

        lowPosi = highPosi = originalPosi;
        lowPosi.y -= moveDistance;
        highPosi.y += moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime == 0)
        {
            changedNotIdle = false;

            // TODO - sua cho dep hon, check bug
            if (moveDown)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, lowPosi, moveSpeed);
                if (transform.localPosition == lowPosi)
                    moveDown = false;
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, highPosi, moveSpeed);
                if (transform.localPosition == highPosi)
                    moveDown = true;
            }

            if (!GlobalWinLose.instance.winFlag && !GlobalWinLose.instance.loseFlag)
            {
                frontWheel.transform.position = frontWheelPosi;
                hindWheel.transform.position = hindWheelPosi;
            }
        }
        else
        {
            if (!changedNotIdle)
            {
                transform.localPosition = originalPosi;
                frontWheel.transform.position = frontWheelPosi;
                hindWheel.transform.position = hindWheelPosi;
                changedNotIdle = true;
            }
        }
    }
}
