using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

// Server Data
public class LobbyData
{
    public int mapIndex;
    private string relayJoinCode = string.Empty;
    private string sceneName = string.Empty;


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
        if (lobbyData.TryGetValue("MapIndex", out DataObject mapIndexData))
        {
            if (int.TryParse(mapIndexData.Value, out int parsedMapIndex))
            {
                mapIndex = parsedMapIndex;
            }
        }

        if (lobbyData.TryGetValue("RelayJoinCode", out DataObject relayJoinCodeData))
        {
            relayJoinCode = relayJoinCodeData.Value ?? string.Empty;
        }

        if (lobbyData.TryGetValue("SceneName", out DataObject sceneNameData))
        {
            sceneName = sceneNameData.Value ?? string.Empty;
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
