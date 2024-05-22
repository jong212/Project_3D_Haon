using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PatrollState : StateMachineBehaviour
{
    float timer;
    float randomTime;

    Transform player;
    Transform WayPoint;
    float chaseRange = 8;

    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MonsterInfo info = animator.gameObject.GetComponent<MonsterInfo>();
        WayPoint = info.wayPoint;
        agent = animator.GetComponent<NavMeshAgent>();
        timer = 0;
        randomTime = Random.Range(10f, 15f);

        if (WayPoint != null)
        {
            foreach (Transform t in WayPoint)
            {
                wayPoints.Add(t);
            }
        }

        if (agent != null && wayPoints.Count > 0)
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
            }
            else
            {
                Debug.LogWarning("NavMeshAgent is not on the NavMesh.");
            }
        }
        else
        {
            Debug.LogWarning("NavMeshAgent or WayPoints not properly set.");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null && agent.isOnNavMesh)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
            }

            timer += Time.deltaTime;
            if (timer > randomTime)
            {
                animator.SetBool("isPatrolling", false);
            }

            float distance = Vector3.Distance(player.position, animator.transform.position);
            if (distance < chaseRange)
            {
                animator.SetBool("isChasing", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(agent.transform.position);
        }
    }
}
