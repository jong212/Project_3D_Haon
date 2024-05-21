
using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject readyParticle;
    [SerializeField] private LobbyCharacterPointer characterPointer;

    private LobbyPlayerData playerData;
    public string PlayerId { get; private set; }
    public void SetData(LobbyPlayerData data)
    {
        playerData = data;
        PlayerId = playerData.Id;

        if (playerNameText != null)
        {
            playerNameText.text = playerData.GamerTag;
        }

        if (characterPointer != null)
        {
            characterPointer.PlayerId = PlayerId;
        }

        if (data.IsReady && readyParticle != null)
        {
            if (readyParticle != null)
            {
                readyParticle.SetActive(true);
            }

        }

        gameObject.SetActive(true);


    }
}
