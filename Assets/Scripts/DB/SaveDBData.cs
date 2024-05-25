using System;

[Serializable]
public class RegisterData
{
    public string Username;
    public string Password;
    public string PlayerName;

}

[Serializable]
public class LoginData
{
    public string Username;
    public string Password;
}

[Serializable]
public class LoginResponse
{
    public string UserId;
    public string PlayerName;
    public CharacterData Character;
}

[Serializable]
public class ErrorResponse
{
    public string errorCode;
    public string message;
}


[Serializable]
public class CharacterData
{
    public string PlayerName;
    public string PlayerId;
    public int Gems;
    public int Coins;
    public int MaxHealth;
    public int HealthEnhancement;
    public int AttackPower;
    public int AttackEnhancement;
    public int WeaponEnhancement;
    public int ArmorEnhancement;

}