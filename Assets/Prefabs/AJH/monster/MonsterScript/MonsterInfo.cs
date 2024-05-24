using UnityEngine;
using static DataManager;

public class MonsterInfo : MonoBehaviour
{
    public string MyObjectName;
    public string _monsterName; // name 변수를 _monsterName으로 변경
    public int _hp; // hp 변수를 _hp로 변경
    public int _level; // level 변수를 _level로 변경
    public int _str; // str 변수를 _str로 변경

    //public int MonsterID;
    //public string MonsterName;
    //public int Health;
    //public int AttackPower;

    public Animator animator;
    public Transform wayPoint;

    void Awake()
    {
        
        MyObjectName = gameObject.name;

        // 위에서 언급했듯 게임오브젝트 이름으로 몬스터 정보를 가져온다.
        MonsterData monsterData = DataManager.Instance.GetMonster($"{MyObjectName}");
        animator = GetComponent<Animator>();
        //가져온 정보를 함수에 넘겨서 hp,level,str 등등 세팅
        SetMonsterData(monsterData);
        Debug.Log("1...몬스터 정보 세팅.." + _monsterName);
    }
    
    private void SetMonsterData(MonsterData monsterData)
    {
        this._monsterName = monsterData.name;
        this._hp = monsterData.hp;
        this._level = monsterData.level;
        this._str = monsterData.str;
    }

    
    public void TakeDamage(int damageAmout)
    {
        Debug.Log($"공격 당함!!! Current Hp : {_hp}");
        Debug.Log(gameObject.name);
        _hp -= damageAmout;
        if (_hp <= 0)
        {
            animator.SetTrigger("die");
            transform.GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            animator.SetTrigger("damage");
        }

    }

}