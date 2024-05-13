using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isAttackStop : StateMachineBehaviour
{
    ColliderScript weaponColliderScript;
    playerAnimator playerAnimator;
    bool hasCollided = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        playerAnimator = animator.GetComponent<playerAnimator>();
        playerAnimator.isAction = true;
        // 무기의 콜라이더 스크립트를 찾습니다.
        weaponColliderScript = animator.GetComponentInChildren<ColliderScript>();

        // 충돌 이벤트를 처리하는 메서드를 설정합니다.
        weaponColliderScript.OnTriggerEnterEvent += OnTriggerEnterEventHandler;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerAnimator = animator.GetComponent<playerAnimator>();
        playerAnimator.isAction = false;
        // 충돌 이벤트를 처리하는 메서드를 제거합니다.
        weaponColliderScript.OnTriggerEnterEvent -= OnTriggerEnterEventHandler;

        // 충돌 플래그를 초기화합니다.
        hasCollided = false;

    }

    private void OnTriggerEnterEventHandler(Collider otherCollider)
    {
        
        // 충돌이 처음 감지될 때만 처리합니다.
        if (!hasCollided)
        {
            Debug.Log(otherCollider.gameObject.name);
            //Debug.Log("콤보 공격중.. " + otherCollider.gameObject.name);
            if(otherCollider.gameObject.tag == "Monster")
            {
                
                otherCollider.GetComponent<MonsterInfo>().TakeDamage(10);

            }
            hasCollided = true;

            // 여기에서 충돌을 처리하는 코드를 추가하세요.
        }
    }
}
/*
public class isAttackStop : StateMachineBehaviour
{
    playerAnimator playerAnimator;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("hi");
        playerAnimator = animator.GetComponent<playerAnimator>();
        playerAnimator.isAction = true;
        playerAnimator.EnableWeapon();

    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerAnimator = animator.GetComponent<playerAnimator>();
        playerAnimator.isAction = false;
        playerAnimator.DisableWeapon();

    }
}
*/