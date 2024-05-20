using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    
    public Transform transform;
    public SphereCollider collider;
    void Start()
    {
        this.collider = GetComponent<SphereCollider>();
    }

    
    
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            transform.DOMove(other.transform.position,1f);
        }
    }


}
