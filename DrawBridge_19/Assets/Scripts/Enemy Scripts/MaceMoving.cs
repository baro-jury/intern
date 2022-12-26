using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceMoving : MonoBehaviour
{
    [SerializeField] GameObject point1, point2;
    [SerializeField] float speed;

    Rigidbody2D myBody;

    float y1, y2;
    bool toPoint1;
    float direction;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();

        y1 = point1.transform.position.y;
        y2 = point2.transform.position.y;
        toPoint1 = true;
        direction = Mathf.Sign(y1 - y2);
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        if (Mathf.Abs(transform.position.y - y2) > Mathf.Abs(y1 - y2))
        {
            toPoint1 = false;
        }
        else if (Mathf.Abs(transform.position.y - y1) > Mathf.Abs(y1 - y2))
        {
            toPoint1 = true;
        }

        if (Time.deltaTime == 0 && Controller.instance.playingState != PlayingState.Finish)
        {
            if (toPoint1)
            {
                Vector3 temp = transform.position;
                temp.y += speed * direction * Time.unscaledDeltaTime;
                transform.position = temp;
            }
            else
            {
                Vector3 temp = transform.position;
                temp.y -= speed * direction * Time.unscaledDeltaTime;
                transform.position = temp;
            }
        }
        else
        {
            if (toPoint1)
            {
               myBody.MovePosition(new Vector2(transform.position.x, transform.position.y + speed * direction * Time.fixedDeltaTime));
            }
            else
            {
                myBody.MovePosition(new Vector2(transform.position.x, transform.position.y - speed * direction * Time.fixedDeltaTime));
            }
        }
    }

}
