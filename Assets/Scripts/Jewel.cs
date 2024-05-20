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

    private void Awake()
    {
        this.jewelcollider = GetComponent<BoxCollider>();
        jewelsposition.x = transform.position.x;
        jewelsposition.y = transform.position.y;
        jewelsposition.z = transform.position.z - 2;
        upposition.y = transform.position.y;
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
        transform.DOJump(jewelsposition, 2f, 1, 2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayInfo.JewelPlus();
            GetJewel();
            jewelcollider.enabled = false;
        }
    }
    void GetJewel()
    {
        Debug.Log("?");
        transform.DOMoveY(upposition.y + 2, 2.0f);
        rotateSpeed = 500.0f;
        Invoke("EraseJewel", 2.0f);
    }
    void EraseJewel()
    {
        gameObject.SetActive(false);
    }
}
