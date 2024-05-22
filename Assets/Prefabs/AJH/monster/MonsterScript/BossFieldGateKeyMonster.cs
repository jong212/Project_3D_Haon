using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFieldGateKeyMonster : MonoBehaviour
{
    public GameObject trigger;
    public MonsterInfo keyMonster;
    public bool testCheck = false;

    private void Update()
    {
        KeyActive();
    }

    void KeyActive()
    {
        if (keyMonster._hp <= 0 || testCheck == true)
        {
            trigger.gameObject.SetActive(true);
        }
       
    }

}
