using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;

public class GameLobbyManager : Singleton<GameLobbyManager>
{

    private List<LobbyPlayerData> lobbyPlayerDatas = new List<LobbyPlayerData>();
    private LobbyPlayerData localLobbyPlayerData;
    private LobbyData lobbyData;

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
        lobbyData.MapIndex = 0;
        lobbyData.Initalize(0);

        bool succeeded = await LobbyManager.Instance.CreateLobby(4, true, localLobbyPlayerData.Serialize(), lobbyData.Serialize());

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


    private void OnLobbyUpdated(Lobby lobby)
    {
        List<Dictionary<string, PlayerDataObject>> playerData = LobbyManager.Instance.GetPlayerData();
        lobbyPlayerDatas.Clear();

        foreach (Dictionary<string, PlayerDataObject> data in playerData)
        {
            LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
            lobbyPlayerData.Initialize(data);

            if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
            {
                localLobbyPlayerData = lobbyPlayerData;
            }

            lobbyPlayerDatas.Add(lobbyPlayerData);
        }

        lobbyData = new LobbyData();
        lobbyData.Initialize(lobby.Data);

        LobbyEvent.OnLobbyUpdated?.Invoke();
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

    internal int GetMapIndex()
    {
        return lobbyData.mapIndex;
    }

    internal async Task<bool> SelectedMap(int currentMapIndex)
    {
        lobbyData.MapIndex = currentMapIndex;
        return await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());
    }
}
