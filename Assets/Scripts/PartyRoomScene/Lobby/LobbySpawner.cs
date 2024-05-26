using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbySpawner : MonoBehaviour
{

    [SerializeField] private List<LobbyPlayer> _players;

    //private void OnEnable()
    //{
    ////    LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    //}

    //private void OnDisable()
    //{
    //    //LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;

    //}

    //private void OnLobbyUpdated(Lobby lobby)
    //{
    //    if (_players == null || _players.Count == 0)
    //    {
    //        Debug.LogError("Player list is not initialized or empty.");
    //        return;
    //    }

    //    var gameLobbyManager = GameLobbyManager.Instance;
    //    if (gameLobbyManager == null)
    //    {
    //        Debug.LogError("GameLobbyManager instance is null.");
    //        return;
    //    }

    //    List<LobbyPlayerData> playerDatas = gameLobbyManager.GetPlayers();
    //    if (playerDatas == null || playerDatas.Count == 0)
    //    {
    //        Debug.LogError("No player data available.");
    //        return;
    //    }

    //    for (int i = 0; i < playerDatas.Count; i++)
    //    {
    //        if (i >= _players.Count)
    //        {
    //            Debug.LogError($"Not enough player slots available. playerDatas.Count: {playerDatas.Count}, _players.Count: {_players.Count}, currentIndex: {i}");
    //            break;
    //        }

    //        LobbyPlayerData data = playerDatas[i];
    //        if (data != null)
    //        {
    //            _players[i].SetData(data);
    //        }
    //        else
    //        {
    //            Debug.LogError($"Player data at index {i} is null.");
    //        }
    //    }
    //}

}
