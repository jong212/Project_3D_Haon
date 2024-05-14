using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    


    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;



    void Start()
    {
        hostButton.onClick.AddListener(OnHostClicked);
        joinButton.onClick.AddListener(OnJoinClicked);
    }

    private async void OnHostClicked()
    {
        bool succeeded = await GameLobbyManager.Instance.CreateLobby();
        if (succeeded)
        {
            SceneManager.LoadSceneAsync("RoomScene_WJH");
        }

    }

    private void OnJoinClicked()
    {
        Debug.Log("Join");
    }
}
