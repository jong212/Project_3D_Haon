using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CurrentATKtext : MonoBehaviour
{
    private TextMeshProUGUI atk;
    void Start()
    {
        atk = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        atk.text = $" {Setting_1.Instance.atk}";
    }
}
