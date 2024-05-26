using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerAttackState : StateMachineBehaviour
{
    Transform player;
    playerAnimator playerinfo;
    MonsterInfo monsterinfo;
    GameObject RandomGameObject;
    float interval = 3;
    float timer = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterinfo = animator.GetComponent<MonsterInfo>();
        SetRandomPlayer(animator, stateInfo);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (RandomGameObject != null && RandomGameObject.activeSelf)
        {
            AttackPlayer(animator, stateInfo);
        }
        else
        {
            SetRandomPlayer(animator, stateInfo);
            if (RandomGameObject == null)
            {
                animator.Play("GameEnd");
            }
        }
    }

    private void SetRandomPlayer(Animator animator, AnimatorStateInfo stateInfo)
    {
        RandomGameObject = monsterinfo.GetRandomGameObject();

        if (RandomGameObject != null)
        {
            player = RandomGameObject.transform;
            playerinfo = player.GetComponent<playerAnimator>();
            interval = stateInfo.length;
        }
    }

    private void AttackPlayer(Animator animator, AnimatorStateInfo stateInfo)
    {
        animator.transform.LookAt(player);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance > 3.5f)
        {
            animator.SetBool("isAttacking", false);
        }

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            playerinfo.TakeDamage(monsterinfo._str, "Noattack");
        }
    }
}
