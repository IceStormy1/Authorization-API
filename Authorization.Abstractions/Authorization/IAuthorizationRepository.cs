using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Entities.Entities;

namespace Authorization.Abstractions.Authorization
{
    public interface IAuthorizationRepository
    {
        /// <summary>
        /// Получить пользователя по уникальному идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Возвращает данные пользователя</returns>
        public Task<UserEntity> GetUserById(Guid userId);

        /// <summary>
        /// Получить список всех пользователей 
        /// </summary>
        /// <returns>Список пользователей</returns>
        /// TODO: Добавить параметры фильтрации (пейджинация)
        public Task<IList<UserEntity>> GetUsers();

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <param name="userEntity">Данные пользователя</param>
        /// <returns>Возвращает true при успешном создании пользователя</returns>
        public Task<bool> CreateUser(UserEntity userEntity);
    }
}
