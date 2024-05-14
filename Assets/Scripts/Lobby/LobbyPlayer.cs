using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;


    private LobbyPlayerData _data;

    public void SetData(LobbyPlayerData data)
    {
        _data = data;
        _playerName.text = _data.GamerTag;
        gameObject.SetActive(true);
    }


}
