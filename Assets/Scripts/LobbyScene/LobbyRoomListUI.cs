using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomListUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mapNameText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private Button joinButton;


    private string _lobbyId;
    private System.Action<string> onJoinRoomCallback;

    private const string DefaultTitle = "파티사냥 가실분";

    private void Awake()
    {
        if (mapNameText == null || titleText == null || playerCountText == null || joinButton == null)
        {
            Debug.LogError("One or more UI components are not assigned.");
            enabled = false;  // Disable this component to prevent further errors.
            return;
        }

        if (MapSelectionData.Instance == null)
        {
            Debug.LogError("MapSelectionData instance is null. Ensure the MapSelectionData asset is placed in the Resources folder.");
            enabled = false;  // Disable this component to prevent further errors.
            return;
        }
    }

    public async Task<bool> Initialize(Lobby lobby, System.Action<string> onJoinRoom, string hiddenTitleText)
    {
        if (!ValidateLobbyData(lobby))
        {
            return false;
        }

        if (lobby.Data.TryGetValue("MapIndex", out var mapIndexValue) && int.TryParse(mapIndexValue.Value, out var mapIndex))
        {
            var mapInfo = MapSelectionData.Instance.Maps[mapIndex];
            mapNameText.text = mapInfo.MapName;
        }
        else
        {
            mapNameText.text = "Unknown Map";
        }

        titleText.text = string.IsNullOrEmpty(hiddenTitleText) ? DefaultTitle : hiddenTitleText;

        playerCountText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        _lobbyId = lobby.Id;
        onJoinRoomCallback = onJoinRoom;

        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(OnJoinButtonClicked);

        return true;
    }

    private bool ValidateLobbyData(Lobby lobby)
    {
        if (lobby == null)
        {
            Debug.LogError("Lobby data is null.");
            return false;
        }

        if (lobby.Data == null)
        {
            Debug.LogError("Lobby data dictionary is null.");
            return false;
        }

        return true;
    }

    private void UpdateUI(Lobby lobby)
    {
        mapNameText.text = GetMapName(lobby);
        titleText.text = GetSceneName(lobby);
        playerCountText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
    }

    private string GetMapName(Lobby lobby)
    {
        if (lobby.Data.ContainsKey("MapIndex"))
        {
            if (int.TryParse(lobby.Data["MapIndex"].Value, out int mapIndex))
            {
                if (MapSelectionData.Instance.Maps != null && mapIndex >= 0 && mapIndex < MapSelectionData.Instance.Maps.Count)
                {
                    MapInfo mapInfo = MapSelectionData.Instance.Maps[mapIndex];
                    return mapInfo.MapName;
                }
            }
        }
        return "Unknown Map";
    }

    private string GetSceneName(Lobby lobby)
    {
        return lobby.Data.ContainsKey("SceneName") ?
            (string.IsNullOrEmpty(lobby.Data["SceneName"].Value) ? DefaultTitle : lobby.Data["SceneName"].Value) :
            DefaultTitle;
    }

    private void OnJoinButtonClicked()
    {
        onJoinRoomCallback?.Invoke(_lobbyId);
    }
}