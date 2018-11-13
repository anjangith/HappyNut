using Pawns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is any pawn the player can interact with.
/// </summary>
public class NonPlayerCharacter : Movable {

	// Use this for initialization
	new void Start () {
        base.Start();
        animator.SetBool("isWalking", false);
        animator.SetBool("isClimbing", false);
    }
	
	// Update is called once per frame
	new void Update ()
    {
        //base.Update();
    }
}
