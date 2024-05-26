using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static DataManager;

public class playerAnimator : MonoBehaviour
{

    //변수들 선언
    public GameObject skillControlObject;
    public ShieldCollision shieldCollision;
    public SkillControl skill;
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
    private bool canInput = true;
    private bool isKnockedBack = false;
    private GameObject attack;
    
    FloatingHealthBar healthBar;
    [SerializeField]
    private Collider WeaponCollider;             // 무기 콜라이더 
    [SerializeField] private PlayerAttackSound playerSound; 
    [SerializeField]
    private Canvas _hpCanvas; 
    void Start()
    {
        //GameObject hpObject = Instantiate(PrefabReference.Instance.hpBarPrefab);
        //hpObject.transform.SetParent(_hpCanvas.transform);
        //healthBar = hpObject.GetComponentInChildren<FloatingHealthBar>();
        //healthBar.SetTarget(transform);
         attack = GameObject.Find("EffectParents");
        
        if (skillControlObject != null)
        {
            skill = skillControlObject.GetComponent<SkillControl>();
        }

        MyObjectName = gameObject.name;          // 플레이어 오브젝트의 이름 가져오기
        PlayerData playerData = DataManager.Instance.GetPlayer($"{MyObjectName}"); // DataManager를 사용하여 플레이어 데이터 가져오기
        _animator = GetComponent<Animator>();
        var behaviours = _animator.GetBehaviours<isAttackStop>();
        foreach (var behaviour in behaviours)
        {
            behaviour.shieldCollision = shieldCollision;
        }
        _characterController = GetComponent<CharacterController>();
        SetPlayerData(playerData);
    }
    // 플레이어 데이터 설정 함수
    private static void SetPlayerData(PlayerData playerData)
    {
        _PlayerName = playerData.name;
        _hp = playerData.hp;
        _level = playerData.level;
        _str = playerData.str;
    }
    public void attackEvent(string type)
    {
        if (type == "1")
        {

            GameObject obj1 = attack.transform.Find("attack1").gameObject;
            if(obj1.activeSelf)
            {
                obj1.GetComponent<ParticleSystem>().Play();
            } else
            {
                obj1.SetActive(true);
            }
            
        } else if (type =="2")
        {
            GameObject obj2 = attack.transform.Find("attack2").gameObject;
            if (obj2.activeSelf)
            {
                obj2.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                obj2.SetActive(true);
            }
        } else
        {
            GameObject obj3 = attack.transform.Find("attack3").gameObject;
            if (obj3.activeSelf)
            {
                obj3.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                obj3.SetActive(true);
            }
        }
        
        // static 변수들은 다 public 
        // 싱글톤은 그래서 public...
        // 참조해야할  것 같으면 public
        // 변수값이 복사될 것 같으면 privic
        // 근데 이제 serialrize 직렬화를 해버리면 
    }
    public static int getstr
    {
        get { return _str; }
        set { _str = value; }
    }
    void Update()
    {
        ApplyGravity();
        if (isAction) return; //공격중이거나 2번스킬 발동중일 땐 캐릭이동 X하기 위해 return

        bool hasControl = (_moveDirection != Vector3.zero);
        if (hasControl && !isKnockedBack)
        {
            if (_characterController.isGrounded)// 이동 방향으로 캐릭터를 회전시킵니다.
            {
                Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            _characterController.Move(_moveDirection * 6f * Time.deltaTime);// 캐릭터를 이동시킵니다.
            _animator.SetBool("isRunning", _isRunning);// 뛰기 상태를 설정합니다.
        }
        else
        {
            _animator.SetBool("isRunning", false); // 이동하지 않을 때는 뛰기 상태 해제
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
            _animator.Play("backDown");
            ApplyKnockback();

        }
    }
    public void ApplyKnockback()
    {
        if (!isKnockedBack)
        {
            StartCoroutine(KnockbackCoroutine());
        }
    }

    private IEnumerator KnockbackCoroutine()
    {
        isKnockedBack = true;
        canInput = false;
        // Apply knockback effect here, e.g., add force to the Rigidbody
        yield return new WaitForSeconds(1f);
        canInput = true;
        isKnockedBack = false;
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
        _moveDirection.y = _velocity; // 수직 이동을 적용합니다.
    }

    #region SEND_MESSAGE
    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>(); // 입력 받은 값을 가져오기
        if (input != null)
        {
            _moveDirection = new Vector3(input.x, 0f, input.y);
            _isRunning = _moveDirection.magnitude > 0;// 이동 입력이 있을 때만 뛰기 상태로 변경
        }
    }
    public void OnDash(InputValue value = null)
    {
        if (value != null && !skill.isHideSkills[0])
        {
            skill.HideSkillSetting(0);
            return;
        }
        if (skill.getSkillTimes[0] > 0) return;
        Vector3 dashDirection = transform.forward; // 플레이어가 보고 있는 방향으로 대시
        playerSound.Dash();
        float dashDistance = 5f;  // 대시 거리
        float dashDuration = 0.2f; // 대시 지속 시

        // 대시 목적지 위치 계산
        Vector3 dashDestination = transform.position + dashDirection * dashDistance;

        // 플레이어의 위치를 빠르게 이동하여 대시 실행
        StartCoroutine(MovePlayerToPosition(transform.position, dashDestination, dashDuration));

        // 여기에 대시 애니메이션 재생과 같은 추가 작업을 추가 예정
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

    public void OnSkillA(InputValue value = null)
    {
        if (value != null && !skill.isHideSkills[1])
        {
            skill.HideSkillSetting(1);
            return;
        }
        playerSound.SkillA();
        Debug.Log("스킬 A 사운드 출력");
        _animator.SetInteger("skillA", 0);// 스킬 A 애니메이션 재생
        _animator.Play("ChargeSkillA_Skill"); // 스킬 A 충전 애니메이션 재생
    }

    public void OnSkillB(InputValue value = null)
    {
        if (value != null && !skill.isHideSkills[2])
        {
            skill.HideSkillSetting(2);
            return;
        }
        if (skill.getSkillTimes[2] > 0) return;
        playerSound.SkillB();
        Debug.Log("스킬 B 사운드 출력");
        StartCoroutine(ActionTimer("SkillA_unlock 1", 2.2f));

    }

    public void OnClick()
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
}