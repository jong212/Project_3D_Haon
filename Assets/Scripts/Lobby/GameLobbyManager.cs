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
        localLobbyPlayerData = new LobbyPlayerData();
        localLobbyPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "HostPlayer");
        lobbyData = new LobbyData();
        lobbyData.Initalize(0);

        bool succeeded = await LobbyManager.Instance.CreateLobby(maxNumberOfPlayers, true, localLobbyPlayerData.Serialize(), lobbyData.Serialize());

        return succeeded;

    }

    public string GetLobbyCode()
    {
        return LobbyManager.Instance.GetLobbyCode();
    }

    public async Task<bool> JoinLobby(string code)
    {
        localLobbyPlayerData = new LobbyPlayerData();
        localLobbyPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");

        bool succeeded = await LobbyManager.Instance.JoinLobby(code, localLobbyPlayerData.Serialize());
        return succeeded;

    }

    // Client
    private async void OnLobbyUpdated(Lobby lobby)
    {
        List<Dictionary<string, PlayerDataObject>> playerData = LobbyManager.Instance.GetPlayerData();
        lobbyPlayerDatas.Clear();

        int numberOfPlayerReady = 0;

        foreach (Dictionary<string, PlayerDataObject> data in playerData)
        {
            LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
            lobbyPlayerData.Initialize(data);

            if (lobbyPlayerData.IsReady)
            {
                numberOfPlayerReady++;
            }

            if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
            {
                localLobbyPlayerData = lobbyPlayerData;
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
        lobbyData.MapIndex = currentMapIndex;
        lobbyData.SceneName = sceneName;
        return await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());
    }

    public async Task StartGame()
    {
        string relayRelayCode = await RelayManager.Instance.CreateRelay(maxNumberOfPlayers);
        inGame = true;

        lobbyData.RelayJoinCode = relayRelayCode;
        await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());

        string allocationId = RelayManager.Instance.GetAllocationId();
        string connectionData = RelayManager.Instance.GetConnectionData();
        await LobbyManager.Instance.UpdatePlayerData(localLobbyPlayerData.Id, localLobbyPlayerData.Serialize(), allocationId, connectionData);

        SceneManager.LoadSceneAsync(lobbyData.SceneName);


    }

    private async Task<bool> JoinRelayServer(string relayJoinCode)
    {
        inGame = true;
        await RelayManager.Instance.JoinRelay(relayJoinCode);

        string allocationId = RelayManager.Instance.GetAllocationId();
        string connectionData = RelayManager.Instance.GetConnectionData();
        await LobbyManager.Instance.UpdatePlayerData(localLobbyPlayerData.Id, localLobbyPlayerData.Serialize(), allocationId, connectionData);

        return true;
    }
}
