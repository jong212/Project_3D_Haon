using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //FadeInFadeOutSceneManager.Instance.ChangeScene("LobbyScene_WJH");
        }
    }


}
