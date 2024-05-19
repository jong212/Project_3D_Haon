using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
    TrailRenderer tr;
    [SerializeField] Transform bos;
    public Vector3 EndPosition;


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        tr.startColor = new Color(1, 0, 0, 0.7f);
        tr.endColor = new Color(1, 0, 0, 0.7f);
        Init();
    }

    void Init()
    {
        transform.position = bos.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, EndPosition, Time.deltaTime * 2.0f);

    }
    public void cleartr()
    {
        tr.Clear();
        tr.startColor = new Color(1, 1, 1, 1f);
        tr.endColor   = new Color(1, 1, 1, 1f);
    }

}