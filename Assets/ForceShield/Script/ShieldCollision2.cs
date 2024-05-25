using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision2 : MonoBehaviour
{
    public isAttackStop isAttackstop;

    [SerializeField] private GameObject player; // 플레이어 오브젝트를 인스펙터에서 참조합니다.
    [SerializeField] string[] _collisionTag;
    float hitTime;
    Material mat;
    private Animator playerAnimator;
    public float count = 0;
    public float count_temp = 0;
    void Start()
    {
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                isAttackstop = playerAnimator.GetBehaviour<isAttackStop>();
            }
        } else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (GetComponent<Renderer>())
        {
            mat = GetComponent<Renderer>().sharedMaterial;
        }
    }

    void Update()
    {
        
        

        if (hitTime > 0)
        {
            float myTime = Time.fixedDeltaTime * 1000;
            hitTime -= myTime;
            if (hitTime < 0)
            {
                hitTime = 0;
            }
            mat.SetFloat("_HitTime", hitTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (count != count_temp)
        {
            count_temp++;
            for (int i = 0; i < _collisionTag.Length; i++)
            {
                if (_collisionTag.Length > 0 || other.CompareTag(_collisionTag[i]))
                {
                    //Debug.Log("hit");
                    mat.SetVector("_HitPosition", transform.InverseTransformPoint(other.transform.position));
                    hitTime = 500;
                    mat.SetFloat("_HitTime", hitTime);
                }
            }
        }
    }
}


 