using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Setting_1 : MonoBehaviour
{
    public static void JewelUpGradeATK()
    {
        if (UserData.Instance.Character.Gems > UserData.Instance.Character.AttackEnhancement)
        {
            UserData.Instance.Character.AttackPower++;
            UserData.Instance.Character.Gems -= UserData.Instance.Character.AttackEnhancement;
            UserData.Instance.Character.AttackEnhancement++;
            UserData.Instance.SavePlayerData();
        }
    }

    // 공격력을 보석으로 다운그레이드
    public static void JewelDownGradeATK()
    {
        if (UserData.Instance.Character.AttackPower > 0 && UserData.Instance.Character.AttackEnhancement > 0)
        {
            UserData.Instance.Character.AttackPower--;
            UserData.Instance.Character.Gems += (UserData.Instance.Character.AttackEnhancement - 1);
            UserData.Instance.Character.AttackEnhancement--;
            UserData.Instance.SavePlayerData();
        }
    }

    // 체력을 보석으로 업그레이드
    public static void JewelUpGradeHP()
    {
        if (UserData.Instance.Character.Gems > UserData.Instance.Character.HealthEnhancement * 5)
        {
            UserData.Instance.Character.MaxHealth += 5;
            UserData.Instance.Character.Gems -= UserData.Instance.Character.HealthEnhancement * 5;
            UserData.Instance.Character.HealthEnhancement++;
            UserData.Instance.SavePlayerData();
        }
    }

    // 체력을 보석으로 다운그레이드
    public static void JewelDownGradeHP()
    {
        if (UserData.Instance.Character.MaxHealth > 0 && UserData.Instance.Character.HealthEnhancement > 0)
        {
            UserData.Instance.Character.MaxHealth -= 5;
            UserData.Instance.Character.Gems += (UserData.Instance.Character.HealthEnhancement - 1) * 5;
            UserData.Instance.Character.HealthEnhancement--;
            UserData.Instance.SavePlayerData();
        }
    }

    // 공격력을 코인으로 업그레이드
    public static void CoinUpGradeATK()
    {
        if (UserData.Instance.Character.Coins > UserData.Instance.Character.AttackEnhancement * 5)
        {
            UserData.Instance.Character.AttackPower++;
            UserData.Instance.Character.Coins -= UserData.Instance.Character.AttackEnhancement * 5;
            UserData.Instance.Character.AttackEnhancement++;
            UserData.Instance.SavePlayerData();
        }
    }

    // 공격력을 코인으로 다운그레이드
    public static void CoinDownGradeATK()
    {
        if (UserData.Instance.Character.AttackPower > 0 && UserData.Instance.Character.AttackEnhancement > 0)
        {
            UserData.Instance.Character.AttackPower--;
            UserData.Instance.Character.Coins += (UserData.Instance.Character.AttackEnhancement - 1) * 5;
            UserData.Instance.Character.AttackEnhancement--;
            UserData.Instance.SavePlayerData();
        }
    }

    // 체력을 코인으로 업그레이드
    public static void CoinUpGradeHP()
    {
        if (UserData.Instance.Character.Coins > UserData.Instance.Character.HealthEnhancement * 5)
        {
            UserData.Instance.Character.MaxHealth += 5;
            UserData.Instance.Character.Coins -= UserData.Instance.Character.HealthEnhancement * 5;
            UserData.Instance.Character.HealthEnhancement++;
            UserData.Instance.SavePlayerData();
        }
    }

    // 체력을 코인으로 다운그레이드
    public static void CoinDownGradeHP()
    {
        if (UserData.Instance.Character.MaxHealth > 0 && UserData.Instance.Character.HealthEnhancement > 0)
        {
            UserData.Instance.Character.MaxHealth -= 5;
            UserData.Instance.Character.Coins += (UserData.Instance.Character.HealthEnhancement - 1) * 5;
            UserData.Instance.Character.HealthEnhancement--;
            UserData.Instance.SavePlayerData();
        }
    }

    // 코인 증가
    public static void PlusCoins()
    {
        UserData.Instance.Character.Coins++;
        UserData.Instance.SavePlayerData();
    }

    // 보석 증가
    public static void PlusJewels()
    {
        UserData.Instance.Character.Gems++;
        UserData.Instance.SavePlayerData();
    }

}
