using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
public class LobbyManager : Singleton<LobbyManager>
{

    public Lobby lobby;
    private Coroutine heartbeatCoroutine;
    private Coroutine refreshLobbyCoroutine;

    public string GetLobbyCode()
    {
        return lobby?.LobbyCode;
    }

    public async Task<bool> CreateLobby(int maxPlayers, bool isPrivate, Dictionary<string, string> playerData, Dictionary<string, string> lobbyData)
    {
        try
        {
            // 릴레이 서버 생성
            string joinCode = await RelayManager.Instance.CreateRelay(maxPlayers);
            if (string.IsNullOrEmpty(joinCode))
            {
                Debug.LogError("Failed to create relay.");
                return false;
            }

            // 로비 데이터에 릴레이 joinCode 추가
            lobbyData["RelayJoinCode"] = joinCode;

            // 로비 생성
            var options = new CreateLobbyOptions
            {
                Data = SerializeLobbyData(lobbyData),
                IsPrivate = isPrivate
            };

            lobby = await LobbyService.Instance.CreateLobbyAsync("MyLobby", maxPlayers, options);
            Debug.Log($"Lobby created with ID: {lobby.Id}");

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to create lobby: {ex.Message}");
            return false;
        }
    }
    public async Task<bool> JoinLobby(string lobbyId, Dictionary<string, string> playerData)
    {
        try
        {
            // 로비 참가
            var options = new JoinLobbyByIdOptions
            {
                Player = new Player(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(playerData))
            };

            lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options);
            Debug.Log($"Joined lobby with ID: {lobby.Id}");

            // 릴레이 joinCode 가져오기
            if (lobby.Data.TryGetValue("RelayJoinCode", out var relayJoinCodeData))
            {
                string relayJoinCode = relayJoinCodeData.Value;
                bool relaySuccess = await RelayManager.Instance.JoinRelay(relayJoinCode);
                if (!relaySuccess)
                {
                    Debug.LogError("Failed to join relay.");
                    return false;
                }
            }
            else
            {
                Debug.LogError("RelayJoinCode not found in lobby data.");
                return false;
            }

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to join lobby: {ex.Message}");
            return false;
        }
    }

    public async Task LeaveLobby()
    {
        if (lobby != null)
        {
            await LobbyService.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);
            lobby = null;
        }
    }

    public async Task<List<Lobby>> GetLobbies()
    {
        try
        {
            var queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            return queryResponse.Results;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get lobbies: {ex.Message}");
            return new List<Lobby>();
        }
    }
    //private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float interval)
    //{
    //    while (true)
    //    {
    //        var task = LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
    //        yield return new WaitUntil(() => task.IsCompleted);

    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError($"Failed to send heartbeat: {task.Exception}");
    //        }
    //        yield return new WaitForSeconds(interval);
    //    }
    //}

    //private IEnumerator RefreshLobbyCoroutine(string lobbyId, float interval)
    //{
    //    while (true)
    //    {
    //        var task = LobbyService.Instance.GetLobbyAsync(lobbyId);
    //        yield return new WaitUntil(() => task.IsCompleted);

    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError($"Failed to refresh lobby: {task.Exception}");
    //        }
    //        else
    //        {
    //            lobby = task.Result;
    //            LobbyEvents.OnLobbyUpdated?.Invoke(lobby);
    //        }
    //        yield return new WaitForSeconds(interval);
    //    }
    //}

    private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
    {
        Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
        foreach (var kvp in data)
        {
            playerData[kvp.Key] = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, kvp.Value);
        }
        return playerData;
    }

    private Dictionary<string, DataObject> SerializeLobbyData(Dictionary<string, string> data)
    {
        Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>();
        foreach (var kvp in data)
        {
            lobbyData[kvp.Key] = new DataObject(DataObject.VisibilityOptions.Public, kvp.Value);
        }
        return lobbyData;
    }

    public async Task<bool> StartGame(string sceneName)
    {
        try
        {
            Dictionary<string, DataObject> updatedData = new Dictionary<string, DataObject>(lobby.Data);

            if (updatedData.ContainsKey("GameStart"))
            {
                updatedData["GameStart"] = new DataObject(DataObject.VisibilityOptions.Public, "true");
            }
            else
            {
                updatedData.Add("GameStart", new DataObject(DataObject.VisibilityOptions.Public, "true"));
            }

            if (updatedData.ContainsKey("SceneName"))
            {
                updatedData["SceneName"] = new DataObject(DataObject.VisibilityOptions.Public, sceneName);
            }
            else
            {
                updatedData.Add("SceneName", new DataObject(DataObject.VisibilityOptions.Public, sceneName));
            }

            lobby = await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, new UpdateLobbyOptions { Data = updatedData });

            // 이벤트 발생
            LobbyEvents.RaiseOnLobbyUpdated(lobby);

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to start game: {ex.Message}");
            return false;
        }
    }

    public async Task CheckForGameStart()
    {
        while (true)
        {
            if (lobby != null)
            {
                try
                {
                    var updatedLobby = await LobbyService.Instance.GetLobbyAsync(lobby.Id);
                    LobbyEvents.RaiseOnLobbyUpdated(updatedLobby);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error checking for game start: {ex.Message}");
                }
            }
            await Task.Delay(3000); // 3초마다 확인
        }
    }


    public string GetHostId()
    {
        return "";
    }

    internal IEnumerable<object> GetPlayerData()
    {
        throw new NotImplementedException();
    }

    internal async Task<bool> UpdatePlayerData(string id, Dictionary<string, string> dictionary)
    {
        throw new NotImplementedException();
    }

    internal async Task<bool> UpdateLobbyData(Dictionary<string, string> dictionary)
    {
        throw new NotImplementedException();
    }

    internal async Task UpdatePlayerData(string id, Dictionary<string, string> dictionary, string allocationId, string connectionData)
    {
        throw new NotImplementedException();
    }
}
