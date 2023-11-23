namespace Authorization.Contracts.Authorization;

public abstract class UserBase
{
    /// <summary>
    /// Никнейм пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; }
}