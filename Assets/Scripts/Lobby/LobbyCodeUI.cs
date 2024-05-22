using System;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCodeUI : MonoBehaviour
{
    [Header("Lobby")]
    [SerializeField] private TextMeshProUGUI lobbyCode;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button startButton;

    [Header("Map")]
    [SerializeField] private Image mapImage;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private TextMeshProUGUI mapName;
    [SerializeField] private MapSelectionData mapSelectionData;


    private int currentMapIndex = 0;

    private void OnEnable()
    {
        readyButton.onClick.AddListener(OnReadyPressed);

        if (GameLobbyManager.Instance.IsHost)
        {
            leftButton.onClick.AddListener(OnLeftButtonClicked);
            rightButton.onClick.AddListener(OnRightButtonClicked);
            startButton.onClick.AddListener(OnStartButtonClicked);
            LobbyEvent.OnLobbyReady += OnLobbyReady;
        }

        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;

    }



    private void OnDisable()
    {
        readyButton.onClick.RemoveListener(OnReadyPressed);

        if (GameLobbyManager.Instance.IsHost)
        {
            leftButton.onClick.RemoveListener(OnLeftButtonClicked);
            rightButton.onClick.RemoveListener(OnRightButtonClicked);
            startButton.onClick.RemoveListener(OnStartButtonClicked);
            LobbyEvent.OnLobbyReady -= OnLobbyReady;
        }

        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
    }

    async void Start()
    {
        lobbyCode.text = $"Lobby Code : {GameLobbyManager.Instance.GetLobbyCode()}";

        if (!GameLobbyManager.Instance.IsHost)
        {
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
        }
        else
        {
            try
            {
                await GameLobbyManager.Instance.SetSelectedMap(currentMapIndex, mapSelectionData.Maps[currentMapIndex].SceneName);
            }
            catch (Exception ex)
            {
                Debug.Log($"선택 맵 초기화에 실패 : {ex.Message}");
            }
        }

    }


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

    private async void OnReadyPressed()
    {
        try
        {
            bool succeed = await GameLobbyManager.Instance.SetPlayerReady();

            if (succeed)
            {
                readyButton.gameObject.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"준비 실패 : {ex.Message}");
        }

    }

    private void OnLobbyReady()
    {
        startButton.gameObject.SetActive(true);
    }

    private async void OnStartButtonClicked()
    {
        try
        {
            await GameLobbyManager.Instance.StartGame();
        }
        catch (Exception ex)
        {
            Debug.LogError($"시작 실패 : {ex.Message}");
        }
    }
}
