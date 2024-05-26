using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItemController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI currentPlayersText;
    [SerializeField] private Button joinButton;
    private string lobbyId;

    public void Initialize(string lobbyName, string currentPlayers, string id, System.Action<string> joinAction)
    {
        lobbyNameText.text = lobbyName;
        currentPlayersText.text = currentPlayers;
        lobbyId = id;
        joinButton.onClick.AddListener(() => joinAction(lobbyId));
    }
}
