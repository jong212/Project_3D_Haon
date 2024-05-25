using TMPro;
using UnityEngine;

public class InfoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cointext;
    [SerializeField] private TextMeshProUGUI jeweltext;
    [SerializeField] private TextMeshProUGUI currentatk;
    [SerializeField] private TextMeshProUGUI currenthp;
    [SerializeField] private TextMeshProUGUI afteratk;
    [SerializeField] private TextMeshProUGUI afterhp;
    [SerializeField] private TextMeshProUGUI jewelAtkUpgradetext;
    [SerializeField] private TextMeshProUGUI jewelHPUpgradetext;
    [SerializeField] private TextMeshProUGUI coinAtkUpgradetext;
    [SerializeField] private TextMeshProUGUI coinHPUpgradetext;

    private void Update()
    {
        if (UserData.Instance != null && UserData.Instance.Character != null)
        {
            var character = UserData.Instance.Character;

            cointext.text = $" {character.Coins}";
            jeweltext.text = $" {character.Gems}";
            jewelAtkUpgradetext.text = $"-{character.AttackEnhancement}";
            jewelHPUpgradetext.text = $"-{character.HealthEnhancement}";
            coinAtkUpgradetext.text = $"-{character.AttackEnhancement * 5}";
            coinHPUpgradetext.text = $"-{character.HealthEnhancement * 5}";
            currentatk.text = $"ATK  :  {character.AttackPower}  >";
            currenthp.text = $"HP  :  {character.MaxHealth}  >";
            afteratk.text = $"{character.AttackPower + 1}";
            afterhp.text = $"{character.MaxHealth + 5}";
        }
        else
        {
            Debug.LogWarning("UserData.Instance or UserData.Instance.Character is null.");
        }
    }
}
