using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSlotController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    public void Initialize(string playerName)
    {
        playerNameText.text = playerName;
    }
}
