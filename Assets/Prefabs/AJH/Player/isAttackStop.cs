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
        /*AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            string clipName = clipInfo[0].clip.name;
            Debug.Log("Currently playing animation clip: " + clipName);
        }
        Debug.Log(stateInfo.IsName("attackA5"));*/
        playerAnimator = animator.GetComponent<playerAnimator>();
        playerAnimator.isAction = true;

        // ������ �ݶ��̴� ��ũ��Ʈ�� ã���ϴ�.
        weaponColliderScript = animator.GetComponentInChildren<ColliderScript>();

        // �浹 �̺�Ʈ�� ó���ϴ� �޼��带 �����մϴ�.
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

        // �浹 �̺�Ʈ�� ó���ϴ� �޼��带 �����մϴ�.
        weaponColliderScript.OnTriggerEnterEvent -= OnTriggerEnterEventHandler;

        // �浹 �÷��׸� �ʱ�ȭ�մϴ�.
        hasCollided = false;
        Debug.Log("test1");
    }

    private void OnTriggerEnterEventHandler(Collider otherCollider)
    {
        // �浹�� ó�� ������ ���� ó���մϴ�.
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
                Debug.Log("��ž" + otherCollider.transform.position.x);
                Debug.Log("�÷��̾�" + playerAnimator.transform.position.x);

                // ��ž�� �÷��̾� ������ ȸ����ŵ�ϴ�.
                RotateTurretTowardsPlayer(otherCollider.transform);
            }

            if (otherCollider.gameObject.tag == "Monster")
            {
                otherCollider.GetComponent<MonsterInfo>().TakeDamage(playerAnimator.getstr);
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
            // �÷��̾ ���ʿ� �ִ� ��� �ݽð� �������� ȸ��
            targetYRotation = currentYRotation - rotationStep;
        }
        else
        {
            // �÷��̾ �����ʿ� �ִ� ��� �ð� �������� ȸ��
            targetYRotation = currentYRotation + rotationStep;
        }

        // 0-360�� ���̷� ȸ���� �����մϴ�.
        if (targetYRotation < 0)
            targetYRotation += 360;
        else if (targetYRotation > 360)
            targetYRotation -= 360;

        // ���ο� ȸ���� �����մϴ�.
        turretTransform.rotation = Quaternion.Euler(0, targetYRotation, 0);
    }
}
