using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject mainHostButton;
    [SerializeField] private GameObject mainJoinButton;
    [SerializeField] private GameObject joinScreen;

    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;

    [SerializeField] Button submitCodeButton;
    [SerializeField] TextMeshProUGUI codeText;


    void OnEnable()
    {
        hostButton.onClick.AddListener(OnHostClicked);
        joinButton.onClick.AddListener(OnJoinClicked);
        submitCodeButton.onClick.AddListener(SubmitCodeClicked);
    }

    private void OnDisable()
    {
        hostButton.onClick.RemoveListener(OnHostClicked);
        joinButton.onClick.RemoveListener(OnJoinClicked);
        submitCodeButton.onClick.RemoveListener(SubmitCodeClicked);
    }

    private async void OnHostClicked()
    {
        try
        {
            bool succeeded = await GameLobbyManager.Instance.CreateLobby();
            if (succeeded)
            {
                if (FadeInFadeOutSceneManager.Instance != null)
                {
                    Destroy(FadeInFadeOutSceneManager.Instance.gameObject);
                }
                SceneManager.LoadSceneAsync("LobbyRoomScene");
            }
            else
            {
                Debug.LogError("로비 생성 실패");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"로비 생성 실패 : {ex.Message}");
        }

    }

    private void OnJoinClicked()
    {

        mainHostButton.SetActive(false);
        mainJoinButton.SetActive(false);
        joinScreen.SetActive(true);
    }

    private async void SubmitCodeClicked()
    {

        string code = codeText.text.Trim();
        Debug.Log(code);
        try
        {
            bool succeeded = await GameLobbyManager.Instance.JoinLobby(code);
            if (succeeded)
            {
                if (FadeInFadeOutSceneManager.Instance != null)
                {
                    Destroy(FadeInFadeOutSceneManager.Instance.gameObject);
                }
                SceneManager.LoadSceneAsync("LobbyRoomScene");
            }
            else
            {
                Debug.LogError("로비 입장 실패");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"로비 입장 실패 : {ex.Message}");
        }
    }

}
