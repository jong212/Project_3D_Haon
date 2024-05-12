using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Jeweltext : MonoBehaviour
{
    private TextMeshProUGUI jewels;
    void Start()
    {
        jewels = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        jewels.text = $" {Setting_1.Instance.jewels}";
    }
}
