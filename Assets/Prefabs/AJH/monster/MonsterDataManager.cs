using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataManager : Singleton<MonsterDataManager>
{
    static private Dictionary<string, MonsterData> monsterDictionary = new Dictionary<string, MonsterData>();

    void Awake()
    {
        InitializeMonsterData();
    }

    //몬스터 정보
    private void InitializeMonsterData()
    {
        AddMonster("mon1", new MonsterData("Monster1", 1, 100, 10));
        AddMonster("mon2", new MonsterData("Monster2", 2, 200, 20));
        AddMonster("mon3", new MonsterData("Monster3", 3, 300, 30));
        AddMonster("mon4", new MonsterData("Monster4", 4, 400, 40));
        AddMonster("mon5", new MonsterData("Monster5", 5, 500, 50));
        AddMonster("mon6", new MonsterData("Monster6", 6, 600, 60));
        AddMonster("mon7", new MonsterData("Monster7", 7, 700, 70));
        AddMonster("mon8", new MonsterData("Monster8", 8, 800, 80));
        AddMonster("mon9", new MonsterData("Monster9", 9, 900, 90));
        AddMonster("mon10", new MonsterData("Monster10", 10, 1000, 100 ));

    }
    //몬스터 정보 Dictionary에 넣기
    public void AddMonster(string monsterID, MonsterData monsterData)
    {
        if (!monsterDictionary.ContainsKey(monsterID))
        {
            monsterDictionary.Add(monsterID, monsterData);
        }
        else
        {
            Debug.LogWarning("Monster with ID " + monsterID + " already exists in the dictionary.");
        }
    }

    //몬스터 정보 가져오기(필요하면 갖다 쓸 수 있도록 함수 만들어 놓음)
    public MonsterData GetMonster(string monsterID)
    {
        if (monsterDictionary.ContainsKey(monsterID))
        {

            return monsterDictionary[monsterID];
        }
        else
        {
            Debug.LogWarning("Monster with ID " + monsterID + " does not exist in the dictionary.");
            return null;
        }
    }
    public class MonsterData
    {
        
        public string name;
        public int level;
        public int hp;
        public int str;



        public MonsterData(string name, int level, int hp, int str)
        {
            this.name = name;
            this.level = level;
            this.hp = hp;
            this.str = str;
        }
    }
}
