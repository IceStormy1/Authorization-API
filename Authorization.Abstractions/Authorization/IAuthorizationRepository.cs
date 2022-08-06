using Authorization.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authorization.Abstractions.Authorization
{
    /// <summary>
    /// Репозиторий для взаимодействия с данными пользователей,
    /// зарегистрированных в системе
    /// </summary>
    public interface IAuthorizationRepository
    {
        /// <summary>
        /// Получить пользователя по уникальному идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Возвращает данные пользователя</returns>
        Task<UserEntity> GetUserById(Guid userId);

        /// <summary>
        /// Получить пользователя по его никнейму
        /// </summary>
        /// <param name="userName">Никнейм пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        Task<UserEntity> FindUser(string userName, string password);

        /// <summary>
        /// Получить список всех пользователей 
        /// </summary>
        /// <remarks>Возвращает первые 300 пользователей отсортированные по никнейму</remarks>
        /// <returns>Список пользователей</returns>
        /// TODO: Добавить параметры фильтрации (пейджинация)
        Task<IReadOnlyCollection<UserEntity>> GetUsers();

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <param name="userEntity">Данные пользователя</param>
        /// <returns>
        /// true и идентификатор пользователя при успешном создании пользователя<br/>
        /// false и <b>null</b> если не удалось создать пользователя
        /// </returns>
        Task<(bool IsSuccess, Guid? UserId)> CreateUser(UserEntity userEntity);
    }
}
