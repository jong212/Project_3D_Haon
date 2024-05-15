using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private Renderer isReadyRenderer;


    private MaterialPropertyBlock propertyBlock;
    private LobbyPlayerData _data;

    private void Start()
    {

        propertyBlock = new MaterialPropertyBlock();
    }


    public void SetData(LobbyPlayerData data)
    {
        _data = data;
        _playerName.text = _data.GamerTag;

        if (data.IsReady)
        {
            if (isReadyRenderer != null)
            {
                isReadyRenderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor("_BaseColor", Color.green);
                isReadyRenderer.SetPropertyBlock(propertyBlock);
            }
        }
        gameObject.SetActive(true);
    }


}
