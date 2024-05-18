using Redcode.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    DangerLine,
    Test
}
[Serializable]
public class PoolInfo
{
    public PoolObjectType type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject container;
    public List<GameObject> pool = new List<GameObject>(); 
}

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