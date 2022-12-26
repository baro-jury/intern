using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    Vector3 rotationVector;
    Rigidbody2D myBody;

    [SerializeField] float moveDistance;
    float time = 0.1f;
    bool moved;

    private void Awake()
    {
        float rotationAngle = transform.eulerAngles.z * Mathf.Deg2Rad;
        rotationVector = new Vector3(-Mathf.Sin(rotationAngle), Mathf.Cos(rotationAngle));

        myBody = GetComponent<Rigidbody2D>();

        moved = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!moved)
            StartCoroutine(MoveSpring());
    }

    IEnumerator MoveSpring()
    {
        moved = true;

        Vector3 goal = transform.position + rotationVector * moveDistance;
        float deltaDistance = moveDistance * Time.fixedDeltaTime / time;
        Vector3 originalPosi = transform.position;
        Vector3 tempPosi = transform.position;

        while (tempPosi != goal)
        {
            tempPosi = Vector3.MoveTowards(tempPosi, goal, deltaDistance);
            myBody.MovePosition(tempPosi);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        //while (tempPosi != originalPosi)
        //{
        //    tempPosi = Vector3.MoveTowards(tempPosi, originalPosi, deltaDistance);
        //    myBody.MovePosition(tempPosi);

        //    yield return new WaitForSeconds(Time.fixedDeltaTime);
        //}

        //moved = false;
    }
}
