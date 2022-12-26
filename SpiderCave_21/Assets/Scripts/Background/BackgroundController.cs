using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float width, height;
    private float startPosX, startPosY;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        float distX = cam.transform.position.x * parallaxEffect;
        float distY = cam.transform.position.y * parallaxEffect;
        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        float tempX = cam.transform.position.x * (1 - parallaxEffect);
        float tempY = cam.transform.position.y * (1 - parallaxEffect);
        if (tempX > startPosX + width)
        {
            startPosX += width * 2;
        }
        else if (tempX < startPosX - width)
        {
            startPosX -= width * 2;
        }
        if (tempY > startPosY + height)
        {
            startPosY += height * 2;
        }
        else if (tempY < startPosY - height)
        {
            startPosY -= height * 2;
        }
    }
}
