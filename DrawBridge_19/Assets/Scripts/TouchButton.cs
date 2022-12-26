using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TouchButtonDestroyGround(collision);
    }

    private void TouchButtonDestroyGround(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Temp Ground");
            foreach (GameObject obj in objects)
            {
                Destroy(obj);
            }
            Destroy(collision.gameObject);
        }
    }
}
