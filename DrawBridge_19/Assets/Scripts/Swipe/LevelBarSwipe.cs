using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBarSwipe : MonoBehaviour
{
    [SerializeField] Swipe swipeControl;
    [SerializeField] Transform startPoint, endPoint;

    Vector3 desiredPosition;
    int currentGrid;
    int totalGrids;
    
    // Start is called before the first frame update
    void Start()
    {
        currentGrid = 0;
        totalGrids = GlobalSwipeValue.totalGrids;
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, currentGrid * 1f / (totalGrids - 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (swipeControl.SwipeLeft)
        {
            if (currentGrid < totalGrids - 1)
                ++currentGrid;
        }
        else if (swipeControl.SwipeRight)
        {
            if (currentGrid > 0)
                --currentGrid;
        }

        desiredPosition = Vector3.Lerp(startPoint.position, endPoint.position, currentGrid * 1f / (totalGrids - 1));
        float distance = (startPoint.position - endPoint.position).magnitude / (totalGrids - 1f);
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, distance * Time.unscaledDeltaTime / GlobalSwipeValue.time);
    }
}
