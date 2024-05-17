using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;
public class NPC : MonoBehaviour
{
    
    
    public SphereCollider talkCollider;
    public GameObject Mark;
    public GameObject TalkButton;
    
    public int talkCount = 0;
    public int talkIndex = 0;
    public GameObject talkBullon;
    public GameObject monsterBullon;
    public GameObject talkBallun2;
    public GameObject talktext1_1;
    public GameObject talktext1_2;
    public GameObject talktext1_3;
    public GameObject talktext1_4;



    public GameObject talktext2_1;
    public GameObject talktext2_2;
    public GameObject talktext2_3;

    public GameObject MoveInfo;
    public GameObject AttackInfo;
    public Transform tree1;
    public Transform tree2;
    public GameObject portalDoor;
    
    private bool IsNear=false;
    public CinemachineVirtualCamera talkcamera;
    public CinemachineVirtualCamera portalcamera;
    public CinemachineVirtualCamera moncamera;

    public GameObject Monster;
    public GameObject player;


   
    
    void Update()
    {
        if(IsNear==true&& talkCount == 0&&talkIndex==0&&Input.GetKeyDown(KeyCode.F))
        {
            talkcamera.Priority = 2;
            Mark.SetActive(false);
            TalkButton.SetActive(false);
            MoveInfo.SetActive(false);
            Invoke("talkballonAppear", 2f);
            player.GetComponent<PlayerInput>().enabled = false;
            talkCount+=1;
        }
        else if (IsNear == true && talkCount == 1 && talkIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            talktext1_1.SetActive(false);
            talktext1_2.SetActive(true);
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 2 && talkIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            talktext1_2.SetActive(false);
            talktext1_3.SetActive(true);
            Invoke("MonsterBallunAppear", 3f);
            talkCount += 1;
        }
        else if (IsNear == true && talkIndex == 3 && talkIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            moncamera.Priority = 3;
            talkBullon.SetActive(false);
            talkBallun2.SetActive(true);
            AttackInfo.SetActive(true);
            Invoke("MonsterAppear", 1.5f);
            talkCount += 1;
        }
        else if (IsNear == true && talkIndex == 4 && talkIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            moncamera.Priority = 0;
            talkBullon.SetActive(true);
            talkBallun2.SetActive(false);
            talktext1_3.SetActive(false);
            talktext1_4.SetActive(true);
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 5 && talkIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            talkBullon.SetActive(false);            
            talkcamera.Priority = 0;
            player.GetComponent<PlayerInput>().enabled = true;
            talkIndex = 1;
            talkCount =0;
        }

        
        //else if (IsNear == true && talkCount == 2 && Input.GetKeyDown(KeyCode.F))
        //{
        //    moncamera.Priority = 0;
        //    portalcamera.Priority = 11;
        //    talkCount += 1;
        //    Invoke("TreeOpen", 2.5f);
        //}
        //if (IsNear == true && Input.GetKeyDown(KeyCode.Escape))
        //{
        //    talkcamera.Priority = 0;
        //    moncamera.Priority = 0;
        //    portalcamera.Priority = 0;
        //    player.GetComponent<PlayerInput>().enabled = true;
        //    talkCount = 0;

        //}

    }
    void TreeOpen()
    {
        tree1.transform.DOMove(new Vector3(7f, 0f, 7f), 5.0f);
        tree2.transform.DOMove(new Vector3(12f, 0f, 2.5f), 5.0f);
        portalDoor.SetActive(true);
    }
    void MonsterBallunAppear()
    {
        
        monsterBullon.SetActive(true);
    }
    void MonsterAppear()
    {
        Monster.SetActive(true);
        Monster.transform.DOJump(new Vector3(-5f,0f,-6.5f), 2f, 1, 2f);
    }
    void talkballonAppear()
    {
        talkBullon.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("말걸래");
            IsNear = true;
            TalkButton.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("잘있어");
            IsNear = false;
            TalkButton.SetActive(false);
        }
    }
}
