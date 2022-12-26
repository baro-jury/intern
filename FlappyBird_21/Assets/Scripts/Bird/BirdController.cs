using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    public static BirdController instance;

    public float bounce;
    private Rigidbody2D body;
    private Animator anim;
    private bool isAlive;
    private bool fly;
    private GameObject spawner;
    public bool isCrashed = false;
    public int score = 0;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip flyClip, pingClip, deadClip;

    void _MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Awake()
    {
        _MakeInstance();
        isAlive = true;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        spawner = GameplayController.instance.spawner;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _BirdMovement();
    }

    void _BirdMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _FlyButton();
        }
        if (isAlive)
        {
            if (fly)
            {
                fly = false;
                body.velocity = new Vector2(body.velocity.x, bounce);
                audioSource.PlayOneShot(flyClip);
            }
        }
        if (body.velocity.y > 0)
        {
            float angle = 0;
            angle = Mathf.Lerp(0, 90, body.velocity.y / 7);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (body.velocity.y == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            float angle = 0;
            angle = Mathf.Lerp(0, -90, -body.velocity.y / 7);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void _FlyButton()
    {
        fly = true;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "PipeMilestone")
        {
            audioSource.PlayOneShot(pingClip);
            score++;
            GameplayController.instance._SetScore(score);
        }
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Pipe" || target.gameObject.tag == "Ground")
        {
            isCrashed = true;
            if (isAlive)
            {
                isAlive = false;
                audioSource.PlayOneShot(deadClip);
                anim.SetTrigger("Collision");
                Destroy(spawner);
            }
            GameplayController.instance._GameOver(score);
        }
    }
}
