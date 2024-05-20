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
    public GameObject talktext1_1;
    public TextMeshProUGUI text2;
    public GameObject talktext1_2;
    public GameObject talktext1_3;
    public GameObject talktext1_4;

    public GameObject talktext2_1;
    public GameObject talktext2_2;
    public GameObject talktext2_3;
    public GameObject talktext2_4;

    public GameObject talktext3;

    public GameObject MoveInfo;
    public GameObject AttackInfo;
    public Transform tree1;
    public Transform tree2;
    public BoxCollider treecollider;

    public GameObject portalDoor;
    
    private bool IsNear=false;
    public CinemachineVirtualCamera talkcamera;
    public CinemachineVirtualCamera portalcamera;
    public CinemachineVirtualCamera moncamera;
    public CinemachineVirtualCamera chestcamera;
    

    public GameObject Monster;
    public GameObject player;
    public GameObject chest;
    public ParticleSystem chesteffect;

    public Transform TR;
    
    void Update()
    {
       
        
        //대화1
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
            Invoke("MonsterBallunAppear", 1f);
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 3 && talkIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            monsterBullon.SetActive(false);
            moncamera.Priority = 3;
            talkBullon.SetActive(false);
            AttackInfo.SetActive(true);
            Invoke("MonsterAppear", 2.5f);
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 4 && talkIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            monsterBullon.SetActive(false);
            moncamera.Priority = 0;
            Invoke("talkballonAppear", 2f);
            talktext1_3.SetActive(false);
            talktext1_4.SetActive(true);
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 5 && talkIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            talktext1_4.SetActive(false);
            talktext2_1.SetActive(true);
            talkBullon.SetActive(false); 
            talkcamera.Priority = 0;
            player.GetComponent<PlayerInput>().enabled = true;
            talkIndex = 1;
            talkCount = 0;
        }
        //대화2
        else if (IsNear == true && talkCount == 0 && talkIndex == 1 && Input.GetKeyDown(KeyCode.F))
        {
            talkcamera.Priority = 2;
            TalkButton.SetActive(false);
            AttackInfo.SetActive(false);
            Invoke("talkballonAppear", 2f);          
            player.GetComponent<PlayerInput>().enabled = false;
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 1 && talkIndex == 1 && Input.GetKeyDown(KeyCode.F))
        {
            talktext2_1.SetActive(false);
            talktext2_2.SetActive(true);
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 2 && talkIndex == 1 && Input.GetKeyDown(KeyCode.F))
        {
            chestcamera.Priority = 3;
            talkBullon.SetActive(false);
            Invoke("ChestAppear", 2f);
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 3 && talkIndex == 1 && Input.GetKeyDown(KeyCode.F))
        {
            chestcamera.Priority = 0;
            Invoke("talkballonAppear", 2f);
            talktext2_2.SetActive(false);
            talktext2_3.SetActive(true);
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 4 && talkIndex == 1 && Input.GetKeyDown(KeyCode.F))
        {
            portalcamera.Priority = 3;
            Invoke("TreeOpen", 2f);
            StartCoroutine(ShakeCamera());
            talkBullon.SetActive(false);

            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 5 && talkIndex == 1 && Input.GetKeyDown(KeyCode.F))
        {
            portalcamera.Priority = 0;
            Invoke("talkballonAppear", 2f);
            talktext2_3.SetActive(false);
            talktext2_4.SetActive(true);

            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 6 && talkIndex == 1 && Input.GetKeyDown(KeyCode.F))
        {
            talktext2_4.SetActive(false);
            talktext3.SetActive(true);
            talkBullon.SetActive(false);
            talkcamera.Priority = 0;
            player.GetComponent<PlayerInput>().enabled = true;
            talkIndex = 2;
            talkCount = 0;
        }
        //대화3
        else if (IsNear == true && talkCount == 0 && talkIndex == 2 && Input.GetKeyDown(KeyCode.F))
        {
            talkcamera.Priority = 2;
            TalkButton.SetActive(true);
            Invoke("talkballonAppear", 2f);
            player.GetComponent<PlayerInput>().enabled = false;
            talkCount += 1;
        }
        else if (IsNear == true && talkCount == 1 && talkIndex == 2 && Input.GetKeyDown(KeyCode.F))
        {
            talkBullon.SetActive(false);
            talkcamera.Priority = 0;
            player.GetComponent<PlayerInput>().enabled = true;
            talkCount = 0;
        }

        //if(Input.GetKeyDown(KeyCode.M)) 
        //{
        //    ShakePoint();
        //}
    }

    void ShakePoint()
    {
        TR.DOShakePosition(2, 1, 10, 5, false, true);
    }
    IEnumerator ShakeBullon()
    {
        float time = 3f;
        float shakePower = 500f;
        Vector3 origin = monsterBullon.transform.position;

        while(time>0f)
        {
            time -= 0.1f;
            monsterBullon.transform.position = origin + (Vector3)Random.insideUnitCircle * shakePower * time;
            yield return null;
        }

        monsterBullon.transform.position = origin;
    }
    IEnumerator ShakeCamera()
    {
        float time = 30f;
        float shakePower = 0.1f;
        Vector3 origin = portalcamera.transform.position;

        while (time > 0f)
        {
            time -= 0.1f;
            portalcamera.transform.position = origin + (Vector3)Random.insideUnitCircle * shakePower;// * time;
            yield return null;
        }

        portalcamera.transform.position = origin;
    }
    void TreeOpen()
    {
        tree1.transform.DOMove(new Vector3(7f, 0f, 7f), 5.0f);
        tree2.transform.DOMove(new Vector3(12f, 0f, 2.5f), 5.0f);
        portalDoor.SetActive(true);
        tree1.GetComponent<BoxCollider>().enabled=false;
    }
    void MonsterBallunAppear()
    {
        
        monsterBullon.SetActive(true);
        StartCoroutine(ShakeBullon());
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
    void ChestAppear()
    {
        chest.SetActive(true);
        chesteffect.Play();


    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            
            IsNear = true;
            TalkButton.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            IsNear = false;
            TalkButton.SetActive(false);
        }
    }
}
