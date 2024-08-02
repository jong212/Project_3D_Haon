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
        // ������ �ݶ��̴� ��ũ��Ʈ�� ã���ϴ�.
        weaponColliderScript = animator.GetComponentInChildren<ColliderScript>();

        // �浹 �̺�Ʈ�� ó���ϴ� �޼��带 �����մϴ�.
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
        // �浹 �̺�Ʈ�� ó���ϴ� �޼��带 �����մϴ�.
        weaponColliderScript.OnTriggerEnterEvent -= OnTriggerEnterEventHandler;

        // �浹 �÷��׸� �ʱ�ȭ�մϴ�.
        hasCollided = false;

    }

    private void OnTriggerEnterEventHandler(Collider otherCollider)
    {

        // �浹�� ó�� ������ ���� ó���մϴ�.
        if (!hasCollided)
        {
            Debug.Log(otherCollider.gameObject.name);
            //Debug.Log("�޺� ������.. " + otherCollider.gameObject.name);
            if (otherCollider.gameObject.tag == "Monster")
            {

                otherCollider.GetComponent<MonsterInfo>().TakeDamage(200);

            }
            //�ı� �Ǵ� ������Ʈ ���� �߰�(����)
            else if (otherCollider.gameObject.name == "DestroyBox")
            {
                otherCollider.GetComponent<BoxInfo>().BoxDamaged(1);
            }
            //=======================================================================(�����۾� ���� ��)
            hasCollided = true;

            // ���⿡�� �浹�� ó���ϴ� �ڵ带 �߰��ϼ���.
        }
    }
}
