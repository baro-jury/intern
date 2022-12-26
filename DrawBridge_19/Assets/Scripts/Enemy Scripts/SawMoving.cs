using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMoving : MonoBehaviour
{
    [SerializeField] GameObject point1, point2;
    [SerializeField] float speed;

    Rigidbody2D myBody;

    float x1, x2;
    bool toPoint1;
    float direction;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();

        x1 = point1.transform.position.x;
        x2 = point2.transform.position.x;
        toPoint1 = true;
        direction = Mathf.Sign(x1 - x2);
    }

    private void Update()
    {
        Rotate();
        Move();
    }

    void Move()
    {
        if (Mathf.Abs(transform.position.x - x2) > Mathf.Abs(x1 - x2))
        {
            toPoint1 = false;
        }
        else if (Mathf.Abs(transform.position.x - x1) > Mathf.Abs(x1 - x2))
        {
            toPoint1 = true;
        }

        if (Time.deltaTime == 0 && Controller.instance.playingState != PlayingState.Finish)
        {
            if (toPoint1)
            {
                Vector3 temp = transform.position;
                temp.x += speed * direction * Time.unscaledDeltaTime;
                transform.position = temp;
            }
            else
            {
                Vector3 temp = transform.position;
                temp.x -= speed * direction * Time.unscaledDeltaTime;
                transform.position = temp;
            }
        }
        else
        {
            if (toPoint1)
            {
                myBody.MovePosition(new Vector2(transform.position.x + speed * direction * Time.fixedDeltaTime, transform.position.y));
            }
            else
            {
                myBody.MovePosition(new Vector2(transform.position.x - speed * direction * Time.fixedDeltaTime, transform.position.y));
            }
        }
    }

    void Rotate()
    {
        if (Time.deltaTime == 0 && Controller.instance.playingState != PlayingState.Finish)
        {
            Vector3 temp = transform.eulerAngles;
            temp.z -= 180 * Time.unscaledDeltaTime;
            transform.eulerAngles = temp;
        }
        else
        {
            myBody.MoveRotation(transform.eulerAngles.z - 180 * Time.fixedDeltaTime);
        }
    }
}
