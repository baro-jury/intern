using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D myBody;

    private Vector3 positionOnScreen;
    private float width, height;
    
    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();

        myBody.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
        height = Camera.main.pixelHeight;
        width = Camera.main.pixelWidth;

        OutOfBound();
    }

    void OutOfBound()
    {
        if (positionOnScreen.x < 0 || positionOnScreen.x > width ||
            positionOnScreen.y < 0 || positionOnScreen.y > height)
        {
            Destroy(gameObject);
        }
    }
}
