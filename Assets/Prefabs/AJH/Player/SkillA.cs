using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ų �ߵ� �� ���� ���� ����Ǵ� ��ũ��Ʈ (������ �ִϸ��̼� ��ǿ� ���� �� ��ũ��Ʈ��)
public class SkillA : StateMachineBehaviour
{
    ColliderScript weaponColliderScript;
    playerAnimator playerAnimator;
    bool isSkillAttack = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
//Debug.Log("��ų �ߵ� �� ù ��°�� �����");

        playerAnimator = animator.GetComponent<playerAnimator>();
        // ������ �ݶ��̴� ��ũ��Ʈ�� ã���ϴ�.
        weaponColliderScript = animator.GetComponentInChildren<ColliderScript>();

        // ��ų ���� ���¿� ���������Ƿ� isSkillAttack ������ true�� �����մϴ�.
        isSkillAttack = true;

        // �浹 �̺�Ʈ�� ó���ϴ� �޼��带 �����մϴ�.
        weaponColliderScript.SkillTriggerA += OnTriggerEnterEventHandler;
    }

    // ��ų ������ ������ isSkillAttack ������ false�� �����մϴ�.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isSkillAttack = false;
        weaponColliderScript.SkillTriggerA -= OnTriggerEnterEventHandler;
    }

    private void OnTriggerEnterEventHandler(Collider otherCollider)
    {
        Debug.Log("��ų �ߵ� �� �� ��°�� �����");

        // ��ų ������ ���
        if (isSkillAttack)
        {
            if(otherCollider.tag == "Monster")
            {
                otherCollider.GetComponent<MonsterInfo>().TakeDamage(playerAnimator.getstr);
            }
            if (otherCollider.tag == "Boss")
            {
                
                otherCollider.GetComponent<Boss>().TakeDamage(playerAnimator.getstr);
            }
            // ��ų ���� ó�� �ڵ� �߰�
        }
        else
        {
            // �Ϲ� ������ ���
            Debug.Log("Normal Attack detected!");
            // �Ϲ� ���� ó�� �ڵ� �߰�
        }
    }
}
