using System;
using System.Collections.Generic;
using Interactables;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Pawns
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Pawn {

        private bool canClimb;
        private bool isClimbing;
        [SerializeField]
        private float climbSpeed = 7;
        private bool atTop;

        [SerializeField]
        private float walkSpeed = 1;
        private float defaultWalkSpeed = 1;
        /// <summary>
        /// 1 means there is no modifier. 1.5 is 50% faster when running.
        /// </summary>
        private float runModifier = 1;

        /// <summary>
        /// A list objects the player can interact with.
        /// </summary>
        private readonly List<Interactable> currentInteractables = new List<Interactable>();
        private bool interacting;
        private float dt;

        public Slider Slider;
        private float currentHealth = 100;
        protected override float CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                currentHealth = value;
                Slider.value = currentHealth;
            }
        }

        /// <summary>
        /// The button you use to interact with objects.
        /// </summary>
        private const KeyCode ActionButton = KeyCode.Z;
    
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        // Use this for initialization
        void Awake()
        {
            defaultWalkSpeed = walkSpeed;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        // Use this for initialization
        void Start() {
            Died += HandleDeath;
        }

        private void HandleDeath(object sender, EventArgs eventArgs)
        {
            GameManager.GameManagerInst.PlayerDied(0);
        }

        void Update()
        {
            if (!Controllable)
            {
                return;
            }
            MoveDirection.x = Input.GetAxis("Horizontal");

            if (canClimb)
            {
                //Press z to get on the vine.
                if (Input.GetKeyDown(ActionButton))
                {
                    StartClimbing();
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
                    else if (Input.GetKeyDown(ActionButton))
                    {
                        StopClimbing();
                    }
                    else
                    {
                        return;
                    }
                }
            }

            HandleInteraction();
            Jump();
            HandleSprint();
            HandleTurn();


            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            animator.enabled = MoveDirection.magnitude > 0.01f;
            var finalMove = new Vector2(MoveDirection.x * walkSpeed * runModifier, MoveDirection.y * walkSpeed * (runModifier > 1.0f ? 1.2f : 1));
            rb2d.MovePosition(rb2d.position + finalMove * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Handles the player interacting with objects. Player interacts with first interactable it finds.
        /// </summary>
        private void HandleInteraction()
        {
            if (currentInteractables.Count > 0)
            {
                if (Input.GetKeyDown(ActionButton))
                {
                    if(currentInteractables[0].Interact(this))
                    {
                        Interacted();
                        currentInteractables.RemoveAt(0);
                    }
                }
            }

            if (interacting)
            {
                if (Time.time - dt > 0.5f)
                {
                    walkSpeed = defaultWalkSpeed;
                    interacting = false;
                }
            }

        }

        // Update is called once per frame
        void FixedUpdate()
        {

            SetGrounded();

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
            if (MoveDirection.x > 0.01f)
            {
                if (spriteRenderer.flipX)
                {
                    spriteRenderer.flipX = false;
                }
            }
            else if (MoveDirection.x < -0.01f)
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
                if (interactable.Immediate && interactable.Interact(this))
                {
                    Debug.Log("Player interacted with " + interactable.name);
                    Interacted();
                }
                else if (!interactable.Immediate)
                {
                    currentInteractables.Add(interactable);
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

            if (collision.gameObject.CompareTag("Interactable"))
            {
                var interactable = collision.gameObject.GetComponent<Interactable>();
                currentInteractables.Remove(interactable);
            }
            else if (collision.gameObject.CompareTag("Ladder"))
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

        private void Interacted()
        {
            walkSpeed = defaultWalkSpeed / 1.2f;
            interacting = true;
            dt = Time.time;
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
        /// Sets all values needed for start climbing.
        /// </summary>
        private void StartClimbing()
        {
            isClimbing = true;
            rb2d.gravityScale = 0;
            rb2d.velocity = Vector2.zero;
            animator.enabled = true;
            animator.SetBool("isClimbing", true);
            MoveDirection.y = 0;
            MoveDirection.x = 0;
        }

        /// <summary>
        /// Handles the key capture for jumping
        /// </summary>
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && (grounded || isClimbing))
            {
                MoveDirection.y = walkSpeed / 1.1f;
            }
            else
            {
                MoveDirection.x = MoveDirection.x * 1.4f;
                MoveDirection.y -= walkSpeed / 25;
                if (MoveDirection.y < 0 && grounded)
                {
                    MoveDirection.y = 0;
                }
            }
        }

    }
}
