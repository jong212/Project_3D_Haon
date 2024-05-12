using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MonsterDataManager;

public class MonsterInfo : MonoBehaviour
{
    private static string MyObjectName;
    private static string _monsterName; // name 변수를 _monsterName으로 변경
    private static int _hp; // hp 변수를 _hp로 변경
    private static int _level; // level 변수를 _level로 변경
    private static int _str; // str 변수를 _str로 변경



    void Awake()
    {
        MyObjectName = gameObject.name;
        MonsterData monsterData = MonsterDataManager.Instance.GetMonster($"{MyObjectName}");
        SetMonsterData(monsterData);
        Debug.Log("Monster Name: " + _monsterName);
        Debug.Log("Monster HP: " + _hp);
        Debug.Log("Monster Level: " + _level);
        Debug.Log("Monster Strength: " + _str);
    }
    private static void SetMonsterData(MonsterData monsterData)
    {
        _monsterName = monsterData.name;
        _hp = monsterData.hp;
        _level = monsterData.level;
        _str = monsterData.str;
    }
    public void testc()
    {
    
    }

    //GameObject monster = ObjectPool.Instance.GetInactiveObject($"mon{def}");
    //monster.GetComponent<monsterinfo>().Init(monsterData);
}
