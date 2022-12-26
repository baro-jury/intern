using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMoving : MonoBehaviour
{
    private Rigidbody2D myBody;

    [SerializeField]
    private Collider2D frontWheel, hindWheel;

    public float accelerateForce;
    [SerializeField]
    private float maxSpeed;

    [SerializeField] AudioClip engineSound;
    AudioSource engineAudioSource;
    bool playedEngineSound = false;

    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();

        engineAudioSource = gameObject.AddComponent<AudioSource>();
        engineAudioSource.clip = engineSound;
        engineAudioSource.pitch = 1;
        engineAudioSource.volume = 1;
        engineAudioSource.loop = true;
        engineAudioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (Time.deltaTime != 0)
            if ((!playedEngineSound) && (Controller.instance.audioOn))
            {
                engineAudioSource.Play();
                playedEngineSound = true;
            }
        if (Time.deltaTime == 0)
            if ((playedEngineSound) && (Controller.instance.audioOn))
            {
                engineAudioSource.Stop();
                playedEngineSound = false;
            }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TouchGroundToMove();
    }

    void TouchGroundToMove()
    {
        if (myBody.velocity.magnitude > maxSpeed) return;

        float force;
        float angle = myBody.transform.eulerAngles.z;
        if (angle > 180) angle -= 360;
        if (angle < -15)
        {
            myBody.AddForce(myBody.velocity * -0.02f, ForceMode2D.Impulse);
            force = 0;
        }
        else force = accelerateForce;

        if (frontWheel && hindWheel)
        {
            if (frontWheel.IsTouchingLayers(LayerMask.GetMask("Ground", "Line")) || hindWheel.IsTouchingLayers(LayerMask.GetMask("Ground", "Line")))
            {
                myBody.AddForceAtPosition(Vector2.right * force, frontWheel.transform.position, ForceMode2D.Force);
            }
        }
    }

}
