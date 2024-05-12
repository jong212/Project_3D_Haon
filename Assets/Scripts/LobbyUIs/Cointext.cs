using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Cointext : MonoBehaviour
{
    private TextMeshProUGUI coins;
    void Start()
    {
        coins = GetComponent<TextMeshProUGUI>();
    }

    
    void Update()
    {
        coins.text = $" {Setting_1.Instance.coins}";
    }
}
