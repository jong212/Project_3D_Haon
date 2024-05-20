using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 발동 시 제일 먼저 실행되는 스크립트 (이유는 애니메이션 모션에 부착 된 스크립트라서)
public class SkillA : StateMachineBehaviour
{
    ColliderScript weaponColliderScript;
    playerAnimator playerAnimator;
    bool isSkillAttack = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
//Debug.Log("스킬 발동 시 첫 번째로 실행됨");

        playerAnimator = animator.GetComponent<playerAnimator>();
        // 무기의 콜라이더 스크립트를 찾습니다.
        weaponColliderScript = animator.GetComponentInChildren<ColliderScript>();

        // 스킬 공격 상태에 진입했으므로 isSkillAttack 변수를 true로 설정합니다.
        isSkillAttack = true;

        // 충돌 이벤트를 처리하는 메서드를 설정합니다.
        weaponColliderScript.SkillTriggerA += OnTriggerEnterEventHandler;
    }

    // 스킬 공격이 끝나면 isSkillAttack 변수를 false로 설정합니다.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerAnimator = animator.GetComponent<playerAnimator>();
        isSkillAttack = false;
        weaponColliderScript.SkillTriggerA -= OnTriggerEnterEventHandler;
    }

    private void OnTriggerEnterEventHandler(Collider otherCollider)
    {
//Debug.Log("스킬 발동 시 세 번째로 실행됨");

        // 스킬 공격인 경우
        if (isSkillAttack)
        {
            if(otherCollider.tag == "Monster")
            {
                otherCollider.GetComponent<MonsterInfo>().TakeDamage(10);
            }
            if (otherCollider.tag == "Boss")
            {
                otherCollider.GetComponent<Boss>().TakeDamage(10);
            }
            // 스킬 공격 처리 코드 추가
        }
        else
        {
            // 일반 공격인 경우
            Debug.Log("Normal Attack detected!");
            // 일반 공격 처리 코드 추가
        }
    }
}
