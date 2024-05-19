using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;

//GimikAgain(); 기믹 재실행
//★  Danger 기믹 : 벽 오브젝트에 Wall Layer추가해 줘야함, 보스맵에서 그냥 벽이라고 판단되는건 다 Wall 레이어 설정 해야함
// public string[] playerTags3 = { "Player2", "Player3", "Player4","Player5" }; 여기 변수에 설정 된 태그들은 플레이어에 꼭 추가 되어있어야 함
// 보스맵 진입 시   [SerializeField] public bool bosssRoomStartCheck; true로 수정하는 로직 상의 후 넣어야함

/*  인터페이스  */
public interface IBossState
{
    void Enter(Boss boss);                                          // 상태에 진입할 때 호출되는 메서드
    void Execute(Boss boss);                                        // 상태가 활성화된 동안 매 프레임 호출되는 메서드
    void Exit(Boss boss);                                           // 상태에서 나갈 때 호출되는 메서드
}
public class Boss : MonoBehaviour
{
/*  선언  */
    [SerializeField] public bool bosssRoomStartCheck;
    
    public List<GameObject> players = new List<GameObject>();
    public string[] playerTags5 = { "Player2", "Player3", "Player4","Player5" };   

    
    private IBossState currentState;                                // 현재 상태
    private IBossState previousState;                               // 이전 상태

    public List<Vector3> setDangerPosition = new List<Vector3>();   // 기믹1 공격범위 저장 후처리용
    
    public float Health { get; private set; }                       // 보스의 체력
    public float GimmickThreshold1 { get; private set; }            // 기믹 임계값 1
    public float GimmickThreshold2 { get; private set; }            // 기믹 임계값 2
    public float GimmickThreshold3 { get; private set; }            // 기믹 임계값 3
    private Animator animator;                                      // 애니메이터
    public bool IsUsingLaser { get; private set; }                  // 레이저 사용 여부

/*  초기화  */
    void Start()
    {
        bosssRoomStartCheck = false;                                // 보스가 활동을 자동으로 하게 하지 않도록 초기화 
        Health = 100f;        
        GimmickThreshold1 = 30f;                                    // 체력 임계값 설정
        GimmickThreshold2 = 50f;
        GimmickThreshold3 = 70f;
        animator = GetComponent<Animator>();
        ChangeState(new NoState());                                 // 초기 상태를 Normal 상태로 설정
    }

    void Update()
    {
        currentState?.Execute(this);                                // 매 프레임 현재 상태의 Execute 메서드 실행
    }


    public void ChangeState(IBossState newState)
    {
        previousState = currentState;                               // 이전 상태 저장
        currentState?.Exit(this);                                   // 현재 상태 종료
        currentState = newState;                                    // 새로운 상태 설정
        currentState.Enter(this);                                   // 새로운 상태 진입
    }

/*  보스 애니메이션 변경(공통)  */
    public void SetAnimation(string animationName)
    {
        animator.Play(animationName);                               // 지정된 애니메이션 재생
    }
     
/*  애니메이션 이벤트 호출 함수(공통)  */
    public void OnGimmickAnimationEvent(string eventName)
    {
        if (eventName == "Stage1_start")
        {
            if (currentState is Stage1 stage1)
            {
                // 1-1. 코루틴 실행을 위해 공통  코루틴 함수인 Start bossCoroutine으로 넘김
                StartBossCoroutine(stage1.IDangerStart(this));
            }
        }
        else if (eventName == "Stage1_end")
        {
            if (currentState is Stage1 stage1)
            {
                StartBossCoroutine(stage1.IDangerEnd(this));
            }
        }
        else if(eventName == "StartLaser")
        {
            StartLaser(GameObject.FindGameObjectWithTag("Player").transform.position); // 레이저 시작
        }
        else if (eventName == "ThrowRock")
        {
            ThrowRock(GameObject.FindGameObjectWithTag("Player").transform.position); // 바위 던지기
        }
        else if (eventName == "AnimationComplete")
        {
            SetAnimation("Idle");                                                     // 애니메이션 완료 후 Idle 애니메이션 설정
        } 
    }
/*  코루틴 시작, 종료 (공통)  */

    // 1-2 매개변수로 받은 코루틴들 실행
    public void StartBossCoroutine(IEnumerator coroutine)
    {
        Debug.Log("test2");
        StartCoroutine(coroutine);
    }

    public void StopBossCoroutine(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }

/*  보스 상태 체크 (공통)  */
    public void CheckHealthAndChangeState()
    {
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
    }
/*  기믹2 패턴  */
    #region #Stage2
    public void StartLaser(Vector3 position)
    {
        Debug.Log("Starting Laser at Position: " + position);
        IsUsingLaser = true;                                       // 레이저 사용 여부 설정
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
        Debug.Log("Firing Laser");                                  // 레이저 발사 로그
        // 레이저 발사 로직 구현
    }

    public void GimikAgain()
    {
        Debug.Log("Stopping Laser");
        IsUsingLaser = false;                                        // 레이저 사용 여부 해제
        // 레이저 중지 로직 구현

                                                                     // 현재 기믹을 다시 실행하기 위해 코루틴 시작
        StartCoroutine(WaitAndReExecuteGimmick());
    }

    private IEnumerator WaitAndReExecuteGimmick()
    {
        IBossState currentGimmickState = currentState;               // 현재 상태 저장
        float waitTime = 10f;                                        // 대기 시간 설정 (초 단위)
        Debug.Log("Waiting for " + waitTime + " seconds before re-executing the gimmick.");
        yield return new WaitForSeconds(waitTime);                   // 지정된 시간 대기

                        
        if (currentState == currentGimmickState)                     // 상태가 변경되지 않았을 경우에만 현재 기믹 상태를 다시 실행
        {
            ChangeState(currentState);                               // 현재 상태로 다시 진입
        }
    }

    public void ThrowRock(Vector3 targetPosition)
    {
        Debug.Log("Throwing Rock at Position: " + targetPosition);
        // 바위 던지기 로직 구현
    }
    #endregion

}

/*  보스 대기 상태  */
public class NoState : IBossState
{
    public void Enter(Boss boss)
    {
        boss.SetAnimation("Idle1");
    }

    public void Execute(Boss boss)
    {
        if (boss.bosssRoomStartCheck) boss.ChangeState(new Stage1());
    }

    public void Exit(Boss boss)
    {
    }
}

/*  보스 Stage1 Class  */
public class Stage1 : IBossState
{
    private GameObject activeDangerLine;


    public void Enter(Boss boss)
    {
        foreach (string tag in boss.playerTags5)
        {

            GameObject player = GameObject.FindGameObjectWithTag(tag);
            if (player != null)
            {
                boss.players.Add(player);
            }
            
        }
        boss.SetAnimation("Stage1"); // Idle 애니메이션 설정
        //boss.StartBossCoroutine(SomeCoroutine(boss)); // 코루틴 시작
    }

    public void Execute(Boss boss)
    {
        Debug.Log("test");
        //boss.CheckHealthAndChangeState(); // 체력 체크 및 상태 변경
    }

    public void Exit(Boss boss)
    {
        Debug.Log("Exiting Normal State");
    }
   
    // 1-3 스타트 코루틴에서 실행이 떨어지면 실행 됨
    public IEnumerator IDangerStart(Boss boss)
    {
        yield return null;
        boss.transform.LookAt(boss.players[Random.Range(0, boss.players.Count)].transform);
        DangerLineStart(boss); 
    } 

    // 1-4 코루틴이 돌아가면 아래 함수가 실행 됨
    void DangerLineStart(Boss boss)
    { 
        foreach (GameObject player in boss.players)
        {
            if (player != null)
            {
                GameObject activeDangerLine = PoolManager.Instance.GetPoolObject(PoolObjectType.DangerLine);

                DangerLine dangerLineComponent = activeDangerLine.GetComponent<DangerLine>();
              
                if (dangerLineComponent != null)
                {
                    Vector3 direction = (player.transform.position - boss.transform.position).normalized;
                    float extendLength = 5f;
                    Vector3 extendedEndPosition = player.transform.position + direction * extendLength;
                    boss.setDangerPosition.Add(extendedEndPosition);
                    dangerLineComponent.EndPosition = extendedEndPosition;
                    activeDangerLine.transform.position = boss.transform.position;
                    activeDangerLine.SetActive(true);
                    boss.StartBossCoroutine(ReturnDangerLineToPool(activeDangerLine, dangerLineComponent.GetComponent<TrailRenderer>().time));

                }
            }
        }
    }
    // DangerLine 풀 반환 함수 정의
    public IEnumerator ReturnDangerLineToPool(GameObject dangerLine, float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.CoolObject(dangerLine, PoolObjectType.DangerLine);
        dangerLine.SetActive(false);
        DangerLine dangerLineComponent = dangerLine.GetComponent<DangerLine>();
        if (dangerLineComponent != null)
        {
            dangerLineComponent.cleartr(); // Call the method to clear the TrailRenderer
        }
    }


    public IEnumerator IDangerEnd(Boss boss)
    {
       DangerLineEnd(boss);

        yield return null;

    }
    void DangerLineEnd(Boss boss)
    {
        foreach (Vector3 setDangerPositions in boss.setDangerPosition)
        {
            if (setDangerPositions != null)
            {
                GameObject activeDangerAttack = PoolManager.Instance.GetPoolObject(PoolObjectType.DangerAttack);
                //DangerLine dangerLineComponent = activeDangerLine.GetComponent<DangerLine>();

                //if (dangerLineComponent != null)
                //{
                activeDangerAttack.transform.position = boss.transform.position;
                activeDangerAttack.transform.LookAt(setDangerPositions);
                Vector3 eulerAngles = activeDangerAttack.transform.rotation.eulerAngles;
                eulerAngles.x = 0;

                // Apply the modified rotation
                activeDangerAttack.transform.rotation = Quaternion.Euler(eulerAngles);
                activeDangerAttack.SetActive(true);
                    //boss.StartBossCoroutine(ReturnDangerLineToPool(activeDangerLine, dangerLineComponent.GetComponent<TrailRenderer>().time));

                //}
            }
        }
    }
}

/*  보스 실행On Stage2  */
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
        boss.GimikAgain(); // 상태 종료 시 레이저 중지
    }
}

/*  보스 실행On Stage3  */
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
        boss.GimikAgain(); // 상태 종료 시 레이저 중지
    }
}

/*  보스 실행On Stage4  */
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
        boss.GimikAgain(); // 상태 종료 시 레이저 중지
    }
}