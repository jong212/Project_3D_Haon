using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyController : MonoBehaviour
{
    public GameObject lobbyMainUI;
    public GameObject lobbyStatusUI;
    public GameObject lobbyEquipUI;
    public GameObject settingUI;
    public GameObject gameSelect;
    public GameObject statUI;
    public GameObject stageSelect;
    public GameObject tutorialSelect;

    //설정버튼
    public void OnClickSet()
    {
        settingUI.SetActive(true);
    }
    //설정 종료
    public void OnClickSetout()
    {
        settingUI.SetActive(false);
    }
    //뒤로가기버튼
    public void OnClickBackSpace()
    {

        lobbyEquipUI.SetActive(false);
        lobbyStatusUI.SetActive(false);
        statUI.SetActive(false);
        lobbyMainUI.SetActive(true);
    }
    //타이틀버튼
    public void OnClickTitle()
    {
        //타이틀로
        Debug.Log("타이틀화면");
    }
    //종료버튼
    public void OnClickQuit()
    {
        //게임종료
        Debug.Log("게임종료");
    }
    //장비버튼
    public void OnClickEquip()
    {
        lobbyMainUI.SetActive(false);
        lobbyEquipUI.SetActive(true);
        statUI.SetActive(true);
    }
    //스탯버튼
    public void OnClickStatus()
    {
        lobbyMainUI.SetActive(false);
        lobbyStatusUI.SetActive(true);
        statUI.SetActive(true);
    }
    //시작버튼
    public void OnClickGameStartEnter()
    {
        gameSelect.SetActive(true);
        tutorialSelect.SetActive(true);
        stageSelect.SetActive(false);
    }
    
    //스테이지셀렉트
    public void OnClickStageSelect()
    {
        tutorialSelect.SetActive(false);
        stageSelect.SetActive(true);
    }
    //스테이지뒤로가기
    public void OnClickStageBackSpace()
    {
        tutorialSelect.SetActive(true);
        stageSelect.SetActive(false);
    }
    //시작버튼 종료
    public void OnClickGameStartExit()
    {
        gameSelect.SetActive(false);
    }
    public void OnClickTutorialStart()
    {
        //튜토리얼씬 전환
        Debug.Log("튜토리얼");
    }
    
    public void OnClickStage1Start()
    {
        //스테이지1씬 전환
        Debug.Log("1스테이지");
    }
    public void OnClickStage2Start()
    {
        //스테이지2씬 전환
        Debug.Log("2스테이지");
    }
    public void OnClickBossStageStart()
    {
        //보스스테이지씬 전환
        Debug.Log("보스스테이지");
    }
    


}
