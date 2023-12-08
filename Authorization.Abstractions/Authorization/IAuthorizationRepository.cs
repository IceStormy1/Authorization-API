using Authorization.Entities.Entities;
using System;
using System.Threading.Tasks;

namespace Authorization.Abstractions.Authorization;

/// <summary>
/// Репозиторий для взаимодействия с данными пользователей,
/// зарегистрированных в системе
/// </summary>
public interface IAuthorizationRepository
{
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