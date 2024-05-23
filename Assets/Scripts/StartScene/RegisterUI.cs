using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterUI : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Button registerButton;
    public Button loginButton;
    public Button cancelButton;
    public Text messageText;
    public CanvasGroup messageCanvasGroup; // 메시지의 페이드 인/아웃을 위해 추가

    private string apiUrl = "http://localhost:3000/api/register";

    void Start()
    {
        registerButton.onClick.AddListener(OnRegisterButtonClick);
        loginButton.onClick.AddListener(OnLoginButtonClick);
        cancelButton.onClick.AddListener(OnCancelButtonClick);

        messageCanvasGroup.alpha = 0; // 초기 상태에서는 메시지를 보이지 않게 설정
    }

    void OnRegisterButtonClick()
    {
        StartCoroutine(Register());
    }

    void OnLoginButtonClick()
    {
        // 로그인 로직 구현 (추가 필요)
        ShowMessage("Login clicked", 3f);
    }

    void OnCancelButtonClick()
    {
        // 현재 창을 닫는 로직 (예: 비활성화)
        gameObject.SetActive(false);
    }

    IEnumerator Register()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowMessage("All fields must be filled!", 3f);
            yield break;
        }

        PlayerData playerData = new PlayerData
        {
            Username = username,
            Password = password
        };

        string jsonData = JsonUtility.ToJson(playerData);

        UnityWebRequest www = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            ShowMessage(www.downloadHandler.text, 3f); // 서버에서 반환된 메시지를 표시
        }
        else
        {
            ShowMessage("Registration successful!", 3f);
            Debug.Log("Player registered successfully: " + www.downloadHandler.text);
            // 회원가입 성공 후 추가 로직 (예: 창 닫기)
        }
    }

    void ShowMessage(string message, float duration)
    {
        messageText.text = message;
        messageCanvasGroup.DOFade(1, 0.5f).OnComplete(() => {
            StartCoroutine(HideMessage(duration));
        });
    }

    IEnumerator HideMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageCanvasGroup.DOFade(0, 0.5f);
    }

    [System.Serializable]
    public class PlayerData
    {
        public string Username;
        public string Password;
    }
}
