namespace Authorization.Contracts.Authorization;

public class UserParameters : UserBase
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }
}