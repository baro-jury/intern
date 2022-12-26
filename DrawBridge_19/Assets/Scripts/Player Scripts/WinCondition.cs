using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    Collider2D[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTouchingGoal())
        {
            GlobalWinLose.instance.winFlag = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Goal!");
        TouchGoal(collision);
    }

    void TouchGoal(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            GlobalWinLose.instance.winFlag = true;
        }
    }

    bool IsTouchingGoal()
    {
        foreach (var collider in colliders)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Goal"));
            filter.useTriggers = true;
            Collider2D[] result = new Collider2D[1];
            if (collider.OverlapCollider(filter, result) > 0)
            {
                return true;
            }
        }

        return false;
    }
}
