using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private bool isMoving;
    private Vector2 input;

    [SerializeField] LayerMask solidObjectLayer;
    [SerializeField] LayerMask interactableLayer;

    private Animator anim;

    [SerializeField] LayerMask longGrassLayer;

    public event Action OnEncountered;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        MoveInput();
        SetAnimParameters();

        if (Input.GetKeyDown(KeyCode.Z))
            Interact();
    }

    void MoveInput()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //remove diagonal movement
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                var targetPosi = transform.position;
                targetPosi.x += input.x;
                targetPosi.y += input.y;

                if (IsWalkable(targetPosi))
                    StartCoroutine(Move(targetPosi));
            }
        }
    }

    IEnumerator Move(Vector3 targetPosi)
    {
        isMoving = true;

        while ((targetPosi - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosi, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosi;
        isMoving = false;

        CheckForEncounter();
    }

    private bool IsWalkable(Vector3 targetPosi)
    {
        if (Physics2D.OverlapCircle(targetPosi, 0.3f, solidObjectLayer | interactableLayer))
        {
            return false;
        }
        return true;
    }

    void SetAnimParameters()
    {
        if (input != Vector2.zero)
        {
            anim.SetFloat("moveX", input.x);
            anim.SetFloat("moveY", input.y);
        }

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            anim.SetBool("isMoving", true);
        else anim.SetBool("isMoving", isMoving);
    }

    void Interact()
    {
        Vector3 facingDir = new Vector3(anim.GetFloat("moveX"), anim.GetFloat("moveY"));
        Vector3 interactPosi = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPosi, Color.green, 0.5f);

        if (Physics2D.OverlapCircle(interactPosi, 0.3f, interactableLayer))
            Debug.Log("Interact");
    }

    void CheckForEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.3f, longGrassLayer))
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                //Debug.Log("Encounter a Pokemon");
                anim.SetBool("isMoving", false);
                OnEncountered();
            }
        }
    }
}
