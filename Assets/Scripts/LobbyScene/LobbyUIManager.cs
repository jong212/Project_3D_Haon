using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private Button createRoomButton;
    
    [SerializeField] private Button startRoomButton;
    

    private void OnEnable()
    {


        createRoomButton.onClick.AddListener(CreateRoom);
        //leaveRoomButton.onClick.AddListener(OnClickedLeaveRoom);
        startRoomButton.onClick.AddListener(OnClickedStartRoom);


    }

    private void OnDisable()
    {


        createRoomButton.onClick.RemoveListener(CreateRoom);
        //leaveRoomButton.onClick.RemoveListener(OnClickedLeaveRoom);
        startRoomButton.onClick.RemoveListener(OnClickedStartRoom);

    }

    private async void CreateRoom()
    {
    //    Debug.Log("Creating room...");
    //    Dictionary<string, string> lobbyData = new Dictionary<string, string>()
    //{
    //    { "MapIndex", $"{LobbyController.currentMapIndex}" }, // Set a default MapIndex
    //    { "SceneName", "파티사냥 하실분" } // Set a default SceneName
    //};

    //    bool success = await LobbyManager.Instance.CreateLobby(3, false, new Dictionary<string, string>(), lobbyData);
    //    if (success)
    //    {
    //        Debug.Log("Room created successfully.");
    //        await RefreshLobbyList();
    //        leaveRoomButton.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("Failed to create room.");
    //    }
    }

    private void OnClickedLeaveRoom()
    {
       // leaveRoomButton.gameObject.SetActive(false);
        createRoomButton.gameObject.SetActive(true);
    }
    private void OnClickedStartRoom()
    {
        // Lobby 방의 인원들 모두 Scene 이동
    }
   

}

