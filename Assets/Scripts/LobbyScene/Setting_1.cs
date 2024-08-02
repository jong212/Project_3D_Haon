using UnityEngine;

public class Setting_1 : MonoBehaviour
{
    private void Start()
    {
        // UserData�� �ʱ�ȭ�մϴ�.
        UserData.Instance.InitializeUserData();

        // �����Ͱ� �ε�� �Ŀ��� ĳ���� �����Ϳ� ������ �� �ֽ��ϴ�.
        if (UserData.Instance.Character != null)
        {
            Debug.Log("Character data is loaded and ready to use.");
        }
        else
        {
            Debug.LogError("Failed to load character data.");
        }
    }

    public static void JewelUpGradeATK()
    {
        if (UserData.Instance.Character != null && UserData.Instance.Character.Gems > UserData.Instance.Character.AttackEnhancement)
        {
            UserData.Instance.Character.AttackPower++;
            UserData.Instance.Character.Gems -= UserData.Instance.Character.AttackEnhancement;
            UserData.Instance.Character.AttackEnhancement++;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded or not enough gems.");
        }
    }

    public static void JewelDownGradeATK()
    {
        if (UserData.Instance.Character != null && UserData.Instance.Character.AttackPower > 0 && UserData.Instance.Character.AttackEnhancement > 0)
        {
            UserData.Instance.Character.AttackPower--;
            UserData.Instance.Character.Gems += (UserData.Instance.Character.AttackEnhancement - 1);
            UserData.Instance.Character.AttackEnhancement--;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded or cannot downgrade ATK.");
        }
    }

    public static void JewelUpGradeHP()
    {
        if (UserData.Instance.Character != null && UserData.Instance.Character.Gems > UserData.Instance.Character.HealthEnhancement * 5)
        {
            UserData.Instance.Character.MaxHealth += 5;
            UserData.Instance.Character.Gems -= UserData.Instance.Character.HealthEnhancement * 5;
            UserData.Instance.Character.HealthEnhancement++;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded or not enough gems.");
        }
    }

    public static void JewelDownGradeHP()
    {
        if (UserData.Instance.Character != null && UserData.Instance.Character.MaxHealth > 0 && UserData.Instance.Character.HealthEnhancement > 0)
        {
            UserData.Instance.Character.MaxHealth -= 5;
            UserData.Instance.Character.Gems += (UserData.Instance.Character.HealthEnhancement - 1) * 5;
            UserData.Instance.Character.HealthEnhancement--;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded or cannot downgrade HP.");
        }
    }

    public static void CoinUpGradeATK()
    {
        if (UserData.Instance.Character != null && UserData.Instance.Character.Coins > UserData.Instance.Character.AttackEnhancement * 5)
        {
            UserData.Instance.Character.AttackPower++;
            UserData.Instance.Character.Coins -= UserData.Instance.Character.AttackEnhancement * 5;
            UserData.Instance.Character.AttackEnhancement++;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded or not enough coins.");
        }
    }

    public static void CoinDownGradeATK()
    {
        if (UserData.Instance.Character != null && UserData.Instance.Character.AttackPower > 0 && UserData.Instance.Character.AttackEnhancement > 0)
        {
            UserData.Instance.Character.AttackPower--;
            UserData.Instance.Character.Coins += (UserData.Instance.Character.AttackEnhancement - 1) * 5;
            UserData.Instance.Character.AttackEnhancement--;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded or cannot downgrade ATK.");
        }
    }

    public static void CoinUpGradeHP()
    {
        if (UserData.Instance.Character != null && UserData.Instance.Character.Coins > UserData.Instance.Character.HealthEnhancement * 5)
        {
            UserData.Instance.Character.MaxHealth += 5;
            UserData.Instance.Character.Coins -= UserData.Instance.Character.HealthEnhancement * 5;
            UserData.Instance.Character.HealthEnhancement++;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded or not enough coins.");
        }
    }

    public static void CoinDownGradeHP()
    {
        if (UserData.Instance.Character != null && UserData.Instance.Character.MaxHealth > 0 && UserData.Instance.Character.HealthEnhancement > 0)
        {
            UserData.Instance.Character.MaxHealth -= 5;
            UserData.Instance.Character.Coins += (UserData.Instance.Character.HealthEnhancement - 1) * 5;
            UserData.Instance.Character.HealthEnhancement--;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded or cannot downgrade HP.");
        }
    }

    public static void PlusCoins()
    {
        if (UserData.Instance.Character != null)
        {
            UserData.Instance.Character.Coins++;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded.");
        }
    }

    public static void PlusJewels()
    {
        if (UserData.Instance.Character != null)
        {
            UserData.Instance.Character.Gems++;
        }
        else
        {
            Debug.LogWarning("Character data is not loaded.");
        }
    }
}
