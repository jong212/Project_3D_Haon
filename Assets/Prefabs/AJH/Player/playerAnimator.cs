using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static DataManager;

public class playerAnimator : MonoBehaviour
{
    public Animator _animator;
    private CharacterController _characterController;   
    private Vector3 _moveDirection;
    private bool _isRunning = false; // 뛰기 상태를 추적하는 변수
    private int _skillA = -1;
    private int _skillB = -1;
    public bool isAction = false;
    private float _gravity = -9.81f;
    private float _velocity;
    private static string MyObjectName;
    private static string _PlayerName;
    private static int _hp;
    private static int _level; 
    private static int _str;
    private static bool isSkillACooldown = false;
    private static bool isSkillBCooldown = false;


    [SerializeField]
    private Collider WeaponCollider;

    void Start()
    {
        StartCoroutine(SkillCooldown());
        MyObjectName = gameObject.name;
        PlayerData playerData = DataManager.Instance.GetPlayer($"{MyObjectName}");
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        SetPlayerData(playerData);
        Debug.Log(_PlayerName);
        Debug.Log(_hp);
        Debug.Log(_level);
        Debug.Log(_str);

    }
    IEnumerator SkillCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초마다 체크

            // 스킬 A의 쿨다운이 활성화되어 있는 경우
            if (isSkillACooldown)
            {
                yield return new WaitForSeconds(5f); //5초간 대기
                isSkillACooldown = false; // 쿨다운 종료
            }

            // 스킬 B의 쿨다운이 활성화되어 있는 경우
            if (isSkillBCooldown)
            {
                yield return new WaitForSeconds(5f); // 5초간 대기
                isSkillBCooldown = false; // 쿨다운 종료
            }
        }
    }
    private static void SetPlayerData(PlayerData playerData)
    {
        _PlayerName = playerData.name;
        _hp = playerData.hp;
        _level = playerData.level;
        _str = playerData.str;
    }

    void Update()
    {
        ApplyGravity();
        if (isAction) return;

        bool hasControl = (_moveDirection != Vector3.zero);
        if (hasControl)
        {
            // 이동 방향으로 캐릭터를 회전시킵니다.
            if (_characterController.isGrounded)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            // 캐릭터를 이동시킵니다.
            _characterController.Move(_moveDirection * 6f * Time.deltaTime);

            // 뛰기 상태를 설정합니다.
            _animator.SetBool("isRunning", _isRunning);
        }
        else
        {
            _animator.SetBool("isRunning", false); // 이동하지 않을 때는 뛰기 상태 해제
        }
    }
    public void TakeDamage(int damageAmout)
    {
        Debug.Log($"공격 당함!!! Current Hp : {_hp}");
        _hp -= damageAmout;
        if (_hp <= 0)
        {
           
        }
        else
        {
            _animator.SetTrigger("hitCharacter");
        }
    }
    void ApplyGravity()
    {
        // 중력을 적용합니다.
        if (!_characterController.isGrounded)
        {
            _velocity += _gravity * Time.deltaTime;
        }
        else
        {
            _velocity = 0f;
        }

        // 수직 이동을 적용합니다.
        _moveDirection.y = _velocity;
    }

    #region SEND_MESSAGE
    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>(); // 입력 받은 값을 가져오기
        if (input != null)
        {
            _moveDirection = new Vector3(input.x, 0f, input.y);

            // 이동 입력이 있을 때만 뛰기 상태로 변경
            _isRunning = _moveDirection.magnitude > 0;
        }
    }

    void OnSkillA(InputValue value)
    {
        if (!isSkillACooldown)
        {
            _animator.SetInteger("skillA", 0);
            _animator.Play("ChargeSkillA_Skill");
            isSkillACooldown = true; // 스킬 A 쿨다운 활성화
        }
    }

    void OnSkillB(InputValue value)
    {
        if (!isSkillBCooldown)
        {
            //        _animator.SetInteger("skillB", 0);
            //      _animator.Play("SkillA_unlock 1");
            if (isAction) return;
            StartCoroutine(ActionTimer("SkillA_unlock 1", 2.2f));
            isSkillBCooldown = true;
        }
    }

    public void onWeaponAttack()
    {
        _animator.SetTrigger("onWeaponAttack");
    }

    void OnClick()
    {
        //if (isAction) return;
        //StartCoroutine(ActionTimer("onWeaponAttack", 2.2f));
        onWeaponAttack();
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