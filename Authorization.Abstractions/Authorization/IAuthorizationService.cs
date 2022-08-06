using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Contracts.Authorization;

namespace Authorization.Abstractions.Authorization
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Получить пользователя по уникальному идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Данные пользователя</returns>
        public Task<UserModel> GetUserById(Guid userId);

        /// <summary>
        /// Получить список всех пользователей 
        /// </summary>
        /// <returns>Список пользователей</returns>
        /// TODO: Добавить параметры фильтрации (пейджинация)
        public Task<IList<UserModel>> GetUsers();

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <param name="user">Данные пользователя</param>
        /// <returns>Возвращает true при успешном создании пользователя</returns>
        public Task<(bool IsSuccess, Guid? UserId)> CreateUser(UserParameters user);
    }
}
