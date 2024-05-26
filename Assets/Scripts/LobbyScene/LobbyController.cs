using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using static LobbyEvent;

public class LobbyController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject mainLobbyUI;
    [Header("Status")]
    [SerializeField] private GameObject statusEnhancementUI;
    [Header("Equip")]
    [SerializeField] private GameObject equipEnhancementUI;
    [Header("Stat")]
    [SerializeField] private GameObject statUI;
    [Header("Setting")]
    [SerializeField] private GameObject settingUI;

    [Header("GameMenu")]
    [SerializeField] private GameObject gameStartUI;

    [Header("PlayerName")]
    [SerializeField] private Button playerNameButton;
    [SerializeField] private TextMeshProUGUI nameText;
    private bool isName = true;

    [Header("Main Button")]
    [SerializeField] private Button backToMainButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button statButton;
    [SerializeField] private Button gameStartButton;

    [Header("Game Start")]
    [SerializeField] private Button closeGameStartUIButton;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

    [Header("Lobby")]
    [SerializeField] private Button backToGameStart;
    [SerializeField] private GameObject JoinMenuUI;
    [SerializeField] private GameObject lobbyRoomUI;
    [SerializeField] private Button lobbyRoomCodeSubmit;
    [SerializeField] private InputField lobbyRoomCodeInputField;

    [Header("Equip Enhancement")]
    [SerializeField] private Button weaponEnhancement;
    [SerializeField] private Button weaponDiminishment;
    [SerializeField] private Button armorEnhancement;
    [SerializeField] private Button armorDiminishment;

    [Header("Stat Enhancement")]
    [SerializeField] private Button attackEnhancement;
    [SerializeField] private Button attackDiminishment;
    [SerializeField] private Button healthEnhancement;
    [SerializeField] private Button healthDiminishment;

    [Header("Map")]
    [SerializeField] private Image mapImage;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private TextMeshProUGUI mapName;
    [SerializeField] private MapSelectionData mapSelectionData;

    [Header("Room")]
    [SerializeField] private Transform scrollViewContentB;
    [SerializeField] private Transform scrollViewContentC;
    [SerializeField] private GameObject LobbyRoomListPrefab;
    [SerializeField] private GameObject LobbyPlayerNamePrefab;
    [SerializeField] private Button leaveRoomButton;

    private string selectedLobbyId;
    private int currentMapIndex = 0;
    private void OnEnable()
    {

        RegisterListeners();
    }

    private void OnDisable()
    {
        UnregisterListeners();
    }

    private void Start()
    {
        ChangePlayerNameToPlayerID();
    }

    private void RegisterListeners()
    {
        // GameStart
        AddListener(gameStartButton, GameStartEnter);

        // GameStartUI
        AddListener(closeGameStartUIButton, OnClickGameStartExit);
        AddListener(createRoomButton, OnCreateRoom);
        AddListener(joinRoomButton, OnJoinRoom);

        // Lobby
        AddListener(backToGameStart, BackToGameStartUI);
        AddListener(lobbyRoomCodeSubmit, RoomCodeSubmit);


        // Enhancement
        AddListener(equipButton, EquipEnhancementUI);
        AddListener(statButton, StatusEnhancementUI);

        // Back To Main
        AddListener(backToMainButton, BackToMainUI);

        // PlayerName
        AddListener(playerNameButton, ChangePlayerNameToPlayerID);

        // Equip Enhancement
        AddListener(weaponEnhancement, JewelUpGradeATK);
        AddListener(weaponDiminishment, JewelDownGradeATK);
        AddListener(armorEnhancement, JewelUpGradeHP);
        AddListener(armorDiminishment, JewelDownGradeHP);

        // Stat Enhancement
        AddListener(attackEnhancement, CoinUpGradeATK);
        AddListener(attackDiminishment, CoinDownGradeATK);
        AddListener(healthEnhancement, CoinUpGradeHP);
        AddListener(healthDiminishment, CoinDownGradeHP);

        // Map
        AddListener(leftButton, OnLeftButtonClicked);
        AddListener(rightButton, OnRightButtonClicked);

        // Room
        AddListener(leaveRoomButton, OnLeaveRoom);

        LobbyEvent.OnLobbyReady += OnLobbyReady;


        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;



    }

    private void UnregisterListeners()
    {
        // GameStart
        RemoveListener(gameStartButton, GameStartEnter);

        // GameStartUI
        RemoveListener(closeGameStartUIButton, OnClickGameStartExit);
        RemoveListener(createRoomButton, OnCreateRoom);
        RemoveListener(joinRoomButton, OnJoinRoom);

        // Lobby
        RemoveListener(backToGameStart, BackToGameStartUI);
        RemoveListener(lobbyRoomCodeSubmit, RoomCodeSubmit);


        // Enhancement
        RemoveListener(equipButton, EquipEnhancementUI);
        RemoveListener(statButton, StatusEnhancementUI);

        // Back To Main
        RemoveListener(backToMainButton, BackToMainUI);

        // PlayerName
        RemoveListener(playerNameButton, ChangePlayerNameToPlayerID);

        // Equip Enhancement
        RemoveListener(weaponEnhancement, JewelUpGradeATK);
        RemoveListener(weaponDiminishment, JewelDownGradeATK);
        RemoveListener(armorEnhancement, JewelUpGradeHP);
        RemoveListener(armorDiminishment, JewelDownGradeHP);

        // Stat Enhancement
        RemoveListener(attackEnhancement, CoinUpGradeATK);
        RemoveListener(attackDiminishment, CoinDownGradeATK);
        RemoveListener(healthEnhancement, CoinUpGradeHP);
        RemoveListener(armorDiminishment, CoinDownGradeHP);

        // Map

        RemoveListener(leftButton, OnLeftButtonClicked);
        RemoveListener(rightButton, OnRightButtonClicked);

        // Room
        RemoveListener(leaveRoomButton, OnLeaveRoom);

        LobbyEvent.OnLobbyReady -= OnLobbyReady;

        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;

    }

    #region Listener 함수
    private void AddListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
        {
            button.onClick.AddListener(action);
        }
        else
        {
            Debug.LogWarning("Button is not assigned.");
        }
    }

    private void RemoveListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
        {
            button.onClick.RemoveListener(action);
        }
    }
    #endregion

    #region MainUI
    //장비 강화
    private void EquipEnhancementUI()
    {
        mainLobbyUI.SetActive(false);
        equipEnhancementUI.SetActive(true);
        statUI.SetActive(true);
    }

    //스탯 강화
    private void StatusEnhancementUI()
    {
        mainLobbyUI.SetActive(false);
        statusEnhancementUI.SetActive(true);
        statUI.SetActive(true);
    }

    // 강화창 뒤로가기
    private void BackToMainUI()
    {
        equipEnhancementUI.SetActive(false);
        statusEnhancementUI.SetActive(false);
        statUI.SetActive(false);
        mainLobbyUI.SetActive(true);
    }



    // 게임시작 UI
    private void GameStartEnter()
    {
        gameStartUI.SetActive(true);
        settingUI.SetActive(false);
    }
    #endregion

    // 게임시작 UI 종료
    private void OnClickGameStartExit()
    {
        gameStartUI.SetActive(false);
        settingUI.SetActive(true);
    }

    private void BackToGameStartUI()
    {
        if (JoinMenuUI.activeSelf)
            JoinMenuUI.SetActive(false);
        if (lobbyRoomUI.activeSelf)
            lobbyRoomUI.SetActive(false);
        createRoomButton.gameObject.SetActive(true);
        joinRoomButton.gameObject.SetActive(true);
        backToGameStart.gameObject.SetActive(false);
    }

    #region 강화
    // 공격력을 보석으로 업그레이드
    public void JewelUpGradeATK()
    {
        if (UserData.Instance.Character.Gems > UserData.Instance.Character.AttackEnhancement)
        {
            UserData.Instance.Character.AttackPower++;
            UserData.Instance.Character.Gems -= UserData.Instance.Character.AttackEnhancement;
            UserData.Instance.Character.AttackEnhancement++;
            UserData.Instance.SavePlayerData();
            Debug.Log("Jewel upgraded ATK");
        }
        else
        {
            Debug.LogWarning("Not enough gems to upgrade ATK");
        }
    }

    // 공격력을 보석으로 다운그레이드
    public void JewelDownGradeATK()
    {
        if (UserData.Instance.Character.AttackPower > 0 && UserData.Instance.Character.AttackEnhancement > 0)
        {
            UserData.Instance.Character.AttackPower--;
            UserData.Instance.Character.Gems += (UserData.Instance.Character.AttackEnhancement - 1);
            UserData.Instance.Character.AttackEnhancement--;
            UserData.Instance.SavePlayerData();
            Debug.Log("Jewel downgraded ATK");
        }
        else
        {
            Debug.LogWarning("Cannot downgrade ATK");
        }
    }

    // 체력을 보석으로 업그레이드
    public void JewelUpGradeHP()
    {
        if (UserData.Instance.Character.Gems > UserData.Instance.Character.HealthEnhancement * 5)
        {
            UserData.Instance.Character.MaxHealth += 5;
            UserData.Instance.Character.Gems -= UserData.Instance.Character.HealthEnhancement * 5;
            UserData.Instance.Character.HealthEnhancement++;
            UserData.Instance.SavePlayerData();
            Debug.Log("Jewel upgraded HP");
        }
        else
        {
            Debug.LogWarning("Not enough gems to upgrade HP");
        }
    }

    // 체력을 보석으로 다운그레이드
    public void JewelDownGradeHP()
    {
        if (UserData.Instance.Character.MaxHealth > 0 && UserData.Instance.Character.HealthEnhancement > 0)
        {
            UserData.Instance.Character.MaxHealth -= 5;
            UserData.Instance.Character.Gems += (UserData.Instance.Character.HealthEnhancement - 1) * 5;
            UserData.Instance.Character.HealthEnhancement--;
            UserData.Instance.SavePlayerData();
            Debug.Log("Jewel downgraded HP");
        }
        else
        {
            Debug.LogWarning("Cannot downgrade HP");
        }
    }

    // 공격력을 코인으로 업그레이드
    public void CoinUpGradeATK()
    {
        if (UserData.Instance.Character.Coins > UserData.Instance.Character.AttackEnhancement * 5)
        {
            UserData.Instance.Character.AttackPower++;
            UserData.Instance.Character.Coins -= UserData.Instance.Character.AttackEnhancement * 5;
            UserData.Instance.Character.AttackEnhancement++;
            UserData.Instance.SavePlayerData();
            Debug.Log("Coin upgraded ATK");
        }
        else
        {
            Debug.LogWarning("Not enough coins to upgrade ATK");
        }
    }

    // 공격력을 코인으로 다운그레이드
    public void CoinDownGradeATK()
    {
        if (UserData.Instance.Character.AttackPower > 0 && UserData.Instance.Character.AttackEnhancement > 0)
        {
            UserData.Instance.Character.AttackPower--;
            UserData.Instance.Character.Coins += (UserData.Instance.Character.AttackEnhancement - 1) * 5;
            UserData.Instance.Character.AttackEnhancement--;
            UserData.Instance.SavePlayerData();
            Debug.Log("Coin downgraded ATK");
        }
        else
        {
            Debug.LogWarning("Cannot downgrade ATK");
        }
    }

    // 체력을 코인으로 업그레이드
    public void CoinUpGradeHP()
    {
        if (UserData.Instance.Character.Coins > UserData.Instance.Character.HealthEnhancement * 5)
        {
            UserData.Instance.Character.MaxHealth += 5;
            UserData.Instance.Character.Coins -= UserData.Instance.Character.HealthEnhancement * 5;
            UserData.Instance.Character.HealthEnhancement++;
            UserData.Instance.SavePlayerData();
            Debug.Log("Coin upgraded HP");
        }
        else
        {
            Debug.LogWarning("Not enough coins to upgrade HP");
        }
    }

    // 체력을 코인으로 다운그레이드
    public void CoinDownGradeHP()
    {
        if (UserData.Instance.Character.MaxHealth > 0 && UserData.Instance.Character.HealthEnhancement > 0)
        {
            UserData.Instance.Character.MaxHealth -= 5;
            UserData.Instance.Character.Coins += (UserData.Instance.Character.HealthEnhancement - 1) * 5;
            UserData.Instance.Character.HealthEnhancement--;
            UserData.Instance.SavePlayerData();
            Debug.Log("Coin downgraded HP");
        }
        else
        {
            Debug.LogWarning("Cannot downgrade HP");
        }
    }
    #endregion
    #region 플레이어 ID
    private void ChangePlayerNameToPlayerID()
    {
        isName = !isName;

        if (UserData.Instance != null && UserData.Instance.Character != null)
        {
            nameText.text = isName ? UserData.Instance.Character.PlayerName : UserData.Instance.Character.PlayerId;
        }
        else
        {
            nameText.text = "Unknown";
            Debug.LogError("Character data is not loaded in UserData.");
        }
    }
    #endregion

    // 방 참가
    private void OnCreateRoom()
    {

        createRoomButton.gameObject.SetActive(false);
        joinRoomButton.gameObject.SetActive(false);
        JoinMenuUI.SetActive(true);
        backToGameStart.gameObject.SetActive(true);

    }

    // 방 입장
    private void OnJoinRoom()
    {

        createRoomButton.gameObject.SetActive(false);
        joinRoomButton.gameObject.SetActive(false);
        lobbyRoomUI.SetActive(true);
        backToGameStart.gameObject.SetActive(true);
    }

    // 방 나가기
    private async void OnLeaveRoom()
    {

        backToGameStart.gameObject.SetActive(true);
        closeGameStartUIButton.gameObject.SetActive(true);

        // 참가한 로비 나가기
        Debug.Log("Leaving room...");

        // 사용자가 현재 방에 있는지 확인
        if (LobbyManager.Instance.lobby != null)
        {
            try
            {
                // 로비에서 플레이어 제거
                await LobbyManager.Instance.LeaveLobby();

                // UI 업데이트
                JoinMenuUI.SetActive(false);
                lobbyRoomUI.SetActive(false);
                leaveRoomButton.gameObject.SetActive(false);
                backToGameStart.gameObject.SetActive(true);
                closeGameStartUIButton.gameObject.SetActive(true);
                createRoomButton.gameObject.SetActive(true);
                joinRoomButton.gameObject.SetActive(true);

                Debug.Log("Room left successfully.");
                await RefreshLobbyList();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to leave room: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Not currently in any room.");
        }

    }

    #region 방 만들기
    // 방 만들기
    private async void RoomCodeSubmit()
    {
        Debug.Log("Creating room...");
        string sceneName = string.IsNullOrEmpty(lobbyRoomCodeInputField.text) ? "파티사냥 하실분" : lobbyRoomCodeInputField.text;

        Dictionary<string, string> lobbyData = new Dictionary<string, string>()
    {
        { "MapIndex", $"{currentMapIndex}" }, // Set a default MapIndex
        { "SceneName", "파티사냥 하실분" } // Set a default SceneName
    };

        bool success = await LobbyManager.Instance.CreateLobby(3, false, new Dictionary<string, string>(), lobbyData);
        if (success)
        {
            Debug.Log("Room created successfully.");
            UpdateUIAfterRoomCreation();
            await RefreshLobbyList();
        }
        else
        {
            Debug.LogError("Failed to create room.");
        }
    }

    private async void JoinRoomButtonClicked()
    {
        Debug.Log("Joining room...");
        await RefreshLobbyList();
    }
    private async Task RefreshLobbyList()
    {
        var lobbies = await LobbyManager.Instance.GetLobbies();

        foreach (Transform child in scrollViewContentB)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in scrollViewContentC)
        {
            Destroy(child.gameObject);
        }

        foreach (var lobby in lobbies)
        {
            GameObject lobbyItemB = Instantiate(LobbyRoomListPrefab, scrollViewContentB);
            GameObject lobbyItemC = Instantiate(LobbyPlayerNamePrefab, scrollViewContentC);

            var lobbyPlayerNameUI = lobbyItemC.GetComponent<LobbyPlayerListUI>();
            var lobbyRoomListUI = lobbyItemB.GetComponent<LobbyRoomListUI>();
            lobbyPlayerNameUI.Initialize();
            lobbyRoomListUI.Initialize(lobby, SelectLobby);
        }
    }

    private void UpdateUIAfterRoomCreation()
    {
        JoinMenuUI.SetActive(false);
        lobbyRoomUI.SetActive(true);
        backToGameStart.gameObject.SetActive(false);
        closeGameStartUIButton.gameObject.SetActive(false);
        leaveRoomButton.gameObject.SetActive(true);
    }


    private void SelectLobby(string lobbyId)
    {
        selectedLobbyId = lobbyId;
    }
    #endregion
    #region 맵
    private async void OnLeftButtonClicked()
    {
        if (currentMapIndex - 1 > 0)
        {
            currentMapIndex--;
        }
        else
        {
            currentMapIndex = 0;
        }

        UpdateMap();
        try
        {
            await GameLobbyManager.Instance.SetSelectedMap(currentMapIndex, mapSelectionData.Maps[currentMapIndex].SceneName);
        }
        catch (Exception ex)
        {
            Debug.Log($"맵 바꾸기 실패 : {ex.Message}");
        }
    }



    private async void OnRightButtonClicked()
    {
        if (currentMapIndex + 1 < mapSelectionData.Maps.Count - 1)
        {
            currentMapIndex++;
        }
        else
        {
            currentMapIndex = mapSelectionData.Maps.Count - 1;
        }

        UpdateMap();

        try
        {
            await GameLobbyManager.Instance.SetSelectedMap(currentMapIndex, mapSelectionData.Maps[currentMapIndex].SceneName);
        }
        catch (Exception ex)
        {
            Debug.Log($"맵 바꾸기 실패 : {ex.Message}");
        }
    }
    private void UpdateMap()
    {
        mapImage.sprite = mapSelectionData.Maps[currentMapIndex].MapImage;
        mapName.text = mapSelectionData.Maps[currentMapIndex].MapName;
    }

    private void OnLobbyUpdated(Lobby lobby)
    {
        currentMapIndex = GameLobbyManager.Instance.GetMapIndex();
        UpdateMap();
    }
    #endregion
}


