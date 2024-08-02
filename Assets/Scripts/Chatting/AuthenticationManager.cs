using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class AuthenticationManager : MonoBehaviour
{
    public static bool IsAuthenticated { get; private set; }

    async void Start()
    {
        await InitializeServices();
    }

    private async Task InitializeServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");

            if (!AuthenticationService.Instance.IsSignedIn && !AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Signed in: {AuthenticationService.Instance.PlayerId}");
            }

            IsAuthenticated = AuthenticationService.Instance.IsSignedIn;
            Debug.Log($"Authentication Status: {IsAuthenticated}");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"AuthenticationException: {ex.Message}");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"RequestFailedException: {ex.Message}");
        }
    }
}
