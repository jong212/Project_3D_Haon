using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbySpawner : MonoBehaviour
{

    [SerializeField] private List<LobbyPlayer> _player;

    private void OnEnable()
    {
        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    }

    private void OnDisable()
    {
        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;

    }

    private void OnLobbyUpdated(Lobby lobby)
    {
        List<LobbyPlayerData> playerDatas = GameLobbyManager.Instance.GetPlayers();

        for (int i = 0; i < playerDatas.Count; i++)
        {
            LobbyPlayerData data = playerDatas[i];
            _player[i].SetData(data);
        }
    }

}
