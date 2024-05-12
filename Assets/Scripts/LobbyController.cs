using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LobbyButton
{
    Home,
    BackSpace,
    Equip,
    Status,
    EnterSetting,
    ExitSetting,
    SelectStage,
    Stage1,
    Stage2,
    Boss,
    Tutorial,
    Title,
    Quit


}
public class LobbyController : MonoBehaviour
{
    public GameObject lobbyMainUI;
    public GameObject lobbyStatusUI;
    public GameObject lobbyEquipUI;
    public GameObject settingUI;
    public GameObject gameSelect;
    public GameObject statUI;
    public GameObject StageSelect;
    public LobbyButton currentType;
    public void OnClickLobbyButton()
    {
        switch (currentType)
        {
            case LobbyButton.Home:
                settingUI.SetActive(false);
                lobbyMainUI.SetActive(true);
                lobbyEquipUI.SetActive(false);
                lobbyStatusUI.SetActive(false);
                statUI.SetActive(false);
                break;
            case LobbyButton.BackSpace:
                break;
            case LobbyButton.Equip:
                break;
            case LobbyButton.Status:
                break;
            case LobbyButton.EnterSetting:
                break;
            case LobbyButton.ExitSetting:
                break;
            case LobbyButton.SelectStage:
                break;
            case LobbyButton.Stage1:
                break;
            case LobbyButton.Stage2:
                break;
            case LobbyButton.Boss:
                break;
            case LobbyButton.Tutorial:
                break;
            case LobbyButton.Title:
                break;
            case LobbyButton.Quit:
                break;
            default:
                break;
        }
    }

    //홈버튼
    public void OnClickHome()
    {
       
    }
    //설정버튼
    public void OnClickSet()
    {
        settingUI.SetActive(true);
    }
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
    //게임시작
    public void OnClickGameStartEnter()
    {
        gameSelect.SetActive(true);
    }
    public void OnClickGameStartExit()
    {
        gameSelect.SetActive(false);
    }

}
