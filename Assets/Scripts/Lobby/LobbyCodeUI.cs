using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCodeUI : MonoBehaviour
{
    [Header("Lobby")]
    [SerializeField] private TextMeshProUGUI lobbyCode;
    [SerializeField] private Button readyButton;

    [Header("Map")]
    [SerializeField] private Image mapImage;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private TextMeshProUGUI mapName;
    [SerializeField] private MapSelectionData mapSelectionData;


    private int currentMapIndex = 0;

    private void OnEnable()
    {

        if (GameLobbyManager.Instance.IsHost)
        {
            readyButton.onClick.AddListener(OnReadyPressed);
            leftButton.onClick.AddListener(OnLeftButtonClicked);
            rightButton.onClick.AddListener(OnRightButtonClicked);
        }

        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;

    }



    private void OnDisable()
    {
        if (GameLobbyManager.Instance.IsHost)
        {
            readyButton.onClick.RemoveAllListeners();
            leftButton.onClick.RemoveAllListeners();
            rightButton.onClick.RemoveAllListeners();
        }

        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
    }

    void Start()
    {
        lobbyCode.text = $"Lobby Code : {GameLobbyManager.Instance.GetLobbyCode()}";

        if (!GameLobbyManager.Instance.IsHost)
        {
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
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
        await GameLobbyManager.Instance.SelectedMap(currentMapIndex);
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

        await GameLobbyManager.Instance.SelectedMap(currentMapIndex);
    }
    private void UpdateMap()
    {
        mapImage.color = mapSelectionData.Maps[currentMapIndex].MapThumbnail;
        mapName.text = mapSelectionData.Maps[currentMapIndex].MapName;
    }

    private void OnLobbyUpdated(Lobby lobby)
    {
        currentMapIndex = GameLobbyManager.Instance.GetMapIndex();
        UpdateMap();
    }

    private async void OnReadyPressed()
    {
        bool succeed = await GameLobbyManager.Instance.SetPlayerReady();

        if (succeed)
        {
            readyButton.gameObject.SetActive(false);
        }

    }

}
