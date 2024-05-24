using TMPro;
using UnityEngine;
public class Setting_1 : MonoBehaviour
{


    //public static float hp = 10;
    //public static float atk = 1;
    //public static float coins;
    //public static float jewels;

    //public static float jewelAtkUpgradeCount;
    //public static float jewelHPUpgradeCount;
    //public static float coinAtkUpgradeCount;
    //public static float coinHPUpgradeCount;

    //public TextMeshProUGUI cointext;
    //public TextMeshProUGUI jeweltext;
    //public TextMeshProUGUI currentatk;
    //public TextMeshProUGUI currenthp;
    //public TextMeshProUGUI afteratk;
    //public TextMeshProUGUI afterhp;
    //public TextMeshProUGUI jewelAtkUpgradetext;
    //public TextMeshProUGUI jewelHPUpgradetext;
    //public TextMeshProUGUI coinAtkUpgradetext;
    //public TextMeshProUGUI coinHPUpgradetext;


    //public void Start()
    //{

    //    //coins = 100;
    //    //jewels = 50;
    //    //jewelAtkUpgradeCount = 1;
    //    //jewelHPUpgradeCount = 1;
    //    //coinAtkUpgradeCount = 1;
    //    //coinHPUpgradeCount = 1;

    //}
    //void Update()
    //{
    //    cointext.text = $"{coins}";
    //    jeweltext.text = $"{jewels}";
    //    jewelAtkUpgradetext.text = $"-{jewelAtkUpgradeCount}";
    //    jewelHPUpgradetext.text = $"-{jewelHPUpgradeCount}";
    //    coinAtkUpgradetext.text = $"-{coinAtkUpgradeCount * 5}";
    //    coinHPUpgradetext.text = $"-{coinHPUpgradeCount * 5}";
    //    currentatk.text = $"ATK  :  {atk}  >";
    //    currenthp.text = $"HP  :  {hp}  >";
    //    afteratk.text = $"{atk + 1}";
    //    afterhp.text = $"{hp + 5}";


    //}

    public static void JewelUpGradeATK()
    {
        if (PlayInfo.jewels > PlayInfo.jewelAtkUpgradeCount)
        {
            PlayInfo.atk++;
            PlayInfo.jewels -= PlayInfo.jewelAtkUpgradeCount;
            PlayInfo.jewelAtkUpgradeCount++;
        }
    }
    public static void JewelDownGradeATK()
    {
        if (PlayInfo.atk > PlayInfo.defaltatk)
        {
            PlayInfo.atk--;
            PlayInfo.jewels += (PlayInfo.jewelAtkUpgradeCount - 1);
            PlayInfo.jewelAtkUpgradeCount--;
        }
    }
    public static void JewelUpGradeHP()
    {
        if (PlayInfo.jewels > PlayInfo.jewelHPUpgradeCount * 5)
        {
            PlayInfo.hp += 5;
            PlayInfo.jewels -= PlayInfo.jewelHPUpgradeCount;
            PlayInfo.jewelHPUpgradeCount++;

        }
    }
    public static void JewelDownGradeHP()
    {
        if (PlayInfo.hp > PlayInfo.defalthp)
        {
            PlayInfo.hp -= 5;
            PlayInfo.jewels += (PlayInfo.jewelHPUpgradeCount - 1);
            PlayInfo.jewelHPUpgradeCount--;
        }
    }
    public static void CoinUpGradeATK()
    {
        if (PlayInfo.coins > PlayInfo.coinAtkUpgradeCount * 5)
        {
            PlayInfo.atk++;
            PlayInfo.coins -= PlayInfo.coinAtkUpgradeCount * 5;
            PlayInfo.coinAtkUpgradeCount++;
        }
    }
    public static void CoinDownGradeATK()
    {
        if (PlayInfo.atk > PlayInfo.defaltatk)
        {
            PlayInfo.atk--;
            PlayInfo.coins += (PlayInfo.coinAtkUpgradeCount - 1) * 5;
            PlayInfo.coinAtkUpgradeCount--;
        }
    }
    public static void CoinUpGradeHP()
    {
        if (PlayInfo.coins > PlayInfo.coinHPUpgradeCount * 5)
        {
            PlayInfo.hp += 5;
            PlayInfo.coins -= PlayInfo.coinHPUpgradeCount * 5;
            PlayInfo.coinHPUpgradeCount++;
        }
    }
    public static void CoinDownGradeHP()
    {
        if (PlayInfo.hp > PlayInfo.defalthp)
        {
            PlayInfo.hp -= 5;
            PlayInfo.coins += (PlayInfo.coinHPUpgradeCount - 1) * 5;
            PlayInfo.coinHPUpgradeCount--;
        }
    }
    public static void PlusCoins()
    {
        PlayInfo.coins++;
    }
    public static void PlusJewels()
    {
        PlayInfo.jewels++;
    }

}
