using System.Security.Cryptography;
using System.Text;

namespace Authorization.Contracts.Authorization
{
    public abstract class UserBase
    {
        private string _password;

        /// <summary>
        /// Никнейм пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password
        {
            get => _password;
            set => _password = GetMD5HashPassword(value);
        }

        // ReSharper disable once InconsistentNaming
        private static string GetMD5HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return null;

            var md5 = MD5.Create();

            var bytes = Encoding.ASCII.GetBytes(password);
            var hash = md5.ComputeHash(bytes);

            var result = new StringBuilder();

            foreach (var b in hash)
                result.Append(b.ToString("X2"));
            
            return result.ToString();
        }
    }
}
