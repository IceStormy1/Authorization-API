using System;
using System.Threading.Tasks;
using Authorization.Contracts.Authorization;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Abstractions.Authorization;

public interface IAuthorizationService : IProfileService
{
    /// <summary>
    /// Получить пользователя по уникальному идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Данные пользователя</returns>
    Task<UserModel> GetUserById(Guid userId);

    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <returns>Возвращает true при успешном создании пользователя</returns>
    Task<(bool IsSuccess, Guid? UserId)> CreateUser(UserParameters user);

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="authenticateParameters"></param>
    Task<SignInResult> Authorize(AuthenticateParameters authenticateParameters);
}