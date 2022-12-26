using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveForce = 20f;
    public float jumpForce = 500f;
    public float maxVelocity = 4f;

    private bool isGrounded;

    private Rigidbody2D body;
    private Animator anim;

    public AudioSource audioSource;

    [SerializeField]
    private AudioClip jumpClip, pingClip, deadClip;

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
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _TapToWalk();
    }

    void _TapToWalk()
    {
        float xForce = 0f, yForce = 0f;
        float vel = Mathf.Abs(body.velocity.x);
        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0)
        {
            if (vel < maxVelocity)
            {
                if (isGrounded)
                {
                    xForce = moveForce;
                }
                else
                {
                    xForce = moveForce * 1.1f;
                }
            }
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;

            anim.SetBool("IsWalking", true);
        }
        else if (h < 0)
        {
            if (vel < maxVelocity)
            {
                if (isGrounded)
                {
                    xForce = -moveForce;
                }
                else
                {
                    xForce = -moveForce * 1.1f;
                }
            }
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;

            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        if(Input.GetKey(KeyCode.Space)){
            if (isGrounded)
            {
                isGrounded = false;
                audioSource.PlayOneShot(jumpClip);
                yForce = jumpForce;
            }
            
        }
        body.AddForce(new Vector2(xForce, yForce));
    }

    public void _Bounce(float force)
    {
        if (isGrounded)
        {
            isGrounded = false;
            body.AddForce(new Vector2(0, force));
        }
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if(target.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        if (target.gameObject.tag == "Enemy")
        {
            audioSource.PlayOneShot(deadClip);
            //Destroy(gameObject);
            GameplayController.instance._GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Diamond")
        {
            audioSource.PlayOneShot(pingClip);
            //Destroy(target.gameObject);
            //DoorController.instance._DecreaseItem();
        }
        if (target.gameObject.tag == "Bullet")
        {
            audioSource.PlayOneShot(deadClip);
            Destroy(target.gameObject);
            GameplayController.instance._GameOver();
        }
    }
}
