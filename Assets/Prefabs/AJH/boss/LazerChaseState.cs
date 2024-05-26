using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LazerChaseState : StateMachineBehaviour
{ 
    NavMeshAgent agent;
    Transform player;
    GameObject RandomGameObject;
    MonsterInfo monsterinfo;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterinfo = animator.GetComponent<MonsterInfo>();
        RandomGameObject = monsterinfo.GetRandomGameObject();


        if (RandomGameObject != null)
        {
            player = RandomGameObject.transform;
            agent = animator.GetComponent<NavMeshAgent>();
            agent.speed = 3.5f;
        } else if (RandomGameObject == null) {
            RandomGameObject = monsterinfo.GetRandomGameObject();
            if(RandomGameObject != null)
            {
                player = RandomGameObject.transform;
                agent = animator.GetComponent<NavMeshAgent>();
                agent.speed = 3.5f;
            } else
            {
                    animator.Play("GameEnd");
            }
        } 
       


    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (RandomGameObject != null)
        {
            if (agent != null && agent.isOnNavMesh)
            {
                agent.SetDestination(player.position);
            }
            float distance = Vector3.Distance(player.position, animator.transform.position);
            if (distance > 10)
                animator.SetBool("isChasing", false);
            if (distance < 2.5f)
                animator.SetBool("isAttacking", true);
        }
        else if (RandomGameObject == null)
        {
            RandomGameObject = monsterinfo.GetRandomGameObject();
            if (RandomGameObject != null)
            {
                if (agent != null && agent.isOnNavMesh)
                {
                    agent.SetDestination(player.position);
                }
                float distance = Vector3.Distance(player.position, animator.transform.position);
                if (distance > 10)
                    animator.SetBool("isChasing", false);
                if (distance < 2.5f)
                    animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.Play("GameEnd");
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //agent.SetDestination(animator.transform.position);

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
