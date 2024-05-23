using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class RemoteConfigManager : Singleton<RemoteConfigManager>
{
    public static string ServerUrl { get; private set; }

    private void Awake()
    {
        UnityServices.InitializeAsync();
    }
    void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        FetchRemoteConfig();
    }

    void FetchRemoteConfig()
    {
        RemoteConfigService.Instance.FetchConfigs(new UserAttributes(), new AppAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        if (configResponse.requestOrigin == ConfigOrigin.Remote)
        {
            ServerUrl = RemoteConfigService.Instance.appConfig.GetString("server_url");
        }
        else
        {
            Debug.LogWarning("remote config 값을 가져오는데 실패");
        }
    }

    public struct UserAttributes { }
    public struct AppAttributes { }

}
