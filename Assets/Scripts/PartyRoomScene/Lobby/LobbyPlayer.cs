
using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject readyParticle;
    [SerializeField] private LobbyCharacterPointer characterPointer;

    private LobbyPlayerData _data;
    public string PlayerId { get; private set; }
    public void SetData(LobbyPlayerData data)
    {
        _data = data;
        PlayerId = _data.Id;

        
        if (characterPointer != null)
        {
            characterPointer.PlayerId = PlayerId;
        }

        if (data.IsReady)
        {
            if (readyParticle != null)
            {
                readyParticle.SetActive(true);
            }

        }

        gameObject.SetActive(true);


    }
}
