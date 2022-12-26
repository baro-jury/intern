using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnimation : MonoBehaviour
{
    private Animator anim;

    private string WALK_PARAMETER = "Walk";

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PlayIdleAnimation();
        PlayWalkAnimation();
    }

    void PlayIdleAnimation()
    {
        anim.SetFloat("Direction Float", 1f / 4 * PlayerMovement.instance.direction);
    }

    void PlayWalkAnimation()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            anim.SetBool(WALK_PARAMETER, true);
        else anim.SetBool(WALK_PARAMETER, PlayerMovement.instance.moving);
    }
}
