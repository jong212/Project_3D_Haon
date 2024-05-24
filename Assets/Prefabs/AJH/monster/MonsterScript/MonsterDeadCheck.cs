using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadCheck : MonoBehaviour
{
    public int hp;
    public static bool isDead=false;

    
    void Update()
    {
        hp = this.GetComponent<MonsterInfo>()._hp;
        if (hp <= 0) 
        {
            isDead = true;
        }
    }
}
