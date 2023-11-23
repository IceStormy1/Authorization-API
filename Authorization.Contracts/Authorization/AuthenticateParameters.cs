namespace Authorization.Contracts.Authorization;

public class AuthenticateParameters : UserBase
{
    public string ReturnUrl { get; set; }
}