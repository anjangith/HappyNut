using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : PhysicsObject {

    private bool canClimb = false;
    private bool isClimbing = false;
    public float climbSpeed = 7;
    private bool atTop = false;

    public float walkSpeed = 1;
    private float runModifier = 1;

    private bool interacting = false;
    private float dt = 0;

    
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start() {
        move.y = -1;
        SetGrounded();
    }

    void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move.x = Input.GetAxis("Horizontal");

        if (canClimb)
        {
            //Press z to get on the vine.
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isClimbing = true;
                rb2d.gravityScale = 0;
                rb2d.velocity = Vector2.zero;
                animator.enabled = true;
                animator.SetBool("isClimbing", true);
                return;
            }
            if (isClimbing)
            {
                var moveY = Input.GetAxis("Vertical");
                if (atTop && moveY > 0)
                {
                    moveY = 0;
                }
                animator.speed = Mathf.Clamp(Math.Abs(moveY), 0, 1);
                rb2d.MovePosition(rb2d.position + new Vector2(0, moveY) * climbSpeed * Time.fixedDeltaTime);
                //Press space to get off the vine.
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                    StopClimbing();
                }
                else if(Input.GetKeyDown(KeyCode.Z))
                {
                    StopClimbing();
                }
                else
                {
                    return;
                }
            }
        }

        if (interacting)
        {
            if (Time.time - dt > 0.5)
            {
                walkSpeed *= 3;
                interacting = false;
            }
        }

        HandleSprint();
        HandleTurn();
        SetGrounded();
        Jump();

        animator.enabled = move.magnitude > 0.01f;
        var finalMove = new Vector2(move.x * walkSpeed * runModifier, move.y * walkSpeed * ((runModifier > 1.0f) ? 1.2f : 1));
        rb2d.MovePosition(rb2d.position + finalMove * Time.fixedDeltaTime);

    }

    private void HandleSprint()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if (!animator.GetBool("isRunning"))
            {
                runModifier = 1.5f;
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            if (animator.GetBool("isRunning"))
            {
                runModifier = 1;
                animator.SetBool("isRunning", false);
            }
        }
    }

    /// <summary>
    /// Handles the animation and flipping of sprite for turning left and right.
    /// </summary>
    private void HandleTurn()
    {
        if (move.x > 0.01f)
        {
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (move.x < -0.01f)
        {
            if (spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interacting)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Interactable"))
        {
            var interactable = collision.gameObject.GetComponent<Interactable>();
            if (interactable.Interact(this))
            {
                walkSpeed = walkSpeed / 3;
                interacting = true;
                dt = Time.time;
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            atTop = false;
            canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            if(isClimbing && transform.position.y > collision.transform.position.y)
            {
                atTop = true;
            }
            else
            {
                StopClimbing();
            }
        }
    }

    /// <summary>
    /// Sets all values needed for stop climbing.
    /// </summary>
    private void StopClimbing()
    {
        canClimb = false;
        isClimbing = false;
        rb2d.gravityScale = 9.8f;
        rb2d.velocity = Vector2.zero;
        animator.SetBool("isClimbing", false);

        animator.speed = 1;
    }

    /// <summary>
    /// Handles the key capture for jumping
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (grounded || isClimbing))
        {
            move.y = walkSpeed / 1.4f;
        }
        else
        {
            move.y -= walkSpeed / 15;
            if (move.y < 0 && grounded)
            {
                move.y = 0;
            }
        }
    }

}
