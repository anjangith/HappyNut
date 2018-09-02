using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : Interactable {

    private Animator animator;

    //public override bool Interact()
    //{
    //    if(!animator.GetBool("DoorOpened"))
    //    {
    //        animator.SetBool("DoorOpened", true);
    //        return true;
    //    }
    //    return false;
    //}

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
