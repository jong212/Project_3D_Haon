using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getPool : Singleton<getPool>
{
    PoolManager poolManager;
    protected void Awake()
    {
        poolManager = GetComponent<PoolManager>();
    }
    protected void Spawn()
    {
        int ran = Random.Range(0, 6);
        poolManager.GetFromPool<DangerLine>(0);
    }
    
}