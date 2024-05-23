using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class AuthenticationManager : MonoBehaviour
{
    async void Start()
    {
        await InitializeUnityServices();
        SetupEvents();
        await SignInAnonymouslyAsync();
    }

    private async Task InitializeUnityServices()
    {
        await UnityServices.InitializeAsync();
    }

    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in!");
        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError($"Sign in failed: {err}");
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Signed out!");
        };
    }

    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"Sign in failed: {ex}");
        }
    }
}
