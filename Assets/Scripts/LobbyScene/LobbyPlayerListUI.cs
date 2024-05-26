using TMPro;
using UnityEngine;

public class LobbyPlayerListUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    public void Initialize()
    {
        if (playerName == null)
        {
            Debug.LogError("playerName is not assigned.");
            return;
        }

        Debug.Log("Initializing LobbyPlayerListUI...");


        if (UserData.Instance != null && UserData.Instance.Character != null)
        {
            playerName.text = UserData.Instance.Character.PlayerName;
        }
        else
        {
            playerName.text = "Unknown";
            Debug.LogError("Character data is not loaded in UserData.");
        }

    }
}
