using UnityEngine;

public class LobbyCharacterPointer : MonoBehaviour
{
    public GameObject characterPointer;

    public void ActivateCharacterPointer()
    {
        characterPointer.SetActive(true);
    }

    public void DeActivateCharacterPointer()
    {
        characterPointer.SetActive(false);
    }

}
