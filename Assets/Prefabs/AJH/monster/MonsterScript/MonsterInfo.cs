using Unity.VisualScripting;
using UnityEngine;
// using static DataManager;

public class MonsterInfo : MonoBehaviour
{
    public string MyObjectName;
    public string _monsterName; // name 변수를 _monsterName으로 변경
    public int _hp; // hp 변수를 _hp로 변경
    public int _level; // level 변수를 _level로 변경
    public int _str; // str 변수를 _str로 변경

    public Animator animator;
    public Transform wayPoint;

    [SerializeField] private PlayerAttackSound playerSound;
    [SerializeField] private MonsterType monsterType; //몬스터 유형 판정 컴포넌트 따로 존재(몬스터에 부착)
    

    void Awake()
    {
        //몬스터 정보를 게임오브젝트 이름으로 가져오기 위한 변수
        //MyObjectName = (gameObject.name == "LazerMon1(Clone)" || gameObject.name == "LazerMon1" || gameObject.name == "LazerMon(Clone)") ? "mon6" : gameObject.name;
        
        // 위에서 언급했듯 게임오브젝트 이름으로 몬스터 정보를 가져온다.
        //MonsterData monsterData = DataManager.Instance.GetMonster($"{MyObjectName}");
        animator = GetComponent<Animator>();
        //가져온 정보를 함수에 넘겨서 hp,level,str 등등 세팅
        //SetMonsterData(monsterData);
        //Debug.Log("1...몬스터 정보 세팅.." + _monsterName);        
    }
    //몬스터 정보 세팅
    //private void SetMonsterData(MonsterData monsterData)
    //{

    //    this._monsterName = monsterData.name;
    //    this._hp = monsterData.hp;
    //    this._level = monsterData.level;
    //    this._str = monsterData.str;
    //    if (gameObject.name == "LazerMon1(Clone)" || gameObject.name == "LazerMon1" )
    //    {
    //        this._hp = 10;
    //        this._str = 10;
    //    } else if (gameObject.name == "LazerMon(Clone)")
    //    {
    //        this._hp = 100;
    //        this._str = 100;
    //    }
    //}

    public void TakeDamage(int damageAmout)
    {
        //Debug.Log($"공격 당함!!! Current Hp : {_hp}");
        //Debug.Log(gameObject.name);
        _hp -= damageAmout;
        if (_hp <= 0)
        {
            animator.SetTrigger("die");
            playerSound.MonsterDie();//몬스터 사망 사운드 출력
            transform.GetComponent<CapsuleCollider>().enabled = false;

        }
        else
        {
            animator.SetTrigger("damage");
            if (monsterType.monsterType == 1)
            {
                playerSound.BiologyAttack();// 생물형 몬스터 타격음
            }
            else if (monsterType.monsterType == 2)
            {
                playerSound.NonBiologyAttack(); // 비생물형 몬스터 타격음
            }
        }
    }
}