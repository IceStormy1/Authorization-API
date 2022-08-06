namespace Authorization.Contracts.Authorization
{
    public class AuthenticateResponse : UserModel
    {
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
    }
}
