using UnityEngine;

namespace Pawns
{
    public class Movable: Pawn
    {
        [SerializeField]
        protected float walkSpeed = 1;
        /// <summary>
        /// 1 means there is no modifier. 1.5 is 50% faster when running.
        /// </summary>
        protected float runModifier = 1;

        protected void MoveHorizontal(bool moveLeft)
        {
            if (moveLeft)
            {
                MoveDirection.x = -1;
            }
            else
            {
                MoveDirection.x = 1;
            }
        }

        protected void MoveVertical(bool moveLeft)
        {
            if (moveLeft)
            {
                MoveDirection.x = -1;
            }
            else
            {
                MoveDirection.x = 1;
            }
        }

        // Use this for initialization
        protected virtual void Start () {
		    MoveHorizontal(true);
        }
	
        // Update is called once per frame
        protected virtual void Update () {
            var finalMove = new Vector2(MoveDirection.x * walkSpeed * runModifier, MoveDirection.y * walkSpeed * (runModifier > 1.0f ? 1.2f : 1));
            rb2d.MovePosition(rb2d.position + finalMove * Time.fixedDeltaTime);
        }
    }
}
