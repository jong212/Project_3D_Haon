
using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private GameObject isReadyParticle;

    private LobbyPlayerData _data;

    public void SetData(LobbyPlayerData data)
    {
        _data = data;
        _playerName.text = _data.GamerTag;

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
