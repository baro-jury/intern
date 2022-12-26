using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyButtonController : MonoBehaviour
{
    Vector2 originalPos;

    [SerializeField] GameObject firstVehicle;

    private void Awake()
    {
        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetPosition();
    }

    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        SetPosition();
        StartCoroutine(Wiggling());
    }

    void SetPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 pos = originalPos;

        if (Screen.height / (Screen.width / 9f) < 16)
        {
            //Pivot in the middle
            //pos.y = pos.y * 4.8f / Camera.main.orthographicSize - rectTransform.rect.height / 2f * (4.8f / Camera.main.orthographicSize - 1f);

            //Pivot in the bottom
            pos.y = pos.y * 4.8f / Camera.main.orthographicSize;

            rectTransform.anchoredPosition = pos;
        }

        GameObject currentVehicle = GameObject.FindWithTag("Player");
        if (currentVehicle)
        {
            //Debug.Log("Current: " + Camera.main.WorldToScreenPoint(currentVehicle.transform.position).y);
            //Debug.Log("First: " + Camera.main.WorldToScreenPoint(firstVehicle.transform.position).y);
            
            pos.y -= (Camera.main.WorldToScreenPoint(currentVehicle.transform.position).y 
                - Camera.main.WorldToScreenPoint(firstVehicle.transform.position).y);
            rectTransform.anchoredPosition = pos;
        }
    }

    IEnumerator Wiggling()
    {
        float speed = 300f;

        while (true)
        {
            yield return new WaitForSecondsRealtime(2f);
            
            for (int i = 0; i < 5; ++i)
            {
                yield return Rotate(10f, speed);
                yield return Rotate(-10f, speed);
            }
            yield return Rotate(0f, speed);

            yield return new WaitForSecondsRealtime(2f);
        }
    }

    IEnumerator Rotate(float goalAngle, float speed)
    {
        Quaternion goal = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, goalAngle);

        while (Quaternion.Angle(transform.rotation, goal) > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goal, speed * Time.unscaledDeltaTime);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
    }
}
