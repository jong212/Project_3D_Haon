using UnityEngine;

public class LobbyCharacterPointer : MonoBehaviour
{
    public string PlayerId { get; set; }

    public void ActivateCharacterPointer()
    {
        gameObject.SetActive(true);
    }

    public void DeActivateCharacterPointer()
    {
        gameObject.SetActive(false);
    }

}
