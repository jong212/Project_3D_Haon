using Unity.Services.Lobbies.Models;

public static class LobbyEvents
{
    public delegate void LobbyUpdate(Lobby lobby);

    public static LobbyUpdate OnLobbyUpdated;


}
