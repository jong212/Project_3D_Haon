using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class UserData : Singleton<UserData>
{
    public string UserId { get; set; }
    public CharacterData Character { get; set; }

    private const string ApiEndpoint = "/api/players";


    

    public void LoadPlayerData(string userId, CharacterData character)
    {
        UserId = userId;
        Character = character;
    }
    private void Start()
    {
        this.InitializeUserData();
    }



    public void InitializeUserData()
    {
        // UserId와 Character 데이터를 수동으로 설정합니다.
        UserId = "exampleUserId";
        Character = new CharacterData
        {
            PlayerName = "examplePlayerName",
            PlayerId = "examplePlayerId",
            Gems = 100,
            Coins = 50,
            MaxHealth = 100,
            HealthEnhancement = 1,
            AttackPower = 10,
            AttackEnhancement = 1,
            WeaponEnhancement = 1,
            ArmorEnhancement = 1
        };
    }


    private void LogError(UnityWebRequest request, string message)
    {
        Debug.LogError($"{message}: {request.error}");
        Debug.LogError($"Response Code: {request.responseCode}");
        Debug.LogError($"Response Text: {request.downloadHandler.text}");
    }
}

public static class UnityWebRequestExtensions
{
    public static Task<UnityWebRequest> SendWebRequestAsync(this UnityWebRequest request)
    {
        var tcs = new TaskCompletionSource<UnityWebRequest>();
        request.SendWebRequest().completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                tcs.SetException(new UnityWebRequestException(request));
            }
            else
            {
                tcs.SetResult(request);
            }
        };
        return tcs.Task;
    }
}

public class UnityWebRequestException : System.Exception
{
    public UnityWebRequest Request { get; }

    public UnityWebRequestException(UnityWebRequest request) : base(request.error)
    {
        Request = request;
    }
}