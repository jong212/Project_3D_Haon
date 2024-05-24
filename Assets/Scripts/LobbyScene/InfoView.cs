using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoView : MonoBehaviour
{
    public TextMeshProUGUI cointext;
    public TextMeshProUGUI jeweltext;
    public TextMeshProUGUI currentatk;
    public TextMeshProUGUI currenthp;
    public TextMeshProUGUI afteratk;
    public TextMeshProUGUI afterhp;
    public TextMeshProUGUI jewelAtkUpgradetext;
    public TextMeshProUGUI jewelHPUpgradetext;
    public TextMeshProUGUI coinAtkUpgradetext;
    public TextMeshProUGUI coinHPUpgradetext;

    private void Update()
    {
        cointext.text = $" {PlayInfo.coins}";
        jeweltext.text = $" {PlayInfo.jewels}";
        jewelAtkUpgradetext.text = $"-{PlayInfo.jewelAtkUpgradeCount}";
        jewelHPUpgradetext.text = $"-{PlayInfo.jewelHPUpgradeCount}";
        coinAtkUpgradetext.text = $"-{PlayInfo.coinAtkUpgradeCount * 5}";
        coinHPUpgradetext.text = $"-{PlayInfo.coinHPUpgradeCount * 5}";
        currentatk.text = $"ATK  :  {PlayInfo.atk}  >";
        currenthp.text = $"HP  :  {PlayInfo.hp}  >";
        afteratk.text = $"{PlayInfo.atk + 1}";
        afterhp.text = $"{PlayInfo.hp + 5}";

    }
}
