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

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //홈버튼
    void OnClickHome()
    {
        settingUI.SetActive(false);
        lobbyMainUI.SetActive(true);
        lobbyEquipUI.SetActive(false);
        lobbyStatusUI.SetActive(false);
    }
    //설정버튼
    void OnClickEnterSetting()
    {
        settingUI.SetActive(true);
    }
    void OnClickExitSetting()
    {
        settingUI.SetActive(false);
    }
    //뒤로가기버튼
    void OnClickBackSpace()
    {
        lobbyMainUI.SetActive(true);
        lobbyEquipUI.SetActive(false);
        lobbyStatusUI.SetActive(false);
    }
    //타이틀버튼
    void OnClickTitle()
    {
        FadeInFadeOutSceneManager.Instance.ChangeScene("StartScene");
    }
    //종료버튼
    void OnClickQuit()
    {

    }
    //장비버튼
    void OnClickEquip()
    {
        lobbyMainUI.SetActive(false);
        lobbyEquipUI.SetActive(true);
    }
    //스탯버튼
    void OnClickStatus()
    {
        lobbyMainUI.SetActive(false);
        lobbyStatusUI.SetActive(true);
    }
    //게임시작
    void OnClickGameStart()
    {

    }
}
