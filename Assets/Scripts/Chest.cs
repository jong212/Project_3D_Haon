using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    
    public GameObject button;
    public BoxCollider ChestCollider;
    public GameObject Chesttop;
    Vector3 Openposition;
    public Vector3 Openrotation;
    private bool isChest;
    public GameObject jewel;
    private void Awake()
    {
        //Openposition.x = Chesttop.transform.position.x;
        //Openposition.y = Chesttop.transform.position.y + 0.5f;
        //Openposition.z = Chesttop.transform.position.z+0.5f;
        Openrotation = new Vector3(-90,-180,0);
        isChest=false;
        
    }
    void Update()
    {
        if (isChest==true &&Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("열어!");
            ChestCollider.enabled = false;
            BoxOpen();
            button.SetActive(false);
            isChest = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            button.SetActive(true);
            Debug.Log("상자열래");

            isChest = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            button.SetActive(false);
            Debug.Log("상자안열래");
            isChest = false;
        }
    }
    void BoxOpen()
    {
        //Chesttop.transform.DOMove(Openposition, 2.0f);
        Chesttop.transform.DORotate(Openrotation, 2.0f);
        Invoke("JewelAppear", 0.5f);
    }
    void JewelAppear()
    {
        jewel.SetActive(true) ;
    }
}
