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
public class ErrorResponse
{
    public string errorCode;
    public string message;
}
