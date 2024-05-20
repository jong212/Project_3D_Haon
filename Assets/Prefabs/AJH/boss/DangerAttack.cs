using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerAttack : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private bool isReturning = false;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particleSystem.isStopped && !isReturning)
        {
            StartCoroutine(ReturnToPoolWithDelay());
        }
    }

    private IEnumerator ReturnToPoolWithDelay()
    {
        isReturning = true; // 코루틴이 시작됨을 표시
        yield return new WaitForSeconds(3f); // 0.5초 대기

        if (transform.parent != null)
        {
            GameObject parentObject = transform.parent.gameObject;
            Debug.Log("Particle ends and return to pool after 0.5 seconds");
            PoolManager.Instance.CoolObject(parentObject, PoolObjectType.DangerAttack);
        }

        isReturning = false; // 코루틴이 종료됨을 표시
    }
    public void OnParticleCollision(GameObject other)
    {

    }
}
