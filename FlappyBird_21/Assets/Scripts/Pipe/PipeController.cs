using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(BirdController.instance != null)
        //{
            if (BirdController.instance.isCrashed)
            {
                Destroy(GetComponent<PipeController>());
            }
        //}
        
        _PipeMovement();
    }

    void _PipeMovement()
    {
        Vector3 pipePos = transform.position;
        pipePos.x -= speed * Time.deltaTime;
        transform.position = pipePos;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "Destroy")
        {
            Destroy(gameObject);
        }
    }
}
