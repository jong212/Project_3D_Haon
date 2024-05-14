using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{

    private Lobby lobby;
    private Coroutine heartbeatCoroutine;
    private Coroutine refreshLobbyCoroutine;

    public string GetLobbyCode()
    {
        return lobby?.LobbyCode;
    }

    public async Task<bool> CreateLobby(int maxPlayer, bool isPrivate, Dictionary<string, string> data)
    {
        Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
        Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);

        CreateLobbyOptions options = new CreateLobbyOptions()
        {
            IsPrivate = isPrivate,
            Player = player,

        };

        try
        {
            lobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", maxPlayer, options);
        }
        catch (System.Exception)
        {
            return false;
        }

        Debug.Log($"Lobby create with lobby ID : {lobby.Id}");

        heartbeatCoroutine = StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 6f));
        refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));

        return true;
    }

    private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSecond)
    {
        while (true)
        {
            Debug.Log("Heartbeat");
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return new WaitForSecondsRealtime(waitTimeSecond);
        }
    }

    private IEnumerator RefreshLobbyCoroutine(string lobbyId, float waitTimeSecond)
    {
        while (true)
        {
            Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(lobbyId);
            yield return new WaitUntil(() => task.IsCompleted);
            Lobby newLobby = task.Result;
            if (newLobby.LastUpdated > lobby.LastUpdated)
            {
                lobby = newLobby;
                LobbyEvents.OnLobbyUpdated?.Invoke(lobby);
            }
            yield return new WaitForSecondsRealtime(waitTimeSecond);
        }
    }

    private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
    {
        Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
        foreach (var (key, value) in data)
        {
            playerData.Add(key, new PlayerDataObject(
                visibility: PlayerDataObject.VisibilityOptions.Member,
                value: value
                ));
        }

        return playerData;
    }

    public void OnApplicationQuit()
    {
        if (lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
        }
    }

    public async Task<bool> JoinLobby(string code, Dictionary<string, string> playerData)
    {
        JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
        Player player = new Player(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(playerData));
        options.Player = player;

        try
        {
            lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
        }
        catch (System.Exception)
        {

            return false;
        }
        refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));

        return true;
    }

    public List<Dictionary<string, PlayerDataObject>> GetPlayerData()
    {
        List<Dictionary<string, PlayerDataObject>> data = new List<Dictionary<string, PlayerDataObject>>();

        foreach (Player player in lobby.Players)
        {
            data.Add(player.Data);
        }

        return data;

    }
}
