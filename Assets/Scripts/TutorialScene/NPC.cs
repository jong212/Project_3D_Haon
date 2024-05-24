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
    public GameObject[] talktext1;
    //public GameObject talktext1_1;
    //public TextMeshProUGUI text2;
    //public GameObject talktext1_2;
    //public GameObject talktext1_3;
    //public GameObject talktext1_4;

    public GameObject[] talktext2;
    //public GameObject talktext2_1;
    //public GameObject talktext2_2;
    //public GameObject talktext2_3;
    //public GameObject talktext2_4;

    public GameObject talktext3;

    public GameObject MoveInfo;
    public GameObject AttackInfo;
    public Transform tree1;
    public Transform tree2;
    public BoxCollider treecollider;

    public GameObject portalDoor;
    
    private bool IsNear=false;
    private bool canProceed = true;
    public CinemachineVirtualCamera talkcamera;
    public CinemachineVirtualCamera portalcamera;
    public CinemachineVirtualCamera moncamera;
    public CinemachineVirtualCamera chestcamera;
    

    public GameObject Monster;
    public GameObject player;
    public GameObject chest;
    public ParticleSystem chesteffect;

  
    
    void Update()
    {
        if(IsNear&& Input.GetKeyDown(KeyCode.F))
        {
            if(canProceed)
            {
                HandleTalk();
            }
            
        }

       
    }
    private void HandleTalk()
    {
        canProceed = false;
        switch(talkIndex)
        {
            case 0:
                HandleFirstTalk();
                break;
            case 1:
                HandleSecondTalk();
                break;
            case 2:
                HandleThirdTalk();
                break;

        }
        canProceed= true;
    }

    //첫번째 대화

    private void HandleFirstTalk()
    {
        switch (talkCount)
        {
            case 0:
                StartTalk();
                break;
            case 1:
                ShowTalkText(talktext1[0], talktext1[1]);
                break;
            case 2:
                ShowTalkText(talktext1[1], talktext1[2]);                
                break;
            case 3:
                MonsterBallunAppear();
                HideTalk();
                break;
            case 4:
                HideMonsterBallon();
                moncamera.Priority = 3;
                ShowAttackInfo();
                HideTalk();
                Invoke("MonsterAppear", 2f);
                break;
            case 5:
                moncamera.Priority = 0;
                Invoke("ShowTalk",2f);
                ShowTalkText(talktext1[2], talktext1[3]);
                break;
            case 6:
                EndTalk(talktext1[3], talktext2[0]);
                talkIndex = 1;
                break;
        }
        talkCount++;
    }
    private void HandleSecondTalk()
    {
        switch (talkCount)
        {
            case 0:
                StartTalk();
                HideAttackInfo();
                break;
            case 1:
                ShowTalkText(talktext2[0], talktext2[1]);
                break;
            case 2:
                HideTalk();
                chestcamera.Priority = 3;
                Invoke("ChestAppear", 2f);
                
                break;
            case 3:
                Invoke("ShowTalk", 2f);
                chestcamera.Priority = 0;
                ShowTalkText(talktext2[1], talktext2[2]);
                break;
            case 4:
                HideTalk();
                portalcamera.Priority = 3;
                Invoke("TreeOpen", 2f);
                StartCoroutine(ShakeCamera());
                break;
            case 5:
                Invoke("ShowTalk", 2f);               
                portalcamera.Priority = 0;
                ShowTalkText(talktext2[2], talktext2[3]);
                break;
            case 6:
                EndTalk(talktext2[3], talktext3);
                talkIndex = 2;
                break;
        }
        talkCount++;
    }

    private void HandleThirdTalk()
    {
        
        switch (talkCount)
        {
            case 0:
                StartTalk();
                break;
            case 1:
                EndTalk(talkBullon, talktext3);
                break;
        }
        talkCount++;
        
    }
    // 대화 시작 공통 로직
    private void StartTalk()
    {
        talkcamera.Priority = 2;
        Mark.SetActive(false);
        TalkButton.SetActive(false);
        MoveInfo.SetActive(false);
        AttackInfo.SetActive(false);
        Invoke("talkballonAppear", 2f);
        player.GetComponent<PlayerInput>().enabled = false;
    }

    // 대화 텍스트 전환 로직
    private void ShowTalkText(GameObject hideText, GameObject showText)
    {
        hideText.SetActive(false);
        showText.SetActive(true);
    }

    // 몬스터 말풍선 숨기기
    private void HideMonsterBallon()
    {
        monsterBullon.SetActive(false);
    }

    // 공격 정보 보여주기
    private void ShowAttackInfo()
    {
        AttackInfo.SetActive(true);
    }

    // 공격 정보 숨기기
    private void HideAttackInfo()
    {
        AttackInfo.SetActive(false);
        
    }
    private void ShowTalk()
    {
        talkBullon.SetActive(true);
    }
    private void HideTalk()
    {
        talkBullon.SetActive(false);
    }
    // 대화 종료 공통 로직
    private void EndTalk(GameObject hideText, GameObject showText)
    {
        hideText.SetActive(false);
        showText.SetActive(true);
        talkBullon.SetActive(false);
        talkcamera.Priority = 0;
        player.GetComponent<PlayerInput>().enabled = true;
        talkCount = 0;
    }

    IEnumerator ShakeBullon()
    {
        float time = 3f;
        float shakePower = 50f;
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
        Monster.transform.DOMoveX(-2.5f, 3f);
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
