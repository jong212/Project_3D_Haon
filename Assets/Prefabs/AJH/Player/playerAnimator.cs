using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static DataManager;

public class playerAnimator : MonoBehaviour
{
    private System.Random random;
    //������ ����
    public GameObject skillControlObject;
    public ShieldCollision shieldCollision;
    public SkillControl skill;
    public Animator _animator;
    private CharacterController _characterController;
    private Vector3 _moveDirection;              // �÷��̾��� �̵� ����
    private bool _isRunning = false;             // �÷��̾ �޸��� �ִ��� ���θ� �����ϴ� �÷���
    private int _skillA = -1;                    // ��ų A ����ó�� ���� ���� ��ų
    private int _skillB = -1;                    // ��ų B �پ �ٸ��콺ó�� ��½�ų 
    public bool isAction = false;                // �÷��̾ �׼��� ���� ������ ���θ� �����ϴ� �÷���
    private float _gravity = -9.81f;             // �߷� ���ӵ�
    private float _velocity;                     // �÷��̾��� ���� �ӵ�
    private static string MyObjectName;          // �÷��̾� ������Ʈ �̸�
    private static string _PlayerName;           // �÷��̾� �̸�  
    public int _hp = 9000;                      // �÷��̾� ü��
    private static int _level;                   // �÷��̾� ����
    public int _str=200;                     // �÷��̾� ��
    private static bool isSkillACooldown = false;// ��ų A ��ٿ� ���θ� �����ϴ� �÷���
    private static bool isSkillBCooldown = false;// ��ų B ��ٿ� ���θ� �����ϴ� �÷���
    public float dashCooldownDuration = 5f;      // ��� ��ٿ� ���� �ð�(��)
    private bool isDashCooldown = false;         // ��� ��ٿ� ���¸� �����ϴ� �÷�
    private bool canInput = true;
    private bool isKnockedBack = false;
    private GameObject attack;

    private bool bossstart;
    public bool BossStart
    {
        get { return bossstart; }
        set { bossstart = value; }
    }
    [SerializeField] private Vector3 initialPosition; // �ʱ� ��ġ�� ������ ����
    FloatingHealthBar healthBar;
    [SerializeField]
    private Collider WeaponCollider;             // ���� �ݶ��̴� 
    [SerializeField] private PlayerAttackSound playerSound;
    [SerializeField]
    private Canvas _hpCanvas;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

    }

    void Start()
    {
        random = new System.Random();
        if (transform.Find("EffectParents").gameObject != null)
        {
            attack = transform.Find("EffectParents").gameObject; // �� �÷��̾� ������Ʈ ������ ����Ʈ �θ� ������Ʈ�� ã���ϴ�.
        }

        //GameObject hpObject = Instantiate(PrefabReference.Instance.hpBarPrefab);
        //hpObject.transform.SetParent(_hpCanvas.transform);
        //healthBar = hpObject.GetComponentInChildren<FloatingHealthBar>();
        //healthBar.SetTarget(transform);

        if (skillControlObject != null)
        {
            skill = skillControlObject.GetComponent<SkillControl>();
        }

        MyObjectName = gameObject.name;          // �÷��̾� ������Ʈ�� �̸� ��������
        //PlayerData playerData = DataManager.Instance.GetPlayer($"{MyObjectName}"); // DataManager�� ����Ͽ� �÷��̾� ������ ��������

        LoadPlayerDataFromUserData();
        //Debug.Log(UserData.Instance.Character.MaxHealth);
        var behaviours = _animator.GetBehaviours<isAttackStop>();
        foreach (var behaviour in behaviours)
        {
            behaviour.shieldCollision = shieldCollision;
        }

        //SetPlayerData(playerData);
    }

    // UserData �̱����� ����Ͽ� �÷��̾� ������ �ε�
    private void LoadPlayerDataFromUserData()
    {
        if (UserData.Instance != null)
        {
            //_PlayerName = UserData.Instance.Character.PlayerName;
            _hp = 9000;
            _str = 200;
        }
        else
        {
            Debug.LogError("UserData is not loaded or user is not logged in.");
        }
       // Debug.Log($"{_PlayerName}, {_hp}, {_str}");
    }

    // �÷��̾� ������ ���� �Լ�
    private void SetPlayerData(PlayerData playerData)
    {
        _PlayerName = playerData.name;
        _hp = playerData.hp;
        _level = playerData.level;
        _str = playerData.str;
    }
    public void attackEvent(string type)
    {
        if (attack != null)
        {
            GameObject effect = attack.transform.Find($"attack{type}").gameObject;
            if (effect.activeSelf)
            {
                effect.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                effect.SetActive(true);
            }
        }
    }



    private IEnumerator SkillASlashLoop()
    {
        attack.transform.Find($"Slash").gameObject.SetActive(true);
        yield return new WaitForSeconds(2.9f);
        attack.transform.Find($"Slash").gameObject.SetActive(false);
    }

    public int getstr
    {
        get { return _str; }
        set { _str = value; }
    }
    void Update()
    {

        ApplyGravity();
        if (isAction) return; //�������̰ų� 2����ų �ߵ����� �� ĳ���̵� X�ϱ� ���� return

        bool hasControl = (_moveDirection != Vector3.zero);
        if (hasControl && !isKnockedBack)
        {
            if (_characterController.isGrounded)// �̵� �������� ĳ���͸� ȸ����ŵ�ϴ�.
            {
                Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            _characterController.Move(_moveDirection * 6f * Time.deltaTime);// ĳ���͸� �̵���ŵ�ϴ�.
            _animator.SetBool("isRunning", _isRunning);// �ٱ� ���¸� �����մϴ�.
        }
        else
        {
            _animator.SetBool("isRunning", false); // �̵����� ���� ���� �ٱ� ���� ����
        }
        if (transform.position.y < -10 && bossstart)
        {
            Vector3 newPosition = new Vector3(transform.position.x, -2, transform.position.z);
            transform.position = newPosition;
        }

    }

    // �÷��̾ ���ظ� ���� �� ȣ��Ǵ� �Լ�
    public void TakeDamage(int damageAmout, string bossAttack = "")
    {
        //Debug.Log($"���� ����!!! Current Hp : {_hp}");
        _hp -= damageAmout;
        if (_hp <= 0)
        {


            _animator.SetTrigger("Die");
            gameObject.SetActive(false);

        }
        else
        {


            // 0���� 99 ������ ������ �������� �����Ͽ� 15% Ȯ�� Ȯ��
            if (bossAttack == "noattack" || random.Next(100) < 2) //15% Ȯ��
            {
                _animator.Play("backDown");
                ApplyKnockback();
            }

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
    // �߷��� �����ϴ� �Լ�
    void ApplyGravity()
    {

        if (!_characterController.isGrounded) // �߷��� �����մϴ�.
        {
            _velocity += _gravity * Time.deltaTime;
        }
        else
        {
            _velocity = 0f;
        }
        _moveDirection.y = _velocity; // ���� �̵��� �����մϴ�.
    }

    #region SEND_MESSAGE
    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>(); // �Է� ���� ���� ��������
        if (input != null)
        {
            _moveDirection = new Vector3(input.x, 0f, input.y);
            _isRunning = _moveDirection.magnitude > 0;// �̵� �Է��� ���� ���� �ٱ� ���·� ����
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
        Vector3 dashDirection = transform.forward; // �÷��̾ ���� �ִ� �������� ���
        if (playerSound != null) { playerSound.Dash(); }

        float dashDistance = 5f;  // ��� �Ÿ�
        float dashDuration = 0.2f; // ��� ���� ��

        // ��� ������ ��ġ ���
        Vector3 dashDestination = transform.position + dashDirection * dashDistance;
        attack.transform.Find("Dash").gameObject.SetActive(true);
        // �÷��̾��� ��ġ�� ������ �̵��Ͽ� ��� ����
        StartCoroutine(MovePlayerToPosition(transform.position, dashDestination, dashDuration));

        // ���⿡ ��� �ִϸ��̼� ����� ���� �߰� �۾��� �߰� ����
    }

    // �÷��̾ ��� ������ ��ġ�� �ε巴�� �̵���Ű�� �ڷ�ƾ
    IEnumerator MovePlayerToPosition(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
            attack.transform.Find("Dash").gameObject.SetActive(false);
        }

        transform.position = endPosition;// �÷��̾ ��Ȯ�� ��ġ�� �����ϵ��� ����
    }

    public void OnSkillA(InputValue value = null)
    {
        if (value != null && !skill.isHideSkills[1])
        {
            skill.HideSkillSetting(1);
            return;
        }
        if (playerSound != null) { playerSound.SkillA(); }

        Debug.Log("��ų A ���� ���");
        _animator.SetInteger("skillA", 0);// ��ų A �ִϸ��̼� ���
        _animator.Play("ChargeSkillA_Skill"); // ��ų A ���� �ִϸ��̼� ���
        StartCoroutine(SkillASlashLoop());
    }

    public void OnSkillB(InputValue value = null)
    {
        if (value != null && !skill.isHideSkills[2])
        {
            skill.HideSkillSetting(2);
            return;
        }
        if (skill.getSkillTimes[2] > 0) return;
        if (playerSound != null) { playerSound.SkillB(); }

        Debug.Log("��ų B ���� ���");
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

    // 1 10
    // 1 16 2 13

    //00
    public void SkillBEffectGround()
    {
        if (attack != null)
        {
            GameObject Vortex = attack.transform.Find($"Vortex").gameObject;

            if (Vortex.activeSelf)
            {
                Vortex.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                Vortex.SetActive(true);
            }
        }
    }

    //0 27
    public void SkillBEffectWeapon()
    {
        if (attack != null)
        {
            GameObject Electric = attack.transform.Find($"Electric").gameObject;

            if (Electric.activeSelf)
            {
                StartCoroutine(RepeatPlayParticleSystem(0.1f, 0.5f, 5));
            }
            else
            {
                Electric.SetActive(true);
            }
        }
    }
    //1 17
    public void SkillBEffectExplosion()
    {
        if (attack != null)
        {
            GameObject Explosion = attack.transform.Find($"Explosion").gameObject;
            GameObject Electricity = attack.transform.Find($"Electricity").gameObject;
            GameObject Aura = attack.transform.Find($"Aura").gameObject;

            if (Explosion.activeSelf)
            {
                Explosion.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                Explosion.SetActive(true);
            }

            if (Electricity.activeSelf)
            {
                Electricity.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                Electricity.SetActive(true);
            }

            if (Aura.activeSelf)
            {
                Aura.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                Aura.SetActive(true);
            }
        }
    }
    private IEnumerator RepeatPlayParticleSystem(float interval, float totalDuration, int repeatCount)
    {
        float elapsedTime = 0f;
        int currentCount = 0;

        while (elapsedTime < totalDuration && currentCount < repeatCount)
        {
            attack.transform.Find($"Electric").gameObject.GetComponent<ParticleSystem>().Play(); // 파티클 시스템 재생
            currentCount++;
            yield return new WaitForSeconds(interval); // 간격 대기
            elapsedTime += interval;
        }
    }
}