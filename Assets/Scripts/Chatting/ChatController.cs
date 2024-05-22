using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ChatType { Normal = 0, Party,Guild, Whisper,System,Count }
public class ChatController : MonoBehaviour
{
    [SerializeField]
    private GameObject textChatPrefab;
    [SerializeField]
    private Transform parentContent;
    [SerializeField]
    private TMP_InputField inputField;

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

    private string ID = "Good I D er";
    private string friendID = "Noname";


    private void Awake()
    {
        chatList = new List<ChatCell>();

        currentInputType = ChatType.Normal;
        currentTextColor = Color.white;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
        {
            inputField.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && inputField.isFocused == true)
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
        //InputField가 비어있으면 종료
        if (inputField.text.Equals("")) return;

        ////대화내용 출력을위해 Text UI생성(textChatPrefab복제해서parentContent자식으로 배치)
        //GameObject clone = Instantiate(textChatPrefab, parentContent);
        //ChatCell cell = clone.GetComponent<ChatCell>();

        ////clone.GetComponent<TextMeshProUGUI>().text =$"{ID} : {inputField.text}";
        //cell.Setup(currentInputType, currentTextColor, $"{ID} : {inputField.text}");
        //inputField.text = "";

        //chatList.Add(cell);

        UpdateChatWithCommand(inputField.text);
    }

    private Color ChatTypeToColor(ChatType type)
    {
        Color[] colors = new Color[(int)ChatType.Count]
        {
            Color.white,Color.blue,Color.green,Color.magenta,Color.yellow
        };

        return colors[(int)type];
    }

    public void SetCurrentInputType()
    {
        currentInputType = (int)currentInputType < (int)ChatType.Count - 3 ? currentInputType + 1 : 0;
        imageChatInputType.sprite = spriteChatInputType[(int)currentInputType];
        currentTextColor = ChatTypeToColor(currentInputType);
        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor;
    }

    public void SetCurrentViewType(int newtype)
    {
        currentViewType =(ChatType)newtype;

        if(currentViewType == ChatType.Normal)
        {
            for(int i = 0;i<chatList.Count;++i) 
            {
                chatList[i].gameObject.SetActive(true);
            }
        }

        else
        {
            for(int i = 0; i<chatList.Count;++i)
            {
                chatList[i].gameObject.SetActive(chatList[i].ChatType == currentViewType);
            }
        }
    }

    private void PrintChatData(ChatType type,Color color, string text)
    {
        Debug.Log("대화출력");
        //대화내용 출력을위해 Text UI생성(textChatPrefab복제해서parentContent자식으로 배치)
        GameObject clone = Instantiate(textChatPrefab, parentContent);
        ChatCell cell = clone.GetComponent<ChatCell>();

        //clone.GetComponent<TextMeshProUGUI>().text =$"{ID} : {inputField.text}";
        cell.Setup(type, color, $"{ID} : {text}");
        inputField.text = "";

        chatList.Add(cell);
    }

    public void UpdateChatWithCommand(string chat)
    {
        if(!chat.StartsWith('/'))
        {
            
            lastChatData = chat;
            PrintChatData(currentInputType, currentTextColor, lastChatData);
            return;
        }

        if(chat.StartsWith("/re")) 
        {
           
            if (lastChatData.Equals(""))
            {
                
                inputField.text = "";
                return;
            }
            UpdateChatWithCommand(lastChatData);
        }

        else if( chat.StartsWith("/w"))
        {
            
            lastChatData = chat;

            string[] whisper = chat.Split(' ', 3);

            if (whisper[1] == friendID)
            {
                
                lastWhisperID = whisper[1];

                PrintChatData(ChatType.Whisper, ChatTypeToColor(ChatType.Whisper), $"[to {whisper[1]}] {whisper[2]}");
            }
            else
            {
                
                PrintChatData(ChatType.System, ChatTypeToColor(ChatType.System), $"Do not Find [{whisper[1]}]");
            }
        }
        else if(chat.StartsWith("/r "))
        {
            if(lastWhisperID.Equals(""))
            {
                inputField.text = "";
                return;
            }

            lastChatData = chat;

            string[] whisper = chat.Split(' ', 2);

            PrintChatData(ChatType.Whisper, ChatTypeToColor(ChatType.Whisper), $"[to {lastWhisperID}] {whisper[1]}");
        }
    }
}
