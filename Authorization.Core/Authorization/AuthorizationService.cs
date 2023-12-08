using Authorization.Abstractions.Authorization;
using Authorization.Contracts.Authorization;
using Authorization.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Authorization.Core.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationRepository _authorizationRepository;
    private readonly ILogger<AuthorizationService> _logger;

    private readonly UserManager<UserEntity> _userManager;

    public AuthorizationService(
        IAuthorizationRepository authorizationRepository,
        UserManager<UserEntity> userManager,
        ILogger<AuthorizationService> logger)
    {
        _authorizationRepository = authorizationRepository;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<(bool IsSuccess, Guid? UserId)> CreateUser(UserParameters user)
    {
        var userEntity = new UserEntity();

        return await _authorizationRepository.CreateUser(userEntity);
    }

    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        context.IssuedClaims.AddRange(context.Subject.Claims);

        context.LogIssuedClaims(_logger);

        return Task.CompletedTask;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await GetUserByClaims(context.Subject);

        context.IsActive = !user.LockoutEnd.HasValue;
    }

    private async Task<UserEntity> GetUserByClaims(ClaimsPrincipal claims)
    {
        var userId = claims.FindFirst("sub")?.Value ?? throw new KeyNotFoundException("Не задан идентификатор ");

        return await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("Не найден пользователь"); // TODO: сделать нормальный ошибки
    }
}