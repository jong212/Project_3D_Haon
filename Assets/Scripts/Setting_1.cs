using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Setting_1 : MonoBehaviour
{
    

    public float hp=10;
    public float atk=1;
    public float coins;
    public float jewels;

    public float jewelAtkUpgradeCount;
    public float jewelHPUpgradeCount;
    public float coinAtkUpgradeCount;
    public float coinHPUpgradeCount;

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
    private static Setting_1 instance = null;
    void Awake()
    {
        if (null == instance)
        {

            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {

            Destroy(this.gameObject);
        }
    }


    public static Setting_1 Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

  

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
        coinAtkUpgradetext.text = $"-{coinAtkUpgradeCount*5}";
        coinHPUpgradetext.text = $"-{coinHPUpgradeCount*5}";
        currentatk.text = $"ATK  :  {atk}  >";
        currenthp.text = $"HP  :  {hp}  >";
        afteratk.text = $"{atk + 1}";
        afterhp.text = $"{hp + 5}";

        


    }

    public void JewelUpGradeATK()
    {
        if (jewels > jewelAtkUpgradeCount)
        {
            atk++;
            jewels-=jewelAtkUpgradeCount;
            jewelAtkUpgradeCount++;
        }
    }
    public void JewelDownGradeATK()
    {
        if (atk > 1)
        {
            atk--;
            jewels+=(jewelAtkUpgradeCount-1);
            jewelAtkUpgradeCount--;
        }
    }
    public void JewelUpGradeHP()
    {
        if (jewels > jewelHPUpgradeCount*5)
        {
            hp += 5;
            jewels-=jewelHPUpgradeCount;
            jewelHPUpgradeCount++;

        }
    }
    public void JewelDownGradeHP()
    {
        if (hp > 10)
        {
            hp -= 5;
            jewels+=(jewelHPUpgradeCount-1);
            jewelHPUpgradeCount--;
        }
    }
    public void CoinUpGradeATK()
    {
        if (coins > coinAtkUpgradeCount*5)
        {
            atk++;
            coins -= coinAtkUpgradeCount*5;
            coinAtkUpgradeCount++;
        }
    }
    public void CoinDownGradeATK()
    {
        if (atk > 1)
        {
            atk--;
            coins += (coinAtkUpgradeCount - 1)*5;
            coinAtkUpgradeCount--;
        }
    }
    public void CoinUpGradeHP()
    {
        if (coins > coinHPUpgradeCount*5)
        {
            hp+=5;
            coins -= coinHPUpgradeCount * 5;
            coinHPUpgradeCount++;
        }
    }
    public void CoinDownGradeHP()
    {
        if (hp > 10)
        {
            hp -= 5;
            coins += (coinHPUpgradeCount - 1)*5;
            coinHPUpgradeCount--;
        }
    }

}
