using System;
using System.Threading.Tasks;
using Authorization.Contracts.Authorization;
using IdentityServer4.Services;

namespace Authorization.Abstractions.Authorization;

public interface IAuthorizationService : IProfileService
{
    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <returns>Возвращает true при успешном создании пользователя</returns>
    Task<(bool IsSuccess, Guid? UserId)> CreateUser(UserParameters user);
}