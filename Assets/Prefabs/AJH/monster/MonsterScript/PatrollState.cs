using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PatrollState : StateMachineBehaviour
{
    float timer;
    float randomTime;

    //[추격] - 추격 상태로 전환될 때 플레이어의 위치가 필요해서 플레이어의 Transform을 받을 변수 선언
    //근데 나중에 서버 붙이고 하면 플레이어 복제하면 태그 안겹치게 해야 할수도 있음.
    Transform player;
    Transform WayPoint;
    float chaseRange = 8;

    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //[추격] - 플레이어 transform 값 받아옴
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MonsterInfo info = animator.gameObject.GetComponent<MonsterInfo>();
        WayPoint = info.wayPoint;
        agent = animator.GetComponent<NavMeshAgent>();
        timer = 0;
        randomTime = Random.Range(10f, 15f);

        foreach (Transform t in WayPoint)
            wayPoints.Add(t);

        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //목표 지점
        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);

        timer += Time.deltaTime;
        if (timer > randomTime)
            animator.SetBool("isPatrolling", false);

        //[추격] -  몬스터와 플레이어의 거리를 vector3.Distance로 계산하고 chaseRange미만이면 추격상태로 변경
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
            animator.SetBool("isChasing", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

}
