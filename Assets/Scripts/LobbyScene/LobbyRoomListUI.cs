using System.Collections;
using System.Collections.Generic;
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

    private string lobbyId;
    private System.Action<string> onJoinRoomCallback;

    public void Initialize(Lobby lobby, System.Action<string> onJoinRoom)
    {
        if (mapNameText == null || titleText == null || playerCountText == null || joinButton == null)
        {
            Debug.LogError("UI components not assigned.");
            return;
        }

        if (MapSelectionData.Instance == null)
        {
            Debug.LogError("MapSelectionData instance is null. Ensure the MapSelectionData asset is placed in the Resources folder.");
            return;
        }

        if (lobby.Data.ContainsKey("MapIndex"))
        {
            int mapIndex = int.Parse(lobby.Data["MapIndex"].Value);
            MapInfo mapInfo = MapSelectionData.Instance.Maps[mapIndex];
            mapNameText.text = mapInfo.MapName;
        }
        else
        {
            mapNameText.text = "Unknown Map";
        }

        if (lobby.Data.ContainsKey("SceneName"))
        {
            titleText.text = lobby.Data["SceneName"].Value;
        }
        else
        {
            titleText.text = "Unknown Title";
        }

        playerCountText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        lobbyId = lobby.Id;
        onJoinRoomCallback = onJoinRoom;

        joinButton.onClick.AddListener(OnJoinButtonClicked);
    }

    private void OnJoinButtonClicked()
    {
        onJoinRoomCallback?.Invoke(lobbyId);
    }
}
