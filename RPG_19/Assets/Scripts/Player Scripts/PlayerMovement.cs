using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    static public PlayerMovement instance;

    [SerializeField]
    private float step;

    private Vector3 goalPosi;
    [HideInInspector]
    public bool moving = false;
    [HideInInspector]
    public int direction;

    public const int RIGHT_INT = 0;
    public const int UP_INT = 1;
    public const int LEFT_INT = 2;
    public const int DOWN_INT = 3;

    [SerializeField]
    private Collider2D solidObject;

    // Start is called before the first frame update
    void Awake()
    {
        goalPosi = transform.position;
        direction = 3;
        InstanceCreation();
    }

    void InstanceCreation()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(ObjectTouching(transform.position, direction));
        MoveInput();
    }

    private void MoveInput()
    {
        if (!moving)
        {
            goalPosi = transform.position;
            if (Input.GetButton("Vertical"))
            {
                goalPosi.y += Input.GetAxisRaw("Vertical");
                if (TouchingSolidTile(goalPosi)) goalPosi.y -= Input.GetAxisRaw("Vertical");
                if (Input.GetAxisRaw("Vertical") < 0) direction = DOWN_INT;
                else direction = UP_INT;
                moving = true;
            }
            else if (Input.GetButton("Horizontal"))
            {
                goalPosi.x += Input.GetAxisRaw("Horizontal");
                if (TouchingSolidTile(goalPosi)) goalPosi.x -= Input.GetAxisRaw("Horizontal");
                if (Input.GetAxisRaw("Horizontal") < 0) direction = LEFT_INT;
                else direction = RIGHT_INT;
                moving = true;
            }
        }
        else
        {
            SmoothMove(direction);
        }
    }

    private void SmoothMove(int dir)
    {
        if (dir == UP_INT)
        {
            transform.position += new Vector3(0, step, 0);
            if (transform.position.y >= goalPosi.y)
            {
                transform.position = goalPosi;
                moving = false;
            }
        }
        else if (dir == DOWN_INT)
        {
            transform.position -= new Vector3(0, step, 0);
            if (transform.position.y <= goalPosi.y)
            {
                transform.position = goalPosi;
                moving = false;
            }
        }
        else if (dir == RIGHT_INT)
        {
            transform.position += new Vector3(step, 0, 0);
            if (transform.position.x >= goalPosi.x)
            {
                transform.position = goalPosi;
                moving = false;
            }
        }
        else if (dir == LEFT_INT)
        {
            transform.position -= new Vector3(step, 0, 0);
            if (transform.position.x <= goalPosi.x)
            {
                transform.position = goalPosi;
                moving = false;
            }
        }
    }
    
    private bool TouchingSolidTile(Vector3 goalPosi)
    {
        if (Physics2D.OverlapCircle(goalPosi, 0.3f))
        {
            return true;
        }
        return false;
    }

    private GameObject ObjectTouching(Vector3 posi, int direction)
    {
        Vector3 objectPosi = new Vector3();
        if (direction == UP_INT) objectPosi = new Vector3(posi.x, posi.y + 1, 0);
        else if (direction == DOWN_INT) objectPosi = new Vector3(posi.x, posi.y - 1, 0);
        else if (direction == LEFT_INT) objectPosi = new Vector3(posi.x - 1, posi.y, 0);
        else if (direction == RIGHT_INT) objectPosi = new Vector3(posi.x + 1, posi.y, 0);

        Collider2D collider = Physics2D.OverlapCircle(objectPosi, 0.3f);
        if (!collider) return null;
        else return collider.gameObject;
    }
}
