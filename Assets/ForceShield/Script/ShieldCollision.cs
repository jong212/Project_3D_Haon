using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
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
        // Ensure the renderer and material are assigned before accessing them
        if (GetComponent<Renderer>())
        {
            mat = GetComponent<Renderer>().sharedMaterial;
            if (mat != null)
            {
                mat.SetFloat("_Opacity", 0.600f); // Initialize the opacity to 0.600f
                opacity = mat.GetFloat("_Opacity"); // Then get the initial opacity value
            }
        }

        // Ensure the particles are correctly assigned
        if (particle != null)
        {
            particle = particle.GetComponent<ParticleSystem>();
        }

        if (particle2 != null)
        {
            particle2 = particle2.GetComponent<ParticleSystem>();
        }

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
            mat?.SetFloat("_HitTime", hitTime); // Ensure mat is not null
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
                    //Debug.Log("hit");
                    mat.SetVector("_HitPosition", transform.InverseTransformPoint(other.transform.position));
                    hitTime = 0.5f; // Set to a smaller value for faster reset

                    opacity -= 0.050f; // Subtract 0.050 from the opacity
                    if (opacity < 0 &&!EnterCheck)
                    {
                        EnterCheck = true;
                        opacity = 0; // Ensure opacity does not go below 0
                        particle.Play();
                        StartCoroutine(objectOff());
                    }
                    else
                    {
                        particle2.Play();
                    }
                    mat.SetFloat("_Opacity", opacity); // Assign the updated opacity back to the material

                    mat.SetFloat("_HitTime", hitTime);
                    break; // Exit the loop after handling the hit
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
