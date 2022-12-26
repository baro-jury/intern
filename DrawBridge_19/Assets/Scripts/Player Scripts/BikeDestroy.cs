using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeDestroy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D myBody;
    [SerializeField]
    private float destroySpeed;
    [SerializeField] float angleLimit;

    [SerializeField] GameObject bikeBody;
    [SerializeField]
    private GameObject bikeParts;

    const string SOLID_TAG = "Solid";
    const string LINE_TAG = "Line";
    const string HAZARD_TAG = "Hazard";

    bool bikeDestroyed;

    [SerializeField] BikeDestory_Wheel frontWheel;
    private void Start()
    {
        frontWheel.OnWheelCollide += WheelCollide;
        bikeDestroyed = false;
    }

    Vector3 velocityBeforeCollision;
    private void FixedUpdate()
    {
        velocityBeforeCollision = myBody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collide: " + collision.relativeVelocity.magnitude);

        float angle = transform.eulerAngles.z;
        if (angle >= 180) angle -= 360;
        //Debug.Log("Body " + angle);

        if (collision.gameObject.CompareTag(SOLID_TAG) || collision.gameObject.CompareTag(LINE_TAG))
        {
            //If collide while going to fast, destroy bike
            if (collision.relativeVelocity.magnitude > destroySpeed)
                DestroyBike();
            //If collide head first, destroy bike
            else if (angle < angleLimit)
                DestroyBike();
        }

        if (collision.gameObject.CompareTag(HAZARD_TAG))
        {
            DestroyBike();
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(HAZARD_TAG))
        {
            DestroyBike();
        }
    }

    void WheelCollide(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(SOLID_TAG) || collision.gameObject.CompareTag(LINE_TAG))
        {
            float angle = transform.eulerAngles.z;
            if (angle >= 180) angle -= 360;
            //Debug.Log("Wheel " + angle);

            if (angle < angleLimit)
                DestroyBike();
        }

        if (collision.gameObject.CompareTag(HAZARD_TAG))
            DestroyBike();
    }

    void DestroyBike()
    {
        if (bikeDestroyed) return;
        bikeDestroyed = true;
        
        AudioSource engineAudioSource = gameObject.GetComponent<AudioSource>();
        engineAudioSource.Stop();

        if (!GlobalWinLose.instance.winFlag)
        {
            AudioManager.instance.Play("Crash2");
            AudioManager.instance.Play("Die");
        }

        bikeParts.SetActive(true);
        Rigidbody2D[] childBodies = bikeParts.GetComponentsInChildren<Rigidbody2D>();
        foreach (var body in childBodies)
        {
            body.velocity = velocityBeforeCollision;
        }

        //ChangePersonMass();
        //myBody.constraints = RigidbodyConstraints2D.FreezeAll;

        Destroy(bikeBody);
    }

    void ChangePersonMass()
    {
        myBody.mass = 0.5f;
    }
}
