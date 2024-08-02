using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class RemoteConfigManager : Singleton<RemoteConfigManager>
{
    public static string _ip { get; private set; }
    public static string _dbName { get; private set; }
    public static string _Uid { get; private set; }
    public static string _Pwd { get; private set; }
    public static string _Port { get; private set; }

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
            _ip = RemoteConfigService.Instance.appConfig.GetString("server_url");
            _dbName = RemoteConfigService.Instance.appConfig.GetString("server_dbname");
            _Uid = RemoteConfigService.Instance.appConfig.GetString("server_uid");
            _Pwd = RemoteConfigService.Instance.appConfig.GetString("server_pwd");
            _Port = RemoteConfigService.Instance.appConfig.GetString("server_port");
        }
        else
        {
            Debug.LogWarning("remote config 값을 가져오는데 실패");
        }
    }

    public struct UserAttributes { }
    public struct AppAttributes { }

}
