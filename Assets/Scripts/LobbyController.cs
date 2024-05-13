using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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


    //홈버튼
    public void OnClickHome()
    {
        settingUI.SetActive(false);
        lobbyMainUI.SetActive(true);
        lobbyEquipUI.SetActive(false);
        lobbyStatusUI.SetActive(false);
        statUI.SetActive(false);
    }
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
        FadeInFadeOutSceneManager.Instance.ChangeScene("StartScene");
    }
    //종료버튼
    public void OnClickQuit()
    {

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
        //tutorialSelect.SetActive(true);
        //stageSelect.SetActive(false);
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
    
}
