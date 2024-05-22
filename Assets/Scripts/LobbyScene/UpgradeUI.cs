using UnityEngine;

public enum UpgardeButton
{
    CoinUpATK,
    CoinDownATK,
    CoinUpHP,
    CoinDownHp,
    JewelUpATK,
    JewelDownATK,
    JewelUpHp,
    JewelDownHp
}
public class UpgradeUI : MonoBehaviour
{
    public UpgardeButton currentType;

    public void OnUpgradeButton()
    {
        switch (currentType)
        {
            case UpgardeButton.CoinUpATK:
                Setting_1.CoinUpGradeATK();
                break;
            case UpgardeButton.CoinDownATK:
                Setting_1.CoinDownGradeATK();
                break;
            case UpgardeButton.CoinUpHP:
                Setting_1.CoinUpGradeHP();
                break;
            case UpgardeButton.CoinDownHp:
                Setting_1.CoinDownGradeHP();
                break;
            case UpgardeButton.JewelUpATK:
                Setting_1.JewelUpGradeATK();
                break;
            case UpgardeButton.JewelDownATK:
                Setting_1.JewelDownGradeATK();
                break;
            case UpgardeButton.JewelUpHp:
                Setting_1.JewelUpGradeHP();
                break;
            case UpgardeButton.JewelDownHp:
                Setting_1.JewelDownGradeHP();
                break;

        }
    }
}
