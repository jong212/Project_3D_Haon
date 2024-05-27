using UnityEngine;

public class NetworkPlayerStateChange : StateMachineBehaviour
{
    ColliderScript weaponColliderScript;
    NetworkPlayerController playerAnimator;
    bool hasCollided = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        playerAnimator = animator.GetComponent<NetworkPlayerController>();
        playerAnimator.isAction = true;
        // 무기의 콜라이더 스크립트를 찾습니다.
        weaponColliderScript = animator.GetComponentInChildren<ColliderScript>();

        // 충돌 이벤트를 처리하는 메서드를 설정합니다.
        weaponColliderScript.OnTriggerEnterEvent += OnTriggerEnterEventHandler;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerAnimator = animator.GetComponent<NetworkPlayerController>();
        if (stateInfo.IsTag("a"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("p_Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("p_run") || animator.GetCurrentAnimatorStateInfo(0).IsName("ChargeSkillA_Skill"))
            {
                playerAnimator.isAction = false;

            }
        }
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
            if (otherCollider.gameObject.tag == "Monster")
            {

                otherCollider.GetComponent<MonsterInfo>().TakeDamage(200);

            }
            //파괴 되는 오브젝트 조건 추가(준후)
            else if (otherCollider.gameObject.name == "DestroyBox")
            {
                otherCollider.GetComponent<BoxInfo>().BoxDamaged(1);
            }
            //=======================================================================(준후작업 라인 끝)
            hasCollided = true;

            // 여기에서 충돌을 처리하는 코드를 추가하세요.
        }
    }
}
