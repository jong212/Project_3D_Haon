using TMPro;
using UnityEngine;
public class Setting_1 : MonoBehaviour
{


    public static float hp = 10;
    public static float atk = 1;
    public static float coins;
    public static float jewels;

    public static float jewelAtkUpgradeCount;
    public static float jewelHPUpgradeCount;
    public static float coinAtkUpgradeCount;
    public static float coinHPUpgradeCount;

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


    public void Start()
    {

        coins = 100;
        jewels = 50;
        jewelAtkUpgradeCount = 1;
        jewelHPUpgradeCount = 1;
        coinAtkUpgradeCount = 1;
        coinHPUpgradeCount = 1;

    }
    void Update()
    {
        cointext.text = $"{coins}";
        jeweltext.text = $"{jewels}";
        jewelAtkUpgradetext.text = $"-{jewelAtkUpgradeCount}";
        jewelHPUpgradetext.text = $"-{jewelHPUpgradeCount}";
        coinAtkUpgradetext.text = $"-{coinAtkUpgradeCount * 5}";
        coinHPUpgradetext.text = $"-{coinHPUpgradeCount * 5}";
        currentatk.text = $"ATK  :  {atk}  >";
        currenthp.text = $"HP  :  {hp}  >";
        afteratk.text = $"{atk + 1}";
        afterhp.text = $"{hp + 5}";


    }

    public static void JewelUpGradeATK()
    {
        if (jewels > jewelAtkUpgradeCount)
        {
            atk++;
            jewels -= jewelAtkUpgradeCount;
            jewelAtkUpgradeCount++;
        }
    }
    public static void JewelDownGradeATK()
    {
        if (atk > 1)
        {
            atk--;
            jewels += (jewelAtkUpgradeCount - 1);
            jewelAtkUpgradeCount--;
        }
    }
    public static void JewelUpGradeHP()
    {
        if (jewels > jewelHPUpgradeCount * 5)
        {
            hp += 5;
            jewels -= jewelHPUpgradeCount;
            jewelHPUpgradeCount++;

        }
    }
    public static void JewelDownGradeHP()
    {
        if (hp > 10)
        {
            hp -= 5;
            jewels += (jewelHPUpgradeCount - 1);
            jewelHPUpgradeCount--;
        }
    }
    public static void CoinUpGradeATK()
    {
        if (coins > coinAtkUpgradeCount * 5)
        {
            atk++;
            coins -= coinAtkUpgradeCount * 5;
            coinAtkUpgradeCount++;
        }
    }
    public static void CoinDownGradeATK()
    {
        if (atk > 1)
        {
            atk--;
            coins += (coinAtkUpgradeCount - 1) * 5;
            coinAtkUpgradeCount--;
        }
    }
    public static void CoinUpGradeHP()
    {
        if (coins > coinHPUpgradeCount * 5)
        {
            hp += 5;
            coins -= coinHPUpgradeCount * 5;
            coinHPUpgradeCount++;
        }
    }
    public static void CoinDownGradeHP()
    {
        if (hp > 10)
        {
            hp -= 5;
            coins += (coinHPUpgradeCount - 1) * 5;
            coinHPUpgradeCount--;
        }
    }

}
