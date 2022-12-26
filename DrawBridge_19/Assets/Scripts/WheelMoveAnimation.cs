using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMoveAnimation : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D myBody;

    // Update is called once per frame
    void Update()
    {
        SpinWheel();
        //FixSuspensionDirection();
    }

    void SpinWheel()
    {
        //Vector3 tempAngle = transform.eulerAngles;
        //tempAngle.z -= myBody.velocity.magnitude;
        //transform.eulerAngles = tempAngle;

        transform.Rotate(new Vector3(0, 0, -myBody.velocity.magnitude * Time.timeScale));
    }

    void FixSuspensionDirection()
    {
        WheelJoint2D wheel = GetComponent<WheelJoint2D>();
        JointSuspension2D sus = wheel.suspension;
        Vector2 facingDir = transform.InverseTransformDirection(wheel.connectedBody.transform.right);
        float angleOffset = Mathf.Atan2(facingDir.y, facingDir.x) * Mathf.Rad2Deg;

        sus.angle = angleOffset + 90;
        wheel.suspension = sus;
    }
}
