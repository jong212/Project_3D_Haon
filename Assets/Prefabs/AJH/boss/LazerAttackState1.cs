using System.Collections;
using System.Collections.Generic;
using Unity.Services.Matchmaker.Models;
using Unity.VisualScripting;
using UnityEngine;

public class LazerAttackState1 : StateMachineBehaviour
{
    Transform player;
    playerAnimator playerinfo;
    MonsterInfo monsterinfo;
    float interval ;
    float timer = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
 

        interval = stateInfo.length;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Lazer_point");

        if (playerObject != null)
        {
            player = playerObject.transform;
            //playerinfo = player.GetComponent<playerAnimator>();
            monsterinfo = animator.GetComponent<MonsterInfo>();
        }
        else
        {
            Debug.LogError("Lazer_point not found. Make sure the object with tag 'Lazer_point' is created and tagged properly.");
            // Handle the error appropriately, e.g., set default values or disable certain behaviors
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            animator.transform.LookAt(player);
            float distance = Vector3.Distance(player.position, animator.transform.position);
            if (distance > 3.5)
                animator.SetBool("isAttacking", false);

            timer += Time.deltaTime;

            // 설정된 간격마다 isAttacking을 true로 설정하고 타이머를 재설정합니다.
            if (timer >= interval)
            {

                // Debug.Log("ddd");
                timer = 0f;
                //playerinfo.TakeDamage(monsterinfo._str);

            }
        }
      
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
