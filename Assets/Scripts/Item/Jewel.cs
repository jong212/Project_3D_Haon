using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
public class Jewel : MonoBehaviour
{

    Vector3 jewelsposition;
    public float rotateSpeed = 100.0f;
    public BoxCollider jewelcollider;
    Vector3 upposition;
    public ParticleSystem jewelEffect;
    public GameObject magnet;

    private void Awake()
    {
        this.jewelcollider = GetComponent<BoxCollider>();
        upposition.y = transform.position.y;
        jewelsposition = this.transform.localPosition;
        
        jewelsposition.z += 2;
    }
    private void OnEnable()
    {
        Appear();
    }
 

   
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
   
    public void Appear()
    {
         
        transform.DOLocalJump(jewelsposition, 2f, 1, 2f);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Setting_1.PlusJewels();
            GetJewel();
            jewelEffect.Play();
            jewelcollider.enabled = false;
            
        }
    }
    void GetJewel()
    {
        transform.DOMoveY(upposition.y + 2, 2.0f);
        rotateSpeed = 500.0f;
        Invoke("EraseJewel", 2.0f);
    }
    void EraseJewel()
    {
        gameObject.SetActive(false);
    }
}
