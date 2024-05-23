using UnityEngine;
using TMPro;

public class ChatCell : MonoBehaviour
{
    public ChatType ChatType { get; private set; }
    private TextMeshProUGUI textMesh;
    private Color textColor;
    private string messageText;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void Setup(ChatType chatType, Color color, string message)
    {
        ChatType = chatType;
        textColor = color;
        messageText = message;
        textMesh.color = color;
        textMesh.text = message;
    }

    public Color GetColor()
    {
        return textColor;
    }

    public string GetMessage()
    {
        return messageText;
    }
}