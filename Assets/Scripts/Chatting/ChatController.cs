using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public enum ChatType { Normal = 0, Party, Guild, Whisper, System, Count }

public class ChatController : NetworkBehaviour
{
    [SerializeField]
    private GameObject textChatPrefab;
    [SerializeField]
    private Transform parentContent;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TMP_InputField nameInputField; // 사용자 이름 입력 필드 추가
    [SerializeField]
    private Sprite[] spriteChatInputType;
    [SerializeField]
    private Image imageChatInputType;
    [SerializeField]
    private TextMeshProUGUI textInput;

    private ChatType currentInputType;
    private Color currentTextColor;
    private List<ChatCell> chatList;
    private ChatType currentViewType;
    private string lastChatData = "";
    private string lastWhisperID = "";

    private string userName = "Anonymous"; // 기본 사용자 이름 설정
    private string friendID = "Noname";

    private void Awake()
    {
        chatList = new List<ChatCell>();
        currentInputType = ChatType.Normal;
        currentTextColor = Color.white;
    }

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
        if (IsServer)
        {
            string systemMessage = $"Player {clientId} connected.";
            SendSystemMessage(systemMessage);
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (IsServer)
        {
            string systemMessage = $"Player {clientId} disconnected.";
            SendSystemMessage(systemMessage);
        }
    }

    private void SendSystemMessage(string message)
    {
        var chatMessage = new ChatMessage
        {
            SenderName = "System",
            Message = message,
            SenderId = NetworkManager.Singleton.LocalClientId
        };

        UpdateChatDisplayClientRpc(chatMessage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !inputField.isFocused)
        {
            inputField.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && inputField.isFocused)
        {
            SetCurrentInputType();
        }
    }

    public void OnEndEditEventMethod()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdateChat();
        }
    }

    public void UpdateChat()
    {
        if (inputField.text.Equals("")) return;

        if (IsClient)
        {
            userName = nameInputField.text;
            SendChatMessageServerRpc(userName, inputField.text);
            inputField.text = "";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendChatMessageServerRpc(string senderName, string message, ServerRpcParams serverRpcParams = default)
    {
        var chatMessage = new ChatMessage
        {
            SenderName = senderName,
            Message = message,
            SenderId = serverRpcParams.Receive.SenderClientId
        };

        UpdateChatDisplayClientRpc(chatMessage);
    }

    [ClientRpc]
    private void UpdateChatDisplayClientRpc(ChatMessage chatMessage)
    {
        AddMessageToChat(chatMessage);
        UpdateChatDisplay();
    }

    private void AddMessageToChat(ChatMessage chatMessage)
    {
        string messageText = $"{chatMessage.SenderName}: {chatMessage.Message}";
        PrintChatData(chatMessage.SenderName == "System" ? ChatType.System : currentInputType, chatMessage.SenderName == "System" ? Color.yellow : currentTextColor, messageText);
    }

    private void PrintChatData(ChatType type, Color color, string text)
    {
        GameObject clone = Instantiate(textChatPrefab, parentContent);
        ChatCell cell = clone.GetComponent<ChatCell>();
        cell.Setup(type, color, text);
        chatList.Add(cell);
    }

    private void UpdateChatDisplay()
    {
        foreach (Transform child in parentContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var chatCell in chatList)
        {
            GameObject clone = Instantiate(textChatPrefab, parentContent);
            ChatCell cell = clone.GetComponent<ChatCell>();
            cell.Setup(chatCell.ChatType, chatCell.GetColor(), chatCell.GetMessage());
        }
    }

    public void SetCurrentInputType()
    {
        currentInputType = (int)currentInputType < (int)ChatType.Count - 3 ? currentInputType + 1 : 0;
        imageChatInputType.sprite = spriteChatInputType[(int)currentInputType];
        currentTextColor = ChatTypeToColor(currentInputType);
        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor;
    }

    public void SetCurrentViewType(int newType)
    {
        currentViewType = (ChatType)newType;

        foreach (var chatCell in chatList)
        {
            chatCell.gameObject.SetActive(currentViewType == ChatType.Normal || chatCell.ChatType == currentViewType);
        }
    }

    private Color ChatTypeToColor(ChatType type)
    {
        Color[] colors = new Color[(int)ChatType.Count]
        {
            Color.white, Color.blue, Color.green, Color.magenta, Color.yellow
        };

        return colors[(int)type];
    }
}
public struct ChatMessage : INetworkSerializable
{
    public ulong SenderId;
    public string SenderName;
    public string Message;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref SenderId);
        serializer.SerializeValue(ref SenderName);
        serializer.SerializeValue(ref Message);
    }
}

