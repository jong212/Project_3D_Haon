using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
/****************************************************************************** 
 [ 코드 동작 방식 정리 ]
 - 스킬 및 이펙트는 StartBossCoroutine(stage1.IDangerStart(this), 10f) 이런식으로 재생할 코루틴과 종료시간을 같이 넘겨서 코루틴이 종료될 수 있게 처리
 - GimikAgain(); 기믹을 재실행 하는 함수
 - Danger 범위 지속시간은 DangerLine 프리팹 인스펙터에서 Time 값으로 설정

 [ ★ 씬 합치고 세팅해야 할 항목들 정리 ★ ]
 - 보스맵에서 벽으로 판탄되는 오브젝트에 "Wall" 레이어 설정 (이유 기억이 안 남)
 - public string[] playerTags3 = { "Player2", "Player3", "Player4","Player5" }; 여기 변수에 설정 된 태그에 맞게 하이어라키 플레이어 태그를 설정해야 함  
 - 보스맵 진입 시  [SerializeField] public bool bosssRoomStartCheck; true로 수정하는 로직 상의 후 넣어야함
 - 보스 오브젝트에 Boss 태그 달기
 - Flower Dryad_temp Bossbar 에 캔버스_boss 오브젝트 하위의 text(TMP)집어넣기
 - LazerSpawner 오브젝트에 LazerSpawner태그
 - Effect1 오브젝트에 Effect1Spawner 태그
 - Effect2 오브젝트에 Effect2Spawner 태그
****************************************************************************** */

/*  인터페이스  */
public interface IBossState
{
   
    void Enter(Boss boss);                                             // 상태에 진입할 때 호출되는 메서드
    void Execute(Boss boss);                                           // 상태가 활성화된 동안 매 프레임 호출되는 메서드
    void Exit(Boss boss);                                              // 상태에서 나갈 때 호출되는 메서드
    string ToString();
}

/*  선언  */
public class Boss : MonoBehaviour
{
    [SerializeField] public bool bosssRoomStartCheck;
    [SerializeField] public float currentHealth;                       // 현재 체력 
    public BossBar bossbar;
    public float fixHealth;                       // 보스 체력 세팅
    public bool LazerGimick;
    public List<GameObject> players = new List<GameObject>();
    public string[] playerTags5 = {"Player"};// "Player2", "Player3", "Player4", "Player5" ,

    private IBossState currentState;                                   // 현재 상태
    public string previousState;
    public List<Vector3> setDangerPosition = new List<Vector3>();      // 기믹1 : 몬스터가 공격하는 장판 범위를 리스트에 넣어둠 (스킬 도 이 방향대로 나아가야 해서)
    public int Stage2Hp { get; private set; }                          // 기믹 임계값 1
    public int Stage3Hp { get; private set; }                          // 기믹 임계값 2
    public int Stage4Hp { get; private set; }                          // 기믹 임계값 3
    private Animator animator;                                         // 애니메이터
    public bool IsUsingLaser { get; private set; }                     // 레이저 사용 여부

    private List<Coroutine> runningCoroutines = new List<Coroutine>(); // Running coroutine references!!!!

    /*  초기화  */
    void Start()
    {
        bosssRoomStartCheck = false;                                   // 보스가 활동을 자동으로 하게 하지 않도록 초기화 
        fixHealth = 5000f;
        currentHealth = fixHealth;
        Stage2Hp = 4000;                                                // 체력 임계값 설정
        Stage3Hp = 3000;
        Stage4Hp = 2000;
        animator = GetComponent<Animator>();
        ChangeState(new NoState());                                    // 초기 상태를 Normal 상태로 설정
    }

    void Update()
    {
        currentState?.Execute(this);                                   // 매 프레임 현재 상태의 Execute 메서드 실행
    }

    public void ChangeState(IBossState newState)
    {
        previousState = currentState?.ToString();                      // 이전 상태를 문자열로 저장

        currentState?.Exit(this);                                      // 현재 상태 종료
        currentState = newState;                                       // 새로운 상태 설정
        currentState.Enter(this);                                      // 새로운 상태 진입
    }
    /*  보스 체력 관리 시스템  */
    public void TakeDamage(int damageAmout)
    {
        currentHealth -= damageAmout;
        bossbar.RefreshBossHp(this, currentHealth);

        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
        }
        else
        {
            Debug.Log("보스 공격 당하는중");
        }
    }
    /*  보스 애니메이션 변경(공통)  */
    public void SetAnimation(string animationName)
    {
        animator.Play(animationName);                                  // 지정된 애니메이션 재생
    }
/*  애니메이션 이벤트 호출 함수(공통)  */
    public void OnGimmickAnimationEvent(string eventName)
    {
        if (eventName == "Stage1_start")
        {
            if (currentState is Stage1 stage1)
            {   // 1-1. 코루틴 실행을 위해 공통 코루틴 함수인 StartBossCoroutine으로 넘김
                StartBossCoroutine(stage1.IDangerStart(this), 3f);     // 코루틴 시작 및 2초 후 중지
            }
        }
        else if (eventName == "Stage1_end")
        {
            if (currentState is Stage1 stage1)
            {   // 2-1. 코루틴 실행을 위해 공통 코루틴 함수인 StartBossCoroutine으로 넘김
                StartBossCoroutine(stage1.IDangerEnd(this), 5f);       // 코루틴 시작 및 2초 후 중지
            }
        }
        else if (eventName == "StartLaser")
        {
            StartLaser(GameObject.FindGameObjectWithTag("Player").transform.position); // 레이저 시작
        }
        else if (eventName == "ThrowRock")
        {
            ThrowRock(GameObject.FindGameObjectWithTag("Player").transform.position); // 바위 던지기
        }
        else if (eventName == "AnimationComplete")
        {
            SetAnimation("Idle");                                        // 애니메이션 완료 후 Idle 애니메이션 설정
        }
    }

/*  코루틴(공통)  */
    // 1-2 , 2-2
    public void StartBossCoroutine(IEnumerator coroutine, float stopAfterSeconds)
    {   // 여기서 시작시킴
        Coroutine startedCoroutine = StartCoroutine(coroutine);
        // 실행시킨 코루틴값? 배열에 추가시킴
        runningCoroutines.Add(startedCoroutine);
        // 위에서 실행시켰던 코루틴을 종료타이머 설정시킴
        StartCoroutine(StopCoroutineAfterDelay(startedCoroutine, stopAfterSeconds));
    }

    private IEnumerator StopCoroutineAfterDelay(Coroutine coroutine, float delay)
    {
        // 종료타이머 시간만큼 기다림
        yield return new WaitForSeconds(delay);
        // 코루틴 종료
        StopCoroutine(coroutine);
        runningCoroutines.Remove(coroutine);
        Debug.Log("Coroutine stopped after " + delay + " seconds");
    }

    /*  보스 상태 체크 (공통)  */
    public void CheckHealthAndChangeState(bool chk, Boss boss)
    {
                                                                 // previousState 이전 상태
        switch (previousState) 
        {
            //Jonghwa 0523 주석처리
            /* case ("NoState"):
                 {
                     if (currentHealth <= Stage2Hp)             
                     {
                         ChangeState(new Stage2());             
                     }
                     break;
                 }*/
            case ("Stage1"): 
                {

                    if (currentHealth <= Stage2Hp)
                    {
                        if (!LazerGimick) StartCoroutine(Gimick1Lazer(this));     
                            ChangeState(new Stage1());                            

                        //ChangeState(new Stage2()); // 가장 높은 임계값부터 체크
                    }
                    else
                    {
                        if (chk)  // 기믹 1.5로 넘어가기 전에는 여기 탐 
                        {
                            Debug.Log("상태를 재실행해요..");
                            ChangeState(new Stage1());

                        }
                    }
                    break;
                }
            case ("Stage2"):
                { 
                    if (currentHealth <= Stage3Hp) // 전 스테이지가 Stage2 이고 보스 체력을 90 이하로 깎았다면 Stage3로 넘어갈 수 있음
                    {
                        ChangeState(new Stage3()); // 가장 높은 임계값부터 체크
                    }
                    else
                    {

                        /*if ("타임...채워지면 Stage1으로..") {
                            Debug.Log()
                        } */
                    }
                    break;
                }
        }
     
    }


    public IEnumerator Gimick1Lazer(Boss boss)
    {
        boss.LazerGimick = true;
        GameObject getLazer = PoolManager.Instance.GetPoolObject(PoolObjectType.LazerParents);
        Transform Effect1 = FindChildWithTag(getLazer.transform, "Effect1Spawner");
        Transform Effect2 = FindChildWithTag(getLazer.transform, "Effect2Spawner");



        Vector3 startPosition = boss.transform.position;
        startPosition.y -= 5f; // Adjust this value as needed to set the starting position below the boss
        getLazer.transform.position = startPosition;
        getLazer.SetActive(true);

        float riseTime = 4f; // 레이저가 보스의 위치에 도달하는 데 걸리는 총 시간
        float elapsedTime = 0f;  // 경과 시간 (초기에는 0)
        bool test = false;
        while (elapsedTime < riseTime)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Assumes the animation is on layer 0

            if (!stateInfo.IsName("Skill"))
            {
                boss.SetAnimation("Skill");
            }
            // while 루프 내에서 다음 코드가 매 프레임 실행됩니다.
            getLazer.transform.position = Vector3.Lerp(startPosition, boss.transform.position, elapsedTime / riseTime);
            elapsedTime += Time.deltaTime;// 경과 시간을 증가시킵니다 (매 프레임마다)
            yield return null;
        }

        boss.SetAnimation("Stage1");
        // Ensure the laser reaches the exact position of the boss
        getLazer.transform.position = boss.transform.position;
         
        Transform childOneTransform = getLazer.transform.Find("1").Find("LazerSpawner");
        childOneTransform.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        float lazerMonTimer = 0f;
        float lazerMon1Timer = 0f;
        float lazerMonInterval = 10f;
        float lazerMon1Interval = 3f;
        while (true)
        {
            Debug.Log("test");
            lazerMonTimer += Time.deltaTime;
            lazerMon1Timer += Time.deltaTime;

            if (lazerMonTimer >= lazerMonInterval)
            {
                SpawnLazerMon();
                lazerMonTimer = 0f; // Reset the timer for LazerMon
            }

            if (lazerMon1Timer >= lazerMon1Interval)
            {
                SpawnLazerMon1();
                lazerMon1Timer = 0f; // Reset the timer for LazerMon1
            }

            yield return null; // Wait until the next frame
        }
        Transform FindChildWithTag(Transform parent, string tag)
        {
            foreach (Transform child in parent)
            {
                if (child.CompareTag(tag))
                {
                    return child;
                }
            }
            return null;
        }
        // Local functions for spawning
        void SpawnLazerMon()
        {
            GameObject LazerMon = PoolManager.Instance.GetPoolObject(PoolObjectType.LazerMon);
            if (LazerMon == null)
            {
                Debug.LogError("Failed to get LazerMon from the pool.");
                return;
            }

            // 밟는 지점 초기화
            Vector3 spawnPosition = Effect1.position;
            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                LazerMon.transform.position = hit.position;
            }
            else
            {
                Debug.LogError("LazerMon spawn position is not on the NavMesh.");
                return;
            }
            // 회전 초기화 
            LazerMon.transform.rotation = Quaternion.identity; // Reset rotation

            NavMeshAgent agent = LazerMon.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent component missing from LazerMon.");
                return;
            }

            // 네비메쉬 껏다 켜서 초기화 시키기
            StartCoroutine(EnableNavMeshAgent(agent, Effect1.position));

            // 몬스터 정보 초기화 
            MonsterInfo monsterInfo = LazerMon.GetComponent<MonsterInfo>();
            if (monsterInfo == null)
            {
                Debug.LogError("LazerMon does not have a MonsterInfo component.");
                return;
            }
            monsterInfo._hp = 100;

            CapsuleCollider capsuleCollider = LazerMon.GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                capsuleCollider.enabled = true;
            }

            // 초기화 작업 마무리 되면 아래에서 Active
            LazerMon.SetActive(true);

            Debug.Log("Spawned LazerMon with HP: " + monsterInfo._hp);
        }

        void SpawnLazerMon1()
        {
            GameObject LazerMon1 = PoolManager.Instance.GetPoolObject(PoolObjectType.LazerMon1);
            if (LazerMon1 == null)
            {
                Debug.LogError("Failed to get LazerMon1 from the pool.");
                return;
            }

            Vector3 spawnPosition = Effect2.position;
            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                LazerMon1.transform.position = hit.position;
            }
            else
            {
                Debug.LogError("LazerMon1 spawn position is not on the NavMesh.");
                return;
            }

            LazerMon1.transform.rotation = Quaternion.identity; // Reset rotation

            NavMeshAgent agent = LazerMon1.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent component missing from LazerMon1.");
                return;
            }

            StartCoroutine(EnableNavMeshAgent(agent, Effect2.position));

            MonsterInfo monsterInfo = LazerMon1.GetComponent<MonsterInfo>();
            if (monsterInfo == null)
            {
                Debug.LogError("LazerMon1 does not have a MonsterInfo component.");
                return;
            }
            monsterInfo._hp = 10;

            CapsuleCollider capsuleCollider = LazerMon1.GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                capsuleCollider.enabled = true;
            }

            LazerMon1.SetActive(true);

            Debug.Log("Spawned LazerMon1 with HP: " + monsterInfo._hp);
        }

        IEnumerator EnableNavMeshAgent(NavMeshAgent agent, Vector3 targetPosition)
        {
            yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure NavMesh is updated

            agent.enabled = false;
            agent.enabled = true;

            if (agent.isOnNavMesh)
            {
                agent.SetDestination(targetPosition);
                Debug.Log("NavMeshAgent enabled and destination set.");
            }
            else
            {
                Debug.LogError("NavMeshAgent is not on a NavMesh after enabling the agent.");
            }
        }
        // Additional logic for after the laser has reached the boss's position
    }

    /*  기믹2 패턴  */
    #region Stage2
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

    public void GimikAgain()
    {
        Debug.Log("Stopping Laser");
        IsUsingLaser = false; // 레이저 사용 여부 해제
        // 레이저 중지 로직 구현

        // 현재 기믹을 다시 실행하기 위해 코루틴 시작
    }

   

    public void ThrowRock(Vector3 targetPosition)
    {
        Debug.Log("Throwing Rock at Position: " + targetPosition);
        // 바위 던지기 로직 구현
    }
    #endregion
}

/* 보스 대기 상태 */
public class NoState : IBossState
{
    public bool timer = false;
    public bool isChange = false;
    public void Enter(Boss boss)
    {
        Debug.Log("1");
        boss.SetAnimation("Idle1");
        if(boss.previousState != null) boss.StartBossCoroutine(changeClass(this,1f), 10f);
    }

    public void Execute(Boss boss)
    {
        Debug.Log("기본1탓어요");
        // 게임 시작 시 if문 체크하지만 FAlSE 되어서 실행 안 됨 인스펙터에서 체크해야 탐 
        // 보스맵 진입 안 하고 있으면 여기서 Loop돔
        // 게임 스타트 하면 강제로 상태를 State1 로 변환하여 문어 스킬 발동
        if (boss.bosssRoomStartCheck) boss.ChangeState(new Stage1());

        // 상태 변하지 않았다면 null이라 여기서 걸림 
        // 상태 바뀐 시점부터는 NoState상태로 바꾸는 처리가 있을 때마다 아래 함수로 진입
        if (boss.previousState != null) boss.CheckHealthAndChangeState(isChange, boss);
    }

    public void Exit(Boss boss)
    {
        
    }
    public override string ToString()
    {
        return "NoState";
    }
    public IEnumerator changeClass(NoState noteState, float endTime)
    {
        Debug.Log("타이머 on...");
        yield return new WaitForSeconds(endTime);
        Debug.Log("3초 지났어요....");
        isChange = true;
    }

}

/* 보스 Stage1 Class */
public class Stage1 : IBossState
{
    
    private GameObject activeDangerLine;

    public void Enter(Boss boss)
    {
        boss.bosssRoomStartCheck = false;
        foreach (string tag in boss.playerTags5)
        {
            GameObject player = GameObject.FindGameObjectWithTag(tag);
            if (player != null)
            {
                boss.players.Add(player);
            }
        }
        boss.SetAnimation("Stage1"); // Idle 애니메이션 설정
    }

    public void Execute(Boss boss)
    {
    }

    public void Exit(Boss boss)
    {
        boss.players.Clear();
    }
    public override string ToString()
    {
        return "Stage1";
    }
    // 1-3 IDangerStart 코루틴 실행
    public IEnumerator IDangerStart(Boss boss)
    {
        boss.transform.LookAt(boss.players[Random.Range(0, boss.players.Count)].transform);
        DangerLineStart(boss);
        yield return null;
    }
    // 1-4 코루틴이 돌아가면 아래 함수가 실행 됨
    void DangerLineStart(Boss boss)
    {
        bool charge = true; // charge 효과가 활성화되었는지 여부를 추적하는 부울 변수 초기화
        foreach (GameObject player in boss.players) // 보스의 플레이어 목록에서 각 플레이어를 반복
        {
            if (player != null) // 플레이어 객체가 null이 아닌지 확인
            {
                if (charge) // charge 효과가 아직 활성화되지 않은 경우 확인
                {
                    GameObject chargeEffect = PoolManager.Instance.GetPoolObject(PoolObjectType.DangerChage); // charge 효과 객체를 풀에서 가져옴
                    chargeEffect.GetComponent<DangerCharge>().poolinfo = chargeEffect; // 풀 정보를 DangerCharge 컴포넌트에 설정하여 반환할 수 있도록 함
                    chargeEffect.transform.position = boss.transform.position; // charge 효과 위치를 보스의 위치로 설정
                    chargeEffect.SetActive(true); // charge 효과 활성화
                    charge = false; // charge 효과가 한 번만 활성화되도록 부울 변수 설정
                }

                GameObject activeDangerLine = PoolManager.Instance.GetPoolObject(PoolObjectType.DangerLine); // DangerLine 객체를 풀에서 가져옴
                DangerLine dangerLineComponent = activeDangerLine.GetComponent<DangerLine>(); // DangerLine 객체에서 DangerLine 컴포넌트를 가져옴

                if (dangerLineComponent != null) // DangerLine 컴포넌트가 null이 아닌지 확인
                {
                    Vector3 direction = (player.transform.position - boss.transform.position).normalized; // 보스에서 플레이어로의 방향 벡터 계산
                    direction.y = 0f; // 방향 벡터의 y 성분을 0으로 설정하여 수평을 유지

                    float extendLength = 5f; // 선을 확장할 길이 설정
                    Vector3 extendedEndPosition = player.transform.position + direction * extendLength; // 확장된 끝 지점 계산

                    // y축을 고정된 값에서 오프셋만큼 올림 (예: boss.transform.position.y + 1.0f)
                    float yOffset = 1.0f; // 원하는 y축 오프셋 값
                    extendedEndPosition.y = boss.transform.position.y + yOffset; // 끝 지점의 y축을 보스 위치의 y축 + 오프셋 값으로 설정

                    boss.setDangerPosition.Add(extendedEndPosition); // 보스의 위험 위치 목록에 끝 지점 추가
                    dangerLineComponent.EndPosition = extendedEndPosition; // DangerLine 컴포넌트의 끝 지점 설정
                    activeDangerLine.transform.position = boss.transform.position; // DangerLine 객체의 위치를 보스의 위치로 설정
                    activeDangerLine.SetActive(true); // DangerLine 객체 활성화
                                                      // 코루틴 종료 시간 동안 DangerLine 객체를 풀로 반환하는 코루틴 시작
                    boss.StartBossCoroutine(ReturnDangerLineToPool(activeDangerLine, dangerLineComponent.GetComponent<TrailRenderer>().time), dangerLineComponent.GetComponent<TrailRenderer>().time);
                }
            }
        }
    }


    public IEnumerator ReturnDangerLineToPool(GameObject dangerLine, float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.CoolObject(dangerLine, PoolObjectType.DangerLine);
        dangerLine.SetActive(false);
        DangerLine dangerLineComponent = dangerLine.GetComponent<DangerLine>();
        if (dangerLineComponent != null)
        {
            dangerLineComponent.cleartr(); 
        }
    }

    // 2-3 
    public IEnumerator IDangerEnd(Boss boss)
    {
        DangerLineEnd(boss);
        yield return new WaitForSeconds(3f);
        boss.ChangeState(new NoState());
    }
    // 2-4
    void DangerLineEnd(Boss boss)
    {
        foreach (Vector3 setDangerPositions in boss.setDangerPosition)
        {
            if (setDangerPositions != null)
            {
                GameObject activeDangerAttack = PoolManager.Instance.GetPoolObject(PoolObjectType.DangerAttack);
                activeDangerAttack.transform.position = boss.transform.position;
                activeDangerAttack.transform.LookAt(setDangerPositions);
                Vector3 eulerAngles = activeDangerAttack.transform.rotation.eulerAngles;
                eulerAngles.x = 0;
                activeDangerAttack.transform.rotation = Quaternion.Euler(eulerAngles);
                activeDangerAttack.SetActive(true);
                
            }
        }
        boss.setDangerPosition.Clear();
    }
}

/* 보스 실행On Stage2 */
public class Stage2 : IBossState
{
    

    private Transform playerTransform; // 플레이어의 Transform

    public void Enter(Boss boss)
    {
        boss.SetAnimation("Stage2"); // Gimmick1 애니메이션 설정
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
        boss.GimikAgain(); // 상태 종료 시 레이저 중지
    }
    public override string ToString()
    {
        return "Stage2";
    }
}

/* 보스 실행On Stage3 */
public class Stage3 : IBossState
{
    

    private Transform playerTransform; // 플레이어의 Transform

    public void Enter(Boss boss)
    {
        boss.SetAnimation("Stage3"); // Stage3 애니메이션 설정
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform 가져오기
    }

    public void Execute(Boss boss)
    {
        // 필요 시 추가 로직 추가
    }

    public void Exit(Boss boss)
    {
        boss.GimikAgain(); // 상태 종료 시 레이저 중지
    }
    public override string ToString()
    {
        return "Stage3";
    }
}

/* 보스 실행On Stage4 */
public class Stage4 : IBossState
{
    

    private Transform playerTransform; // 플레이어의 Transform

    public void Enter(Boss boss)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform 가져오기
    }

    public void Execute(Boss boss)
    {
        // 필요 시 추가 로직 추가
    }

    public void Exit(Boss boss)
    {
        boss.GimikAgain(); // 상태 종료 시 레이저 중지
    }
    public override string ToString()
    {
        return "Stage4";
    }
}