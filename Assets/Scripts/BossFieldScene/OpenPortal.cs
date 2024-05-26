using System.Collections;
using UnityEngine;

public class OpenPortal : MonoBehaviour
{
    [SerializeField] private Transform pointA; // 시작 지점
    [SerializeField] private Transform pointB; // 도착 지점
    [SerializeField] private float duration = 8.0f; // 이동에 걸리는 시간

    [SerializeField]
    private GameObject boss;

    void Start()
    {
        if (boss.GetComponent<Boss>().currentHealth <= 0)
        {
            StartCoroutine(MoveFromAToB(pointA.position, pointB.position, duration));

        }
        // 오브젝트를 이동시키는 코루틴을 시작합니다.
    }

    private IEnumerator MoveFromAToB(Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 경과 시간을 증가시킵니다.
            elapsed += Time.deltaTime;

            // 오브젝트를 A에서 B로 보간합니다.
            transform.position = Vector3.Lerp(start, end, elapsed / duration);

            // 다음 프레임까지 기다립니다.
            yield return null;
        }

        // 이동이 끝났을 때 오브젝트를 정확히 도착 지점에 맞춥니다.
        transform.position = end;
    }
}
