using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UserData : Singleton<UserData>
{
    public string UserId { get; set; }
    public CharacterData Character { get; set; }

    private string saveDataUrl;
    private string loadDataUrl;

    IEnumerator Start()
    {
        while (string.IsNullOrEmpty(RemoteConfigManager.ServerUrl))
        {
            yield return null;
        }

        saveDataUrl = $"{RemoteConfigManager.ServerUrl}/api/players";
        loadDataUrl = $"{RemoteConfigManager.ServerUrl}/api/players";

        Debug.Log($"Server URL: {RemoteConfigManager.ServerUrl}");
        Debug.Log($"Save Data URL: {saveDataUrl}");
        Debug.Log($"Load Data URL: {loadDataUrl}");

    }

    public void LoadPlayerData(string userId, CharacterData character)
    {
        UserId = userId;
        Character = character;
    }

    public void SavePlayerData()
    {
        StartCoroutine(SavePlayerDataToDatabase(Character));
    }

    private IEnumerator SavePlayerDataToDatabase(CharacterData characterData)
    {
        string url = $"{saveDataUrl}/{characterData.PlayerId}";
        Debug.Log($"Saving player data to URL: {url}");

        string jsonData = JsonUtility.ToJson(characterData);
        Debug.Log($"Saving player data: {jsonData}");

        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Player data saved successfully.");
            }
            else
            {
                Debug.LogError($"Error saving player data: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");
                Debug.LogError($"Response Text: {request.downloadHandler.text}");
            }
        }
    }

    public void LoadPlayerDataFromServer(string playerName)
    {
        StartCoroutine(LoadPlayerDataFromDatabase(playerName));
    }

    private IEnumerator LoadPlayerDataFromDatabase(string playerName)
    {
        string url = $"{loadDataUrl}/{playerName}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = request.downloadHandler.text;
                CharacterData characterData = JsonUtility.FromJson<CharacterData>(jsonResult);
                Character = characterData;
                Debug.Log("Player data loaded successfully.");
            }
            else
            {
                Debug.LogError("Error loading player data: " + request.error);
            }
        }
    }
}

