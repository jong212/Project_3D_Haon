using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyPlayerListUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    public void Initialize(string playerName)
    {
        if (playerNameText == null)
        {
            Debug.LogError("playerNameText is not assigned.");
            return;
        }

        playerNameText.text = playerName;
    }
}