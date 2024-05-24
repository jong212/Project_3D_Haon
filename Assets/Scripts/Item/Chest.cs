using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    
    public GameObject button;
    public BoxCollider ChestCollider;
    public GameObject Chesttop;
    
    Vector3 Openrotation;
    private bool isChest;
    public GameObject jewel;
    private void Awake()
    {
       
        Openrotation = new Vector3(-90,0,0);
        isChest=false;
        
    }
    void Update()
    {
        if (isChest==true &&Input.GetKeyDown(KeyCode.F))
        {
            ChestCollider.enabled = false;
            BoxOpen();
            button.SetActive(false);
            isChest = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            button.SetActive(true);

            isChest = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            button.SetActive(false);        
            isChest = false;
        }
    }
    void BoxOpen()
    {
        
        Chesttop.transform.DOLocalRotate(Openrotation, 2.0f);
        Invoke("JewelAppear", 0.5f);
    }
    void JewelAppear()
    {
        jewel.SetActive(true) ;
    }
}
