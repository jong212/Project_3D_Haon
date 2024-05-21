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

    public async Task<bool> CreateLobby(int maxPlayer, bool isPrivate, Dictionary<string, string> data, Dictionary<string, string> lobbyData)
    {
        Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
        Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);

        CreateLobbyOptions options = new CreateLobbyOptions()
        {
            Data = SerialzeLobbyData(lobbyData),
            IsPrivate = isPrivate,
            Player = player,

        };

        try
        {
            lobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", maxPlayer, options);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"로비 생성 실패 : {ex.Message}");
            return false;
        }

        Debug.Log($"Lobby create with lobby ID : {lobby.Id}");

        heartbeatCoroutine = StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 6f));
        refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));

        return true;
    }



    private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        while (true)
        {
            try
            {
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"heartbeat 전송 실패 : {ex.Message}");
            }
            yield return new WaitForSecondsRealtime(waitTimeSeconds);
        }
    }

    private IEnumerator RefreshLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        while (true)
        {
            var refreshTask = RefreshLobbyAsync(lobbyId);

            yield return new WaitUntil(() => refreshTask.IsCompleted);

            if (refreshTask.IsFaulted)
            {
                Debug.Log($"로비 새로고침 실패 : {refreshTask.Exception}");
            }

            yield return new WaitForSecondsRealtime(waitTimeSeconds);
        }
    }

    private async Task RefreshLobbyAsync(string lobbyId)
    {
        try
        {
            Lobby newLobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);

            if (newLobby.LastUpdated > lobby.LastUpdated)
            {
                lobby = newLobby;
                LobbyEvents.OnLobbyUpdated?.Invoke(lobby);
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log($"로비 새로고침 실패 : {ex.Message}");
        }
    }



    private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
    {
        Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
        foreach (var (key, value) in data)
        {
            playerData.Add(key, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, value));
        }

        return playerData;
    }

    private Dictionary<string, DataObject> SerialzeLobbyData(Dictionary<string, string> data)
    {
        Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>();
        foreach (var (key, value) in data)
        {
            lobbyData.Add(key, new DataObject(DataObject.VisibilityOptions.Member, value));
        }

        return lobbyData;
    }

    public void OnApplicationQuit()
    {
        if (lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            LobbyService.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);
            LobbyService.Instance.DeleteLobbyAsync(lobby.Id);

        }
    }

    public async Task<bool> JoinLobby(string code, Dictionary<string, string> playerData)
    {
        JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
        {
            Player = new Player(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(playerData))
        };

        try
        {
            lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"로비 참가 실패 : {ex.Message}");
            return false;
        }

        refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 5f)); // Interval increased to 5 seconds
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

    public async Task<bool> UpdatePlayerData(string playerId, Dictionary<string, string> data, string allocationId = default, string connectionData = default)
    {
        Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);

        UpdatePlayerOptions options = new UpdatePlayerOptions()
        {
            Data = playerData,
            AllocationId = allocationId,
            ConnectionInfo = connectionData,
        };

        try
        {
            await LobbyService.Instance.UpdatePlayerAsync(lobby.Id, playerId, options);

        }
        catch (System.Exception ex)
        {
            Debug.LogError($"플레이어 데이터 업데이트 실패 : {ex.Message}");
            return false;
        }

        LobbyEvents.OnLobbyUpdated(lobby);

        return true;
    }

    public async Task<bool> UpdateLobbyData(Dictionary<string, string> data)
    {
        Dictionary<string, DataObject> lobbyData = SerialzeLobbyData(data);

        UpdateLobbyOptions options = new UpdateLobbyOptions()
        {
            Data = lobbyData
        };

        try
        {
            lobby = await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, options);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"로비 데이터 업데이트 실패 : {ex.Message}");
            return false;

        }

        LobbyEvents.OnLobbyUpdated(lobby);

        return true;

    }

    public string GetHostId()
    {
        return lobby.HostId;
    }
}
