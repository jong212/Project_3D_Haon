using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

// Server Data
public class LobbyData
{
    public int mapIndex;
    private string relayJoinCode;
    private string sceneName;


    public int MapIndex
    {
        get => mapIndex;
        set => mapIndex = value;
    }

    public string RelayJoinCode
    {
        get => relayJoinCode;
        set => relayJoinCode = value;
    }

    public string SceneName
    {
        get => sceneName;
        set => sceneName = value;
    }

    public void Initialize(int mapIndex)
    {
        this.mapIndex = mapIndex;
    }

    public void Initialize(Dictionary<string, DataObject> lobbyData)
    {
        UpdateState(lobbyData);
    }

    public void UpdateState(Dictionary<string, DataObject> lobbyData)
    {
        if (lobbyData.ContainsKey("MapIndex"))
        {
            mapIndex = int.Parse(lobbyData["MapIndex"].Value);
        }

        if (lobbyData.ContainsKey("RelayJoinCode"))
        {
            relayJoinCode = lobbyData["RelayJoinCode"].Value;
        }

        if (lobbyData.ContainsKey("SceneName"))
        {
            sceneName = lobbyData["SceneName"].Value;
        }
    }

    public Dictionary<string, string> Serialize()
    {
        return new Dictionary<string, string>()
        {
            { "MapIndex", mapIndex.ToString() },
            { "RelayJoinCode", relayJoinCode },
            { "SceneName", sceneName },
        };
    }

}
