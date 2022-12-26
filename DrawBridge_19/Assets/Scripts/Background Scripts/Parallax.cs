using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    private GameObject cam;
    [SerializeField]
    private float parallaxEffectNum;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ParallaxEffect();
    }

    void ParallaxEffect()
    {
        float posiOffset = cam.transform.position.x * (1 - parallaxEffectNum); //sai so giua vi tri background va camera
        float distance = cam.transform.position.x * parallaxEffectNum;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (posiOffset > startPos + length) startPos += length;
        else if (posiOffset < startPos - length) startPos -= length;
    }
}
