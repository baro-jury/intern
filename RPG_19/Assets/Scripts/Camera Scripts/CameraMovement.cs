using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform playerPosi;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(playerPosi.position.x, playerPosi.position.y, transform.position.z);
    }
}
