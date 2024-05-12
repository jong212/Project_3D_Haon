using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

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
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnUpgradeButton()
    {
        switch (currentType)
        {
            case UpgardeButton.CoinUpATK:
                Setting_1.Instance.CoinUpGradeATK();                    
                break;
            case UpgardeButton.CoinDownATK:
                Setting_1.Instance.CoinDownGradeATK();              
                break;
            case UpgardeButton.CoinUpHP:
                Setting_1.Instance.CoinUpGradeHP();               
                break;
            case UpgardeButton.CoinDownHp:
                Setting_1.Instance.CoinDownGradeHP();              
                break;
            case UpgardeButton.JewelUpATK:
                Setting_1.Instance.JewelUpGradeATK();               
                break;
            case UpgardeButton.JewelDownATK:
                Setting_1.Instance.JewelDownGradeATK();
                break;
            case UpgardeButton.JewelUpHp:
                Setting_1.Instance.JewelUpGradeHP();
                break;
            case UpgardeButton.JewelDownHp:
                Setting_1.Instance.JewelDownGradeHP();
                break;
           
        }
    }
}
