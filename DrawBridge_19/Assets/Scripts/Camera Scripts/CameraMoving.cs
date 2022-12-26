using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    private Transform playerPosition;

    [SerializeField]
    private float maxX;

    private float distanceFromPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        distanceFromPlayer = transform.position.x - playerPosition.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = Camera.main.transform.position;
        temp.x = playerPosition.position.x + distanceFromPlayer;
        if (temp.x <= maxX)
            transform.position = temp;
    }

    public IEnumerator MoveCameraOverTime(Vector3 goalPosi, float time, float deltaTime)
    {   
        Controller.instance.playingState = PlayingState.CameraMove;

        Vector3 distance = goalPosi - transform.position;
        Vector3 currentDistance = distance;
        
        Vector3 speed = Vector3.zero;
        float frame = time / deltaTime;
        while (currentDistance.x > Mathf.Epsilon || currentDistance.y > Mathf.Epsilon || currentDistance.z > Mathf.Epsilon)
        {
            speed += 2 * distance / (frame * frame);
            transform.position += speed;

            //transform.position += distance * deltaTime / time;
            currentDistance = goalPosi - transform.position;
            yield return new WaitForSecondsRealtime(deltaTime);
        }
        transform.position = goalPosi;

        Controller.instance.playingState = PlayingState.Draw;
    }

}
