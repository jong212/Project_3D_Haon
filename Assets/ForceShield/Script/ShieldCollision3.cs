using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision3 : MonoBehaviour
{
    public isAttackStop isAttackstop;

    [SerializeField] private GameObject player; // 플레이어 오브젝트를 인스펙터에서 참조합니다.
    [SerializeField] string[] _collisionTag;
    public ParticleSystem particle;
    public ParticleSystem particle2;
    Material mat;
    private bool EnterCheck;
    private float opacity;
    float hitTime;
    private Animator playerAnimator;
    public float count = 0;
    public float count_temp = 0;

    void Start()
    {
        if (GetComponent<Renderer>())
        {
            mat = GetComponent<Renderer>().sharedMaterial;
            if (mat != null)
            {
                mat.SetFloat("_Opacity", 0.600f);
                opacity = mat.GetFloat("_Opacity");
            }
        }

        particle = particle?.GetComponent<ParticleSystem>();
        particle2 = particle2?.GetComponent<ParticleSystem>();

        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                isAttackstop = playerAnimator.GetBehaviour<isAttackStop>();
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
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
            mat?.SetFloat("_HitTime", hitTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (count != count_temp)
        {
            count_temp++;
            for (int i = 0; i < _collisionTag.Length; i++)
            {
                if (_collisionTag.Length > 0 && other.CompareTag(_collisionTag[i]))
                {
                    mat.SetVector("_HitPosition", transform.InverseTransformPoint(other.transform.position));
                    hitTime = 500;

                    opacity -= 0.050f;
                    if (opacity < 0 && !EnterCheck)
                    {
                        EnterCheck = true;
                        opacity = 0;
                        particle?.Play();
                        StartCoroutine(objectOff());
                    }
                    else
                    {
                        particle2?.Play();
                    }
                    mat?.SetFloat("_Opacity", opacity);
                    mat?.SetFloat("_HitTime", hitTime);
                    break;
                }
            }
        }
    }

    IEnumerator objectOff()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
