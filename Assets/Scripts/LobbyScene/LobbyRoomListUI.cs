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

    private string selectedLobbyId;

    private void OnEnable()
    {
        joinButton.onClick.AddListener(JoinSelectedRoom);
    }

    private void OnDisable()
    {
        joinButton.onClick.RemoveListener(JoinSelectedRoom);
    }

    public void Initialize(Lobby lobby, System.Action<string> onJoinRoom)
    {
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

    private async void JoinSelectedRoom()
    {
        if (!string.IsNullOrEmpty(selectedLobbyId))
        {
            Debug.Log($"Joining room with ID: {selectedLobbyId}");
            bool success = await LobbyManager.Instance.JoinLobby(selectedLobbyId, new Dictionary<string, string>());
            if (success)
            {
                Debug.Log("Room joined successfully.");
            }
            else
            {
                Debug.LogError("Failed to join room.");
            }
        }
        else
        {
            Debug.LogWarning("No room selected to join.");
        }
    }

    private void OnJoinButtonClicked()
    {
        onJoinRoomCallback?.Invoke(lobbyId);
    }
}
