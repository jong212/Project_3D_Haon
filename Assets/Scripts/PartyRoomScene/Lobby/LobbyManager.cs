using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : SceneSingleton<LobbyManager>
{
    public Lobby lobby;

    private Coroutine heartbeatCoroutine;
    private Coroutine refreshLobbyCoroutine;

    public static event Action<Lobby> OnLobbyUpdated;
    
    public async Task<Lobby> CreateLobby(string lobbyName, int maxPlayers, Dictionary<string, DataObject> lobbyData)
    {
        Dictionary<string, PlayerDataObject> playerDataObjects = new Dictionary<string, PlayerDataObject>();

        // 추가 데이터 객체 초기화
        lobbyData["GameStart"] = new DataObject(DataObject.VisibilityOptions.Member, "false");
        lobbyData["SceneName"] = new DataObject(DataObject.VisibilityOptions.Member, "");

        Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerDataObjects);

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Data = lobbyData,
            IsPrivate = false,
            Player = player
        };

        try
        {
            lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            Debug.Log($"Lobby created with ID: {lobby.Id}");

            StartHeartbeat();
            StartRefreshLobby();

            return lobby;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to create lobby: {ex.Message}");
            return null;
        }
    }

    public async Task<Lobby> CreateLobby(string lobbyName, int maxPlayers)
    {
        CreateLobbyOptions options = new CreateLobbyOptions
        {
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>
            {
                // 예: 초기 데이터를 설정하려면 여기에 추가
                // { "exampleKey", new DataObject(DataObject.VisibilityOptions.Member, "exampleValue") }
            }
        };

        try
        {
            lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            Debug.Log($"Lobby created with ID: {lobby.Id}");

            StartHeartbeat();
            StartRefreshLobby();

            return lobby;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to create lobby: {ex.Message}");
            return null;
        }
    }



    public async Task<List<Lobby>> GetLobbies()
    {
        try
        {
            var queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            return queryResponse.Results;
        }
        catch (RequestFailedException ex)
        {
           // Debug.LogError($"Failed to get lobbies: {ex.Message} (Error Code: {ex.ErrorCode})");

            // 특정 오류 코드 처리
            if (ex.ErrorCode == 401)
            {
                Debug.LogError("Unauthorized access - please check your authentication settings.");
            }
            return new List<Lobby>();
        }
        catch (Exception ex)
        {
            Debug.LogError($"An unexpected error occurred: {ex.Message}");
            return new List<Lobby>();
        }
    }

    public async Task<bool> JoinLobby(string lobbyId, Dictionary<string, PlayerDataObject> playerData)
    {
        var options = new JoinLobbyByIdOptions
        {
            Player = new Player(AuthenticationService.Instance.PlayerId, null, playerData)
        };

        try
        {
            lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options);
            Debug.Log($"Joined lobby with ID: {lobby.Id}");

            StartHeartbeat();
            StartRefreshLobby();

            if (!ValidateLobbyData(lobby))
            {
                Debug.LogError("Joined lobby data is invalid.");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to join lobby: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> JoinLobby(string lobbyId)
    {
        try
        {
            lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            Debug.Log($"Joined lobby with ID: {lobby.Id}");

            StartHeartbeat();
            StartRefreshLobby();

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to join lobby: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> LeaveLobby()
    {
        try
        {
            if (lobby != null)
            {
                await LobbyService.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);
                lobby = null;
                StopHeartbeat();
                StopRefreshLobby();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to leave lobby: {ex.Message}");
            return false;
        }
    }

    private void StartHeartbeat()
    {
        if (heartbeatCoroutine != null)
        {
            StopCoroutine(heartbeatCoroutine);
        }
        heartbeatCoroutine = StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15f));
    }

    private void StopHeartbeat()
    {
        if (heartbeatCoroutine != null)
        {
            StopCoroutine(heartbeatCoroutine);
            heartbeatCoroutine = null;
        }
    }

    private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        while (true)
        {
            try
            {
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to send heartbeat: {ex.Message}");
            }
            yield return new WaitForSecondsRealtime(waitTimeSeconds);
        }
    }

    private void StartRefreshLobby()
    {
        if (refreshLobbyCoroutine != null)
        {
            StopCoroutine(refreshLobbyCoroutine);
        }
        refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 5f));
    }

    private void StopRefreshLobby()
    {
        if (refreshLobbyCoroutine != null)
        {
            StopCoroutine(refreshLobbyCoroutine);
            refreshLobbyCoroutine = null;
        }
    }

    private IEnumerator RefreshLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(waitTimeSeconds);
            var refreshTask = RefreshLobbyAsync(lobbyId);
            yield return new WaitUntil(() => refreshTask.IsCompleted);

            if (refreshTask.IsFaulted)
            {
                Debug.LogError($"Failed to refresh lobby: {refreshTask.Exception}");
            }
        }
    }
    private async Task RefreshLobbyAsync(string lobbyId)
    {
        try
        {
            Lobby newLobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);

            if (newLobby != null && newLobby.Data != null && newLobby.LastUpdated > lobby.LastUpdated)
            {
                lobby = newLobby;
                OnLobbyUpdated?.Invoke(lobby);
            }
            else
            {
                Debug.LogError("Failed to refresh lobby: New lobby or its data is null.");
            }
        }
        catch (LobbyServiceException ex)
        {
            if (ex.ErrorCode == 16001) // 정수 코드로 비교
            {
                Debug.LogWarning("Lobby not found. It might have been deleted or expired.");
            }
            else
            {
                Debug.LogError($"Failed to refresh lobby: {ex.Message} (Error Code: {ex.ErrorCode})");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to refresh lobby: {ex.Message}");
        }
    }
    private bool ValidateLobbyData(Lobby lobby)
    {
        if (lobby == null)
        {
            Debug.LogError("Lobby is null.");
            return false;
        }

        if (lobby.Data == null)
        {
            Debug.LogError("Lobby data dictionary is null.");
            return false;
        }

        return true;
    }

    public async Task<bool> SetGameStartFlag(string sceneName)
    {
        try
        {
            var updatedData = new Dictionary<string, DataObject>
            {
                ["GameStart"] = new DataObject(lobby.Data["GameStart"].Visibility, "true"),
                ["SceneName"] = new DataObject(lobby.Data["SceneName"].Visibility, sceneName)
            };

            await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, new UpdateLobbyOptions { Data = updatedData });
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to set game start flag: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateLobbyData(Dictionary<string, DataObject> updatedData)
    {
        try
        {
            lobby = await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, new UpdateLobbyOptions { Data = updatedData });
            OnLobbyUpdated?.Invoke(lobby);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update lobby data: {ex.Message}");
            return false;
        }
    }


}