using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private Vector2 defaultInitialPlanePosition = new Vector2(-14, -19);
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            SpawnPlayers();

        }
    }

    private void SpawnPlayers()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            SpawnPlayerForClient(client.ClientId);
        }


    }

    private void SpawnPlayerForClient(ulong clientId)
    {
        if (SceneManager.GetActiveScene().name == "WaveField")
        {
            GameObject playerInstance = Instantiate(playerPrefab, new Vector3(-16, 1, -154), Quaternion.identity);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
        else if (SceneManager.GetActiveScene().name == "BossField")
        {
            GameObject playerInstance = Instantiate(playerPrefab, new Vector3(30, -2, -1), Quaternion.identity);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
        else
        {
            GameObject playerInstance = Instantiate(playerPrefab, new Vector3(), Quaternion.identity);
            playerInstance.gameObject.SetActive(false);
        }
            
    }
}
