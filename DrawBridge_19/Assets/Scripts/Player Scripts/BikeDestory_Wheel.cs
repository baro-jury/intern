using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeDestory_Wheel : MonoBehaviour
{
    public event Action<Collision2D> OnWheelCollide;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnWheelCollide(collision);
    }
}
