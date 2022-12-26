using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerController : MonoBehaviour
{
    public float force = 700f;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    IEnumerator Bounce()
    {
        anim.Play("Up");
        yield return new WaitForSeconds(0.5f);
        anim.Play("Down");
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            StartCoroutine(Bounce());
            target.gameObject.GetComponent<PlayerController>()._Bounce(force);
        }
    }
}
