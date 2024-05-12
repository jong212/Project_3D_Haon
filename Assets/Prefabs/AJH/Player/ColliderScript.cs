using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ColliderScript : MonoBehaviour
{
/* 1. 이벤트 정의 */
    // 마우스 클릭 시 콤보 공격 이벤트 정의
    public event Action<Collider> OnTriggerEnterEvent;

    // 1번 스킬 이벤트 정의
    public event Action<Collider> SkillTriggerA;

/* 2. 스킬,콤보 공격 하다 몬스터와 충돌나면 이벤트 발생*/
    private void OnTriggerEnter(Collider other)
    {
        if(other != null && other.name != "player")
        {
            OnTriggerEnterEvent?.Invoke(other);
            SkillTriggerA?.Invoke(other);
        }

    }
}