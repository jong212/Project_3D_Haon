using System.Threading;
using UnityEngine;

public class isAttackStop : StateMachineBehaviour
{
    ColliderScript weaponColliderScript;
    public playerAnimator playerAnimator;
    public float rotationStep = 2f;
    public bool hasCollided = false;
    public float AttackCheck = 0;
    public ShieldCollision shieldCollision;
    public ShieldCollision1 shieldCollision1;
    public ShieldCollision2 shieldCollision2;
    public ShieldCollision3 shieldCollision3;

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
        Debug.Log("test5");
        playerAnimator = animator.GetComponent<playerAnimator>();
        if (stateInfo.IsTag("a"))
        {
            Debug.Log("test4");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("p_Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("p_run") || animator.GetCurrentAnimatorStateInfo(0).IsName("ChargeSkillA_Skill"))
            {
                Debug.Log("test3");
                playerAnimator.isAction = false;
            }
            else
            {
                Debug.Log("test2");
            }
        }

        // 충돌 이벤트를 처리하는 메서드를 제거합니다.
        weaponColliderScript.OnTriggerEnterEvent -= OnTriggerEnterEventHandler;

        // 충돌 플래그를 초기화합니다.
        hasCollided = false;
        Debug.Log("test1");
    }

    private void OnTriggerEnterEventHandler(Collider otherCollider)
    {
        // 충돌이 처음 감지될 때만 처리합니다.
        if (!hasCollided)
        {
            if (otherCollider.gameObject.name == "Shield")
            {
                shieldCollision = otherCollider.gameObject.GetComponent<ShieldCollision>();
                if (shieldCollision != null)
                {
                    shieldCollision.count += 1;
                }
            }
            if (otherCollider.gameObject.name == "Shield1")
            {
                shieldCollision1 = otherCollider.gameObject.GetComponent<ShieldCollision1>();
                if (shieldCollision1 != null)
                {
                    shieldCollision1.count += 1;
                }
            }
            if (otherCollider.gameObject.name == "Shield2")
            {
                shieldCollision2 = otherCollider.gameObject.GetComponent<ShieldCollision2>();
                if (shieldCollision2 != null)
                {
                    shieldCollision2.count += 1;
                }
            }
            if (otherCollider.gameObject.name == "Shield3")
            {
                shieldCollision3 = otherCollider.gameObject.GetComponent<ShieldCollision3>();
                if (shieldCollision3 != null)
                {
                    shieldCollision3.count += 1;
                }
            }
            Debug.Log(otherCollider.gameObject.name);

            if (otherCollider.CompareTag("Lazer_point"))
            {
                Debug.Log("포탑" + otherCollider.transform.position.x);
                Debug.Log("플레이어" + playerAnimator.transform.position.x);

                // 포탑을 플레이어 쪽으로 회전시킵니다.
                RotateTurretTowardsPlayer(otherCollider.transform);
            }

            if (otherCollider.gameObject.tag == "Monster")
            {
                otherCollider.GetComponent<MonsterInfo>().TakeDamage(10);
            }
            else if (otherCollider.gameObject.name == "DestroyBox")
            {
                otherCollider.GetComponent<BoxInfo>().BoxDamaged(1);
            }
            else if (otherCollider.gameObject.tag == "Boss")
            {
                otherCollider.GetComponent<Boss>().TakeDamage(playerAnimator.getstr);
            }

            hasCollided = true;
        }
    }

    private void RotateTurretTowardsPlayer(Transform turretTransform)
    {
        float currentYRotation = turretTransform.eulerAngles.y;
        float targetYRotation;

        if (playerAnimator.transform.position.x < turretTransform.position.x)
        {
            // 플레이어가 왼쪽에 있는 경우 반시계 방향으로 회전
            targetYRotation = currentYRotation - rotationStep;
        }
        else
        {
            // 플레이어가 오른쪽에 있는 경우 시계 방향으로 회전
            targetYRotation = currentYRotation + rotationStep;
        }

        // 0-360도 사이로 회전을 보장합니다.
        if (targetYRotation < 0)
            targetYRotation += 360;
        else if (targetYRotation > 360)
            targetYRotation -= 360;

        // 새로운 회전을 적용합니다.
        turretTransform.rotation = Quaternion.Euler(0, targetYRotation, 0);
    }
}
