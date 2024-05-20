using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerCharge : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public GameObject poolinfo;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particleSystem.isStopped)
        {
            PoolManager.Instance.CoolObject(poolinfo, PoolObjectType.DangerChage);
        }
    }
}
