using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDatabase : MonoBehaviour
{
    public int MonsterID;
    public string MonsterName;
    public int Health;
    public int AttackPower;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Monster initialized: " + MonsterName);
    }

    
}
