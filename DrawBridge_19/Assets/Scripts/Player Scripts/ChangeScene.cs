using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    Camera camera;

    Collider2D[] colliders;

    [SerializeField] Rigidbody2D myBody;
    [SerializeField] BikeMoving movingScript;

    bool touchCheckpoint;

    private void Start()
    {
        camera = Camera.main;

        colliders = GetComponentsInChildren<Collider2D>();
        touchCheckpoint = false;
    }

    private void Update()
    {
        GameObject checkpoint = IsTouchingCheckpoint();
        if (checkpoint != null && !touchCheckpoint)
        {
            StartCoroutine(SceneMoving(checkpoint));
            touchCheckpoint = true;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Checkpoint"))
    //    {
    //        Debug.Log("Checkpoint!");
    //        GameObject checkpoint = collision.gameObject;
    //        StartCoroutine(SceneMoving(checkpoint));
    //    }
    //}

    IEnumerator SceneMoving(GameObject checkpoint)
    {
        movingScript.accelerateForce = 0;
        myBody.velocity = new Vector2(0, myBody.velocity.y);
        Rigidbody2D[] childrenBodies = myBody.GetComponentsInChildren<Rigidbody2D>();
        foreach (var body in childrenBodies)
            body.velocity = new Vector2(0, body.velocity.y);
        checkpoint.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        Vector3 goalPosi = checkpoint.GetComponent<CheckpointInfo>().cameraGoalPosi;
        float deltaTime = Time.deltaTime;
        Time.timeScale = 0;

        yield return camera.GetComponent<CameraMoving>().MoveCameraOverTime(goalPosi, 0.5f, deltaTime);

        movingScript.accelerateForce = checkpoint.GetComponent<CheckpointInfo>().newSpeed;
        touchCheckpoint = false;
        
    }    

    GameObject IsTouchingCheckpoint()
    {
        foreach (var collider in colliders)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Checkpoint"));
            filter.useTriggers = true;
            Collider2D[] result = new Collider2D[1];
            if (collider.OverlapCollider(filter, result) > 0)
                return result[0].gameObject;
        }

        return null;
    }
}
