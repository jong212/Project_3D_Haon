using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayersData : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public int PlayerID;
        public string PlayerName;
        public int Gems;
        public int Coins;
        public int MaxHealth;
        public int HealthEnhancement;
        public int AttackPower;
        public int AttackEnhancement;
        public int WeaponEnhancement;
        public int ArmorEnhancement;
    }

    [System.Serializable]
    public class PlayersList
    {
        public List<PlayerData> players;
    }



}
