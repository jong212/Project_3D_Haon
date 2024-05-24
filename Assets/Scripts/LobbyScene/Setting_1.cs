using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Setting_1 : MonoBehaviour
{
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
