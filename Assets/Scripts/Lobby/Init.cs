using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{

    async void Start()
    {
        await UnityServices.InitializeAsync();

        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            AuthenticationService.Instance.SignedIn += OnSignedIn;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (AuthenticationService.Instance.IsSignedIn)
            {
                // DB에서 받을 Name
                string userName = PlayerPrefs.GetString("Username");
                if (userName == "")
                {
                    userName = "Player";
                    PlayerPrefs.SetString("Username", userName);
                }

                SceneManager.LoadSceneAsync("TestScene_WJH");
            }

        }


    }

    private void OnSignedIn()
    {
        Debug.Log($"Player Id : {AuthenticationService.Instance.PlayerId}");
        Debug.Log($"Token : {AuthenticationService.Instance.AccessToken}");
    }

    void Update()
    {

    }

    



}
