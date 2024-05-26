using Unity.Services.Lobbies.Models;

public static class LobbyEvents
{
    public delegate void LobbyUpdate(Lobby lobby);
    public static event LobbyUpdate OnLobbyUpdated;

    public static void RaiseOnLobbyUpdated(Lobby lobby)
    {
        OnLobbyUpdated?.Invoke(lobby);
    }
}