using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour, IPoolObject
{
    TrailRenderer tr;
    [SerializeField] Transform bos; 
    public Vector3 EndPosition;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("??");
        tr = GetComponent<TrailRenderer>();
        tr.startColor = new Color(1,0,0,0.7f);
        tr.endColor = new Color(1, 0, 0, 0.7f);
        Destroy(gameObject, 30f) ;
        Init();
    }
    void Init()
    {
        transform.position = bos.position;
    }
    // Update is called once per frame
    void Update()
    {
            Debug.Log(bos.position);
            transform.position = Vector3.Lerp(transform.position, EndPosition, Time.deltaTime * 3.5f);
    }
    public void OnCreatedInPool()
    {
        Init();
    }
    //재사용 되어서 가져 올 때 마다
    public void OnGettingFromPool()
    {
        Init();
        tr.Clear();
    }


}
