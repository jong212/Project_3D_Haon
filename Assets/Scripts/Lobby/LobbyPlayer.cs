
using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private GameObject isReadyParticle;
    [SerializeField] private LobbyCharacterPointer characterPointer;

    private LobbyPlayerData _data;
    public string PlayerId { get; private set; }
    public void SetData(LobbyPlayerData data)
    {
        _data = data;
        PlayerId = _data.Id;
        _playerName.text = _data.GamerTag;

        if (characterPointer != null)
        {
            characterPointer.PlayerId = PlayerId;
        }

        if (data.IsReady)
        {
            if (isReadyParticle != null)
            {
                isReadyParticle.SetActive(true);
            }

        }

        gameObject.SetActive(true);


    }
}
