using System;

namespace Authorization.Contracts.Authorization
{
    public class UserModel : UserParameters
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата создания пользователя
        /// </summary>
        public DateTime DateOfCreate { get; set; }
    }
}
