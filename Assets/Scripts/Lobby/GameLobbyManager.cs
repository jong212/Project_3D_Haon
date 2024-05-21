using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLobbyManager : Singleton<GameLobbyManager>
{

    private List<LobbyPlayerData> lobbyPlayerDatas = new List<LobbyPlayerData>();
    private LobbyPlayerData localLobbyPlayerData;
    private LobbyData lobbyData;
    private int maxNumberOfPlayers = 3;
    private bool inGame = false;


    public bool IsHost => localLobbyPlayerData.Id == LobbyManager.Instance.GetHostId();

    private void OnEnable()
    {
        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;

    }

    private void OnDisable()
    {
        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
    }

    public async Task<bool> CreateLobby()
    {
        try
        {
            localLobbyPlayerData = new LobbyPlayerData();
            localLobbyPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "HostPlayer");
            lobbyData = new LobbyData();
            lobbyData.Initialize(0);

            bool succeeded = await LobbyManager.Instance.CreateLobby(maxNumberOfPlayers, true, localLobbyPlayerData.Serialize(), lobbyData.Serialize());
            return succeeded;
        }
        catch (Exception ex)
        {
            Debug.Log($"로비 생성 실패 : {ex.Message}");
            return false;
        }
    }

    public string GetLobbyCode()
    {
        return LobbyManager.Instance.GetLobbyCode();
    }

    public async Task<bool> JoinLobby(string code)
    {
        try
        {
            localLobbyPlayerData = new LobbyPlayerData();
            localLobbyPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");

            bool succeeded = await LobbyManager.Instance.JoinLobby(code, localLobbyPlayerData.Serialize());
            Debug.Log(succeeded ? "로비 입장 성공." : "로비 입장 실패.");

            return succeeded;
        }
        catch (Exception ex)
        {
            Debug.Log($"JoinLobby 중 예외 발생 : {ex.Message}");
            return false;
        }

    }

    // Client
    private async void OnLobbyUpdated(Lobby lobby)
    {
        try
        {
            var playerData = LobbyManager.Instance.GetPlayerData();
            lobbyPlayerDatas.Clear();

            int numberOfPlayerReady = 0;

            foreach (var data in playerData)
            {
                var lobbyPlayerData = new LobbyPlayerData();
                lobbyPlayerData.Initialize(data);

                if (lobbyPlayerData.IsReady)
                {
                    numberOfPlayerReady++;
                }

                if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
                {
                    localLobbyPlayerData = lobbyPlayerData;

                    var characterPointers = Resources.FindObjectsOfTypeAll<LobbyCharacterPointer>();
                    foreach (var pointer in characterPointers)
                    {
                        if (pointer.PlayerId == lobbyPlayerData.Id)
                        {
                            pointer.ActivateCharacterPointer();
                        }
                        else
                        {
                            pointer.DeActivateCharacterPointer();
                        }
                    }
                }

                lobbyPlayerDatas.Add(lobbyPlayerData);
            }

            lobbyData = new LobbyData();
            lobbyData.Initialize(lobby.Data);

            LobbyEvent.OnLobbyUpdated?.Invoke();

            if (numberOfPlayerReady == lobby.Players.Count)
            {
                LobbyEvent.OnLobbyReady?.Invoke();
            }

            if (lobbyData.RelayJoinCode != default && !inGame)
            {
                await JoinRelayServer(lobbyData.RelayJoinCode);
                SceneManager.LoadSceneAsync(lobbyData.SceneName);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"로비 업데이트 실패 : {ex.Message}");
        }
    }

    public List<LobbyPlayerData> GetPlayers()
    {
        
        return lobbyPlayerDatas;
    }

    public async Task<bool> SetPlayerReady()
    {
        localLobbyPlayerData.IsReady = true;
        return await LobbyManager.Instance.UpdatePlayerData(localLobbyPlayerData.Id, localLobbyPlayerData.Serialize());
    }

    public int GetMapIndex()
    {
        return lobbyData.mapIndex;
    }

    public async Task<bool> SetSelectedMap(int currentMapIndex, string sceneName)
    {
        try
        {
            Debug.Log($"{lobbyData.MapIndex} ,{currentMapIndex}");
            lobbyData.MapIndex = currentMapIndex;
            lobbyData.SceneName = sceneName;
            return await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());
        }
        catch (Exception ex)
        {
            Debug.LogError($"맵 선택 실패 : {ex.Message}");
            return false;
        }
    }

    public async Task StartGame()
    {
        try
        {
            string relayJoinCode = await RelayManager.Instance.CreateRelay(maxNumberOfPlayers);
            inGame = true;

            lobbyData.RelayJoinCode = relayJoinCode;
            await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());

            string allocationId = RelayManager.Instance.GetAllocationId();
            string connectionData = RelayManager.Instance.GetConnectionData();
            await LobbyManager.Instance.UpdatePlayerData(localLobbyPlayerData.Id, localLobbyPlayerData.Serialize(), allocationId, connectionData);

            SceneManager.LoadSceneAsync(lobbyData.SceneName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"게임 시작 실패 : {ex.Message}");
        }

    }

    private async Task<bool> JoinRelayServer(string relayJoinCode)
    {
        if (string.IsNullOrEmpty(relayJoinCode))
        {
            Debug.LogError("JoinRelayServer: joinCode is null or empty.");
            return false;
        }

        try
        {
            Debug.Log("JoinRelayServer: Attempting to join relay server with code: " + relayJoinCode);
            inGame = true;
            bool relayJoined = await RelayManager.Instance.JoinRelay(relayJoinCode);

            if (!relayJoined)
            {
                Debug.LogError("Failed to join the relay server.");
                return false;
            }

            Debug.Log("JoinRelayServer: Successfully joined the relay server.");

            string allocationId = RelayManager.Instance.GetAllocationId();
            string connectionData = RelayManager.Instance.GetConnectionData();
            Debug.Log($"JoinRelayServer: AllocationId: {allocationId}, ConnectionData: {connectionData}");
            await LobbyManager.Instance.UpdatePlayerData(localLobbyPlayerData.Id, localLobbyPlayerData.Serialize(), allocationId, connectionData);

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Relay Server 접속 실패 : {ex.Message}");
            return false;
        }
    }
}
