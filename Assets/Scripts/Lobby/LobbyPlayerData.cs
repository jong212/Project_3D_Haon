using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

public class LobbyPlayerData
{
    private string _id;
    private string _gamerTag;
    private bool _isReady;

    public string Id => _id;
    public string GamerTag => _gamerTag;

    public bool IsReady
    {
        get => _isReady;
        set => _isReady = value;
    }


    public void Initialize(string id, string gamerTag)
    {
        _id = id;
        _gamerTag = gamerTag;

    }

    public void Initialize(Dictionary<string, PlayerDataObject> playerData)
    {
        UpdateState(playerData);
    }

    public void UpdateState(Dictionary<string, PlayerDataObject> playerData)
    {
        if (playerData.ContainsKey("Id"))
        {
            _id = playerData["Id"].Value;
        }
        if (playerData.ContainsKey("GamerTag"))
        {
            _gamerTag = playerData["GamerTag"].Value;
        }
        if (playerData.ContainsKey("IsReady"))
        {
            _isReady = playerData["IsReady"].Value == "True";
        }
    }

    public Dictionary<string, string> Serialize()
    {
        return new Dictionary<string, string>()
        {

            { "Id", _id },
            { "GamerTag", _gamerTag },
            { "IsReady", _isReady.ToString() },
        };
    }

}
