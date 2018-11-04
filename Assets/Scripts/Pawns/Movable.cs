using System;
using UnityEngine;

namespace Pawns
{
    public class Movable: Pawn
    {
        [SerializeField]
        protected float walkSpeed = 1;
        [SerializeField]
        protected float defaultWalkSpeed;
        /// <summary>
        /// 1 means there is no modifier. 1.5 is 50% faster when running.
        /// </summary>
        protected float runModifier = 1;

        protected bool walkingLeft = true;

        /// <summary>
        /// These variables tell the enemy if he is near a cliff so he can turn around and walk away if he's walking.
        /// </summary>
        [NonSerialized]
        public bool CliffOnLeft;
        private bool nearCliff;
        public bool NearCliff {
            get { return nearCliff; }
            set {
                nearCliff = value;
                if(CliffOnLeft)
                {
                    MoveHorizontal(false);
                }
                else
                {
                    MoveHorizontal(true);
                }
            }
        }
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        // Use this for initialization
        void Awake()
        {
            defaultWalkSpeed = walkSpeed;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected void MoveHorizontal(bool moveLeft)
        {
            walkingLeft = moveLeft;
            if (moveLeft)
            {
                MoveDirection.x = -1;
            }
            else
            {
                MoveDirection.x = 1;
            }
            //HandleTurn();
            animator.SetBool("isHit", true);
        }

        // Use this for initialization
        protected virtual void Start () {
		    MoveHorizontal(true);
        }
	
        // Update is called once per frame
        protected virtual void Update () {
            if(Paused || !Controllable)
            {
                return;
            }
            var finalMove = new Vector2(MoveDirection.x * walkSpeed * runModifier, MoveDirection.y * walkSpeed * (runModifier > 1.0f ? 1.2f : 1));
            rb2d.MovePosition(rb2d.position + finalMove * Time.fixedDeltaTime);
            animator.SetBool("isWalking", true);
        }

        private bool WalkingOffCliff()
        {
            return nearCliff;
        }

        /// <summary>
        /// Handles the animation and flipping of sprite for turning left and right.
        /// </summary>
        private void HandleTurn()
        {
            if (MoveDirection.x > 0.01f)
            {
                transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (!spriteRenderer.flipX)
                {
                    //spriteRenderer.flipX = true;
                }
            }
            else if (MoveDirection.x < -0.01f)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (spriteRenderer.flipX)
                {
                    //spriteRenderer.flipX = false;
                }
            }
        }
    }
}
