using Cinemachine;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using static DataManager;

public class NetworkPlayerController : NetworkBehaviour
{
    public GameObject skillControlObject;
    public SkillControlNetwork skill;
    public Animator _animator;

    private CharacterController _characterController;
    private Vector3 _moveDirection;              // 플레이어의 이동 방향
    private bool _isRunning = false;             // 플레이어가 달리고 있는지 여부를 추적하는 플래그
    private int _skillA = -1;                    // 스킬 A 가렌처럼 빙빙 도는 스킬
    private int _skillB = -1;                    // 스킬 B 뛰어서 다리우스처럼 찍는스킬 
    public bool isAction = false;                // 플레이어가 액션을 수행 중인지 여부를 추적하는 플래그
    private float _gravity = -9.81f;             // 중력 가속도
    private float _velocity;                     // 플레이어의 수직 속도
    private static string MyObjectName;          // 플레이어 오브젝트 이름
    private static string _PlayerName;           // 플레이어 이름  
    private static int _hp;                      // 플레이어 체력
    private static int _level;                   // 플레이어 레벨
    private static int _str;                     // 플레이어 힘
    private static bool isSkillACooldown = false;// 스킬 A 쿨다운 여부를 추적하는 플래그
    private static bool isSkillBCooldown = false;// 스킬 B 쿨다운 여부를 추적하는 플래그
    public float dashCooldownDuration = 5f;      // 대시 쿨다운 지속 시간(초)
    private bool isDashCooldown = false;         // 대시 쿨다운 상태를 추적하는 플래
    FloatingHealthBar healthBar;
    [SerializeField]
    private Collider WeaponCollider;             // 무기 콜라이더

    [SerializeField]
    private Canvas _hpCanvas;

    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private Vector2 defaultInitialPlanePosition = new Vector2(-14, -19);
    [SerializeField]
    private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>();

    [SerializeField]
    private float speed = 3.5f;

    [SerializeField]
    private float rotationSpeed = 1.5f;

    private Vector3 oldInputPosition;
    private Quaternion oldInputRotation;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }


    private void Start()
    {
        if (IsClient && IsOwner)
        {

            if (skillControlObject != null)
            {
                skill = skillControlObject.GetComponent<SkillControlNetwork>();
            }
            // 스킬 쿨다운을 관리하는 코루틴 시작
            StartCoroutine(SkillCooldown());
        }
    }
    void Update()
    {
        if (IsClient && IsOwner)
        {
            MoveClient();
            HandleInput();
        }
        ApplyGravity();

        if (IsServer)
        {
            _characterController.SimpleMove(networkPosition.Value);
            if (networkRotation.Value != Quaternion.identity)
            {
                transform.rotation = networkRotation.Value;
            }
        }
        
    }

    public override void OnNetworkSpawn()
    {
        transform.position = new Vector3(Random.Range(defaultInitialPlanePosition.x, defaultInitialPlanePosition.y), 1, -154);

        if (IsLocalPlayer)
        {

            MyObjectName = gameObject.name;          // 플레이어 오브젝트의 이름 가져오기
                                                     // DataManager를 사용하여 플레이어 데이터 가져오기

            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

            if (virtualCamera != null)
            {
                // Virtual Camera의 Follow 및 Look At 필드를 로컬 플레이어로 설정합니다
                virtualCamera.Follow = transform;
            }
        }

    }

    void MoveClient() // 클라이언트에서 이동 처리
    {

        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (movementInput.sqrMagnitude > 0.01f) // movementInput이 0인지 확인
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementInput);

            if (oldInputPosition != movementInput || oldInputRotation != targetRotation)
            {
                oldInputRotation = targetRotation;
                oldInputPosition = movementInput;
                MoveServerRPC(movementInput * speed, targetRotation);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                _animator.SetBool("isRunning", true);
            }
        }
        else if (oldInputPosition != Vector3.zero)
        {
            oldInputPosition = Vector3.zero;
            MoveServerRPC(Vector3.zero, Quaternion.identity);
            _animator.SetBool("isRunning", false);
        }

    }



    // 스킬 쿨다운을 관리하는 코루틴
    IEnumerator SkillCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);     // 1초마다 체크    


            if (isSkillACooldown)                    // 스킬 A 쿨다운 상태 확인 및 처리
            {
                yield return new WaitForSeconds(5f); //5초간 대기
                isSkillACooldown = false;            // 쿨다운 종료
            }


            if (isSkillBCooldown)                    // 스킬 B의 쿨다운이 활성화되어 있는 경우
            {
                yield return new WaitForSeconds(5f); // 5초간 대기
                isSkillBCooldown = false;            // 쿨다운 종료
            }
        }
    }

    // 플레이어 데이터 설정 함수
    private static void SetPlayerData(PlayerData playerData)
    {
        _PlayerName = playerData.name;
        _hp = playerData.hp;
        _level = playerData.level;
        _str = playerData.str;
    }



    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (IsOwner)
                DashServerRPC();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (IsOwner)
                SkillAServerRPC();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (IsOwner)
                SkillBServerRPC();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (IsOwner)
                ClickServerRPC();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (IsOwner)
                SkillClickServerRPC();
        }

    }

    // 플레이어가 피해를 받을 때 호출되는 함수

    public void TakeDamage(int damageAmout)
    {
        //Debug.Log($"공격 당함!!! Current Hp : {_hp}");
        _hp -= damageAmout;
        if (_hp <= 0)
        {

            _animator.SetTrigger("Die");

        }
        else
        {
            _animator.SetTrigger("hitCharacter");
        }
    }

    // 중력을 적용하는 함수
    void ApplyGravity()
    {

        if (!_characterController.isGrounded) // 중력을 적용합니다.
        {
            _velocity += _gravity * Time.deltaTime;
        }
        else
        {
            _velocity = 0f;
        }
        //_moveDirection.y = _velocity; // 수직 이동을 적용합니다.
    }

    #region SEND_MESSAGE

    void Move(Vector3 movementInput)
    {

        // 입력 받은 값을 가져오기
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.z);
        //movement.y = 0f;
        _isRunning = movement.magnitude > 0;

        bool hasControl = (movement != Vector3.zero);
        if (hasControl)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            _characterController.Move(movement * Time.deltaTime);// 캐릭터를 이동시킵니다.
            _animator.SetBool("isRunning", _isRunning);// 뛰기 상태를 설정합니다.
        }
        else
        {
            _animator.SetBool("isRunning", false); // 이동하지 않을 때는 뛰기 상태 해제
        }


    }

    public void Dash()
    {

        //if (!skill.isHideSkills[0])
        //{
        //    skill.HideSkillSetting(0);
        //    return;
        //}

        if (skill.getSkillTimes[0] > 0)
            return;

        Vector3 dashDirection = transform.forward; // 플레이어가 보고 있는 방향으로 대시
        float dashDistance = 5f;  // 대시 거리
        float dashDuration = 0.2f; // 대시 지속 시간

        // 대시 목적지 위치 계산
        Vector3 dashDestination = transform.position + dashDirection * dashDistance;

        // 플레이어의 위치를 빠르게 이동하여 대시 실행
        StartCoroutine(MovePlayerToPosition(transform.position, dashDestination, dashDuration));

        skill.isHideSkills[0] = true;
        skill.getSkillTimes[0] = skill.skillTimes[0];
    }

    // 플레이어를 대시 목적지 위치로 부드럽게 이동시키는 코루틴
    IEnumerator MovePlayerToPosition(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;// 플레이어가 정확한 위치에 도달하도록 보장
    }

    //대시스킬 Shift 추후 변경예정

    public void SkillA()
    {
        //if (!skill.isHideSkills[1])
        //{
        //    skill.HideSkillSetting(1);
        //    return;
        //}
        _animator.SetInteger("skillA", 0);// 스킬 A 애니메이션 재생
        _animator.Play("ChargeSkillA_Skill");
    }

    public void SkillB()
    {
        //if (!skill.isHideSkills[2])
        //{
        //    skill.HideSkillSetting(2);
        //    return;
        //}
        if (skill.getSkillTimes[2] > 0) return;
        StartCoroutine(ActionTimer("SkillA_unlock 1", 2.2f));
    }

    public void Click()
    {
        _animator.SetTrigger("onWeaponAttack");

    }

    public void SkillClick()
    {
        _animator.SetTrigger("onWeaponAttack");

    }

    IEnumerator ActionTimer(string actionName, float time)
    {
        isAction = true;

        if (actionName != "none") _animator.Play(actionName);

        yield return new WaitForSeconds(time);
        isAction = false;
    }
    public void EnableWeapon()
    {
        WeaponCollider.enabled = true;
    }
    public void DisableWeapon()
    {
        WeaponCollider.enabled = false;
    }
    #endregion

    [ServerRpc]
    private void MoveServerRPC(Vector3 movementInput, Quaternion rotationInput)
    {

        networkPosition.Value = movementInput;
        networkRotation.Value = rotationInput;

    }

    [ServerRpc]
    private void DashServerRPC()
    {
        Dash();
    }

    [ServerRpc]
    private void SkillAServerRPC()
    {
        SkillA();
    }
    [ServerRpc]
    private void SkillBServerRPC()
    {
        SkillB();
    }
    [ServerRpc]
    private void ClickServerRPC()
    {
        Click();
    }

    [ServerRpc]
    private void SkillClickServerRPC()
    {
        SkillClick();
    }
    [ServerRpc]
    private void ApplyGravityServerRPC()
    {
        ApplyGravity();
    }
}
