using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWController : MonoBehaviour
{
    public float speed = 1f;
    private bool isCollided, checkEndRoad;

    [SerializeField]
    private Transform changeDirPos;

    private Rigidbody2D body;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Move();
    }

    void _Move()
    {
        body.velocity = new Vector2(transform.localScale.x, 0) * speed;

        checkEndRoad = Physics2D.Linecast(transform.position, changeDirPos.position, 1 << LayerMask.NameToLayer("Ground"));
        isCollided = Physics2D.Linecast(transform.position, changeDirPos.position, 1 << LayerMask.NameToLayer("Obstacle"));
        if (isCollided || !checkEndRoad)
        {
            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;
        }
    }
}
