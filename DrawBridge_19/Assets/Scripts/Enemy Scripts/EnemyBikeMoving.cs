using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBikeMoving : MonoBehaviour
{
    private Rigidbody2D myBody;

    [SerializeField] bool moveRight;

    [SerializeField]
    private Collider2D frontWheel, hindWheel;

    public float accelerateForce;
    [SerializeField]
    private float maxSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
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
                if (moveRight)
                    myBody.AddForceAtPosition(Vector2.right * force, frontWheel.transform.position, ForceMode2D.Force);
                else
                    myBody.AddForceAtPosition(Vector2.left * force, frontWheel.transform.position, ForceMode2D.Force);
            }
        }
    }

}
