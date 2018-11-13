using Pawns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : StateMachineBehaviour {

    private float lastAttackTime = -1;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(lastAttackTime);
        //Already attack 2 seconds ago.
        if(lastAttackTime != -1 && Time.time - lastAttackTime < 2 )
        {
            return;
        }
        var enemy = animator.gameObject.GetComponent<Enemy>();
        if(!enemy)
        {
            return;
        }
        var raycastDirection = (enemy.GetMoveDirection().x > 0.01f) ? Vector2.right : Vector2.left;

        var result = Physics2D.RaycastAll(new Vector2(animator.gameObject.transform.position.x, animator.gameObject.transform.position.y + 1), raycastDirection);
        for(var i = 0; i < result.Length; i++)
        {
            if(result[i].transform.CompareTag("Player"))
            {
                enemy.AwareOfPlayer = true;
                animator.SetBool("isAttacking", true);
                lastAttackTime = Time.time;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
