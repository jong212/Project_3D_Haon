using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFieldSetting : MonoBehaviour
{

    [SerializeField] private GameObject player; 

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<playerAnimator>()._hp = UserData.Instance.Character.MaxHealth;
        player.GetComponent<playerAnimator>()._str = UserData.Instance.Character.WeaponEnhancement+ UserData.Instance.Character.AttackEnhancement+ UserData.Instance.Character.AttackPower;
    }

    
}
