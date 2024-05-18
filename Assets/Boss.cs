using UnityEngine;
using System.Collections;
public interface IBossState
{
    void Enter(Boss boss);   // 상태에 진입할 때 호출되는 메서드
    void Execute(Boss boss); // 상태가 활성화된 동안 매 프레임 호출되는 메서드
    void Exit(Boss boss);    // 상태에서 나갈 때 호출되는 메서드
}
public class Boss : MonoBehaviour
{
    private IBossState currentState; // 현재 상태
    private IBossState previousState; // 이전 상태
    public float Health { get; private set; } // 보스의 체력
    public float GimmickThreshold1 { get; private set; } // 기믹 임계값 1
    public float GimmickThreshold2 { get; private set; } // 기믹 임계값 2
    public float GimmickThreshold3 { get; private set; } // 기믹 임계값 3
    private Animator animator; // 애니메이터
    public bool IsUsingLaser { get; private set; } // 레이저 사용 여부

    /*기믹1 start*/
    public GameObject laserEffectPrefab; // 레이저 이펙트 프리팹
    public float laserDistance = 10f; // 레이저 거리
    public LayerMask floorLayerMask; // 바닥 레이어 마스크
    /*기믹1 end*/
    void Start()
    {
        GimmickThreshold1 = 30f; // 체력 임계값 설정
        GimmickThreshold2 = 50f;
        GimmickThreshold3 = 70f;
        animator = GetComponent<Animator>();
        ChangeState(new NormalState()); // 초기 상태를 Normal 상태로 설정
    }

    void Update()
    {
        currentState?.Execute(this); // 매 프레임 현재 상태의 Execute 메서드 실행
    }


    public void ChangeState(IBossState newState)
    {
        previousState = currentState; // 이전 상태 저장
        currentState?.Exit(this); // 현재 상태 종료
        currentState = newState; // 새로운 상태 설정
        currentState.Enter(this); // 새로운 상태 진입
    }

    public void SetAnimation(string animationName)
    {
        Debug.Log("Setting animation to: " + animationName);
        animator.Play(animationName); // 지정된 애니메이션 재생
    }

    public void OnGimmickAnimationEvent(string eventName)
    {
        Debug.Log("Animation Event Triggered: " + eventName);
        if (eventName == "StartLaser")
        {
            StartLaser(GameObject.FindGameObjectWithTag("Player").transform.position); // 레이저 시작
        }
        else if (eventName == "ThrowRock")
        {
            ThrowRock(GameObject.FindGameObjectWithTag("Player").transform.position); // 바위 던지기
        }
        else if (eventName == "AnimationComplete")
        {
            SetAnimation("Idle"); // 애니메이션 완료 후 Idle 애니메이션 설정
        }
    }

    public void StartLaser(Vector3 position)
    {
        Debug.Log("Starting Laser at Position: " + position);
        IsUsingLaser = true; // 레이저 사용 여부 설정
        // 레이저 시작 로직 구현
    }

    public void UpdateLaserPosition(Vector3 position)
    {
        if (IsUsingLaser)
        {
            Debug.Log("Updating Laser Position to: " + position);
            // 레이저 위치 업데이트 로직 구현
        }
    }

    public void FireLaser()
    {
        Debug.Log("Firing Laser"); // 레이저 발사 로그
        // 레이저 발사 로직 구현
    }

    public void StopLaser()
    {
        Debug.Log("Stopping Laser");
        IsUsingLaser = false; // 레이저 사용 여부 해제
        // 레이저 중지 로직 구현

        // 현재 기믹을 다시 실행하기 위해 코루틴 시작
        StartCoroutine(WaitAndReExecuteGimmick());
    }

    private IEnumerator WaitAndReExecuteGimmick()
    {
        IBossState currentGimmickState = currentState; // 현재 상태 저장
        float waitTime = 3f; // 대기 시간 설정 (초 단위)
        Debug.Log("Waiting for " + waitTime + " seconds before re-executing the gimmick.");
        yield return new WaitForSeconds(waitTime); // 지정된 시간 대기

        // 상태가 변경되지 않았을 경우에만 현재 기믹 상태를 다시 실행
        if (currentState == currentGimmickState)
        {
            ChangeState(currentState); // 현재 상태로 다시 진입
        }
    }

    public void ThrowRock(Vector3 targetPosition)
    {
        Debug.Log("Throwing Rock at Position: " + targetPosition);
        // 바위 던지기 로직 구현
    }

    public void CheckHealthAndChangeState()
    {
        // 체력 체크 및 상태 전환 로직
        if (Health < GimmickThreshold1)
        {
            ChangeState(new GimmickState3()); // 가장 높은 임계값부터 체크
        }
        else if (Health < GimmickThreshold2)
        {
            ChangeState(new GimmickState2());
        }
        else if (Health < GimmickThreshold3)
        {
            ChangeState(new GimmickState1());
        }
        else
        {
            ChangeState(new NormalState());
        }
    }
}
public class NormalState : IBossState
{
    public void Enter(Boss boss)
    {
        Debug.Log("Entering Normal State");
        boss.SetAnimation("Idle"); // Idle 애니메이션 설정
    }

    public void Execute(Boss boss)
    {
        Debug.Log("test");
        FireLasersInEightDirections(boss);
        //        boss.CheckHealthAndChangeState(); // 체력 체크 및 상태 변경
    }

    public void Exit(Boss boss)
    {
        Debug.Log("Exiting Normal State");
    }
    void FireLasersInEightDirections(Boss boss)
    {
        // 8방향 벡터 설정
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right,
            (Vector3.forward + Vector3.left).normalized,
            (Vector3.forward + Vector3.right).normalized,
            (Vector3.back + Vector3.left).normalized,
            (Vector3.back + Vector3.right).normalized
        };

        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;
            // Raycast를 사용하여 바닥에 닿는 지점 찾기
            if (Physics.Raycast(boss.transform.position, direction, out hit, boss.laserDistance, boss.floorLayerMask))
            {
                Debug.Log("Laser hit at: " + hit.point);

                // 레이저 이펙트 생성
                Object.Instantiate(boss.laserEffectPrefab, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("Laser did not hit anything in direction: " + direction);
            }
        }
    }

}
public class GimmickState1 : IBossState
{
    private Transform playerTransform; // 플레이어의 Transform

    public void Enter(Boss boss)
    {
        Debug.Log("Entering Gimmick State 1");
        boss.SetAnimation("Gimmick1"); // Gimmick1 애니메이션 설정
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform 가져오기
    }

    public void Execute(Boss boss)
    {
        // 실시간으로 플레이어의 위치를 추적하고 필요 시 레이저 발사 계속
        if (boss.IsUsingLaser)
        {
            boss.UpdateLaserPosition(playerTransform.position); // 레이저 위치 업데이트
            boss.FireLaser(); // 레이저 발사
        }
    }

    public void Exit(Boss boss)
    {
        Debug.Log("Exiting Gimmick State 1");
        boss.StopLaser(); // 상태 종료 시 레이저 중지
    }
}

public class GimmickState2 : IBossState
{
    private Transform playerTransform; // 플레이어의 Transform

    public void Enter(Boss boss)
    {
        Debug.Log("Entering Gimmick State 2");
        boss.SetAnimation("Gimmick2"); // Gimmick2 애니메이션 설정
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform 가져오기
    }

    public void Execute(Boss boss)
    {
        // 필요 시 추가 로직 추가
    }

    public void Exit(Boss boss)
    {
        Debug.Log("Exiting Gimmick State 2");
        boss.StopLaser(); // 상태 종료 시 레이저 중지
    }
}
public class GimmickState3 : IBossState
{
    private Transform playerTransform; // 플레이어의 Transform

    public void Enter(Boss boss)
    {
        Debug.Log("Entering Gimmick State 3");
        boss.SetAnimation("Gimmick3"); // Gimmick3 애니메이션 설정
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform 가져오기
    }

    public void Execute(Boss boss)
    {
        // 필요 시 추가 로직 추가
    }

    public void Exit(Boss boss)
    {
        Debug.Log("Exiting Gimmick State 3");
        boss.StopLaser(); // 상태 종료 시 레이저 중지
    }
}