using TMPro;
using UnityEngine;

public class LobbyCodeUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI lobbyCode;


    void Start()
    {
        lobbyCode.text = $"Lobby Code : {GameLobbyManager.Instance.GetLobbyCode()}";
    }


    void Update()
    {

    }
}
