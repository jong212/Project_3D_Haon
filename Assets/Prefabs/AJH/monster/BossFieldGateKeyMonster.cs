using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFieldGateKeyMonster : MonoBehaviour
{
    public GameObject trigger;
    public MonsterInfo kyeMonster;

    void KeyActive()
    {
        if (kyeMonster._hp <= 0 || kyeMonster.gameObject.activeSelf == false)
        {
            trigger.SetActive(true);
        }
    }

}
