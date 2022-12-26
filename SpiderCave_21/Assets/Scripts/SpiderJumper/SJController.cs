using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJController : MonoBehaviour
{
    public float yForce = 300f;

    private Rigidbody2D body;
    private Animator anim;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Ground")
        {
            anim.SetBool("IsAttacking", false);
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1);

        body.AddForce(new Vector2(0, yForce));
        anim.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(1);

        StartCoroutine(Attack());
    }
}
