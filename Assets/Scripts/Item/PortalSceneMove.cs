using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSceneMove : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SceneLoader.Instance.LoadSceneAsync("LobbyScene");
        }
    }
}
