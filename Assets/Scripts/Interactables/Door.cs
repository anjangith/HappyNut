using Pawns;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Animator))]
    public class Door : Interactable {

        private Animator animator;

        public override bool Interact(Player player)
        {
            if (!animator.GetBool("DoorOpened"))
            {
                animator.SetBool("DoorOpened", true);
                return true;
            }
            return false;
        }

        // Use this for initialization
        void Start () {
            animator = GetComponent<Animator>();
        }
	
        // Update is called once per frame
        void Update () {
		
        }
    }
}
