using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class NetworkStatusHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statusText; // 연결 상태를 표시할 Text UI

    private void Start()
    {
        // 네트워크 이벤트 콜백 등록
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy()
    {
        // 네트워크 이벤트 콜백 해제
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            // 클라이언트가 서버에 연결됨
            Debug.Log("Connected to server");
            UpdateStatusText("Connected to server");
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            // 클라이언트가 서버에서 연결 해제됨
            Debug.Log("Disconnected from server");
            UpdateStatusText("Disconnected from server");
        }
    }

    private void UpdateStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}