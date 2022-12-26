using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    protected Rigidbody2D myBody;

    private float speed;

    private float width, height;
    private Vector3 positionOnScreen;

    private float bound = 50;
    
    // Start is called before the first frame update
    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();

        width = Camera.main.pixelWidth;
        height = Camera.main.pixelHeight;

        speed = Random.Range(3, 8);

        Move();
    }

    // Update is called once per frame
    protected void Update()
    {
        positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
        OutOfBound();
    }

    protected void Move()
    {
        Vector3 moveVector = transform.up * speed;
        myBody.velocity = moveVector;
    }

    protected void OutOfBound()
    {
        if (positionOnScreen.x < -bound || positionOnScreen.x > width + bound ||
            positionOnScreen.y < -bound || positionOnScreen.y > height + bound)
        {
            Destroy(gameObject);
        }
    }
}
