using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    private bool canClimb = false;
    private bool isClimbing = false;
    public float climbSpeed = 7;

    public float walkSpeed = 1;

    private bool interacting = false;
    private float dt = 0;

    /// <summary>
    /// Gets added to position every frame.
    /// </summary>
    private Vector3 move;

    private Rigidbody2D rgbd2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rgbd2D = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var localMove = new Vector2(Input.GetAxis("Horizontal"), 0) * walkSpeed;

        if (canClimb)
        {
            //Press z to get on the vine.
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isClimbing = true;
                rgbd2D.gravityScale = 0;
                rgbd2D.velocity = Vector2.zero;
            }
            if (isClimbing)
            {
                rgbd2D.MovePosition(rgbd2D.position + new Vector2(0, Input.GetAxis("Vertical")) * climbSpeed * Time.fixedDeltaTime);
                //Press space to get off the vine.
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StopClimbing();
                }
                return;
            }
        }

        var grounded = move.y == 0;
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }
        else if (move.y > 0)
        {
            move.y -= walkSpeed / 10 ;
            if (move.y < 0)
            {
                move.y = 0;
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

        HandleTurn(localMove);

        localMove.y = move.y;
        rgbd2D.AddRelativeForce(localMove, ForceMode2D.Impulse);
        //rgbd2D.MovePosition(rgbd2D.position + localMove * Time.fixedDeltaTime);
        
    }

    /// <summary>
    /// Handles the animation and flipping of sprite for turning left and right.
    /// </summary>
    /// <param name="localMove">Current movement calculated in update.</param>
    private void HandleTurn(Vector3 localMove)
    {
        if (localMove.x > 0.01f)
        {
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (localMove.x < -0.01f)
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
            if (interactable.Interact())
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
            canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            StopClimbing();
        }
    }

    /// <summary>
    /// Sets all values needed for stop climbing.
    /// </summary>
    private void StopClimbing()
    {
        canClimb = false;
        isClimbing = false;
        rgbd2D.gravityScale = 9.8f;
    }

    private void Jump()
    {
        move.y = walkSpeed * 3.0f;
    }
}
