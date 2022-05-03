using System;

namespace Authorization.Sql.Entities
{
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Никнейм пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Дата создания пользователя
        /// </summary>
        public DateTime DateOfCreate { get; set; }
    }
}
