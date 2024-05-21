using TMPro;
using Unity.Services.Authentication;
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
        bool succeeded = await GameLobbyManager.Instance.CreateLobby();
        if (succeeded)
        {
            Destroy(FadeInFadeOutSceneManager.Instance.gameObject);
            SceneManager.LoadSceneAsync("LobbyRoomScene");
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
        Debug.Log("Submit code button clicked");
        string code = codeText.text;
        code = code.Substring(0, code.Length - 1);
        Debug.Log($"{code}");
        Debug.Log(AuthenticationService.Instance.PlayerId + "¿‘¥œ¥Ÿ!");
        bool succeeded = await GameLobbyManager.Instance.JoinLobby(code);

        if (succeeded)
        {
            Debug.Log("Joined lobby successfully");
            Destroy(FadeInFadeOutSceneManager.Instance.gameObject);
            SceneManager.LoadSceneAsync("LobbyRoomScene");
        }
        else
        {
            Debug.Log("Joined lobby failed");

        }
    }

}
