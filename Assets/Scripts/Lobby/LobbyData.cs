using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

public class LobbyData
{
    public int mapIndex;

    public int MapIndex
    {
        get => mapIndex;
        set => mapIndex = value;
    }

    public void Initalize(int mapIndex)
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
    }

    public Dictionary<string, string> Serialize()
    {
        return new Dictionary<string, string>()
        {
            { "MapIndex", mapIndex.ToString() }
        };
    }

}
