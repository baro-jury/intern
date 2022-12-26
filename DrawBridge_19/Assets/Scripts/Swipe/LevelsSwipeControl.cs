using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsSwipeControl : MonoBehaviour
{
    [SerializeField] Swipe swipeControl;
    Vector3 desiredPosition;

    int totalGrids;
    int currentGrid;

    private void Start()
    {
        desiredPosition = transform.position;
        totalGrids = GlobalSwipeValue.totalGrids;
        currentGrid = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (swipeControl.SwipeLeft)
        {
            if (currentGrid < totalGrids - 1)
            {
                desiredPosition += new Vector3(-10.8f, 0f, 0f);
                ++currentGrid;
            }
        }
        else if (swipeControl.SwipeRight)
        {
            if (currentGrid > 0)
            {
                desiredPosition += new Vector3(10.8f, 0f, 0f);
                --currentGrid;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, 10.8f * Time.unscaledDeltaTime / GlobalSwipeValue.time);
    }
}
