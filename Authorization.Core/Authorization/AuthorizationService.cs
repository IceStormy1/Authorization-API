using Authorization.Abstractions.Authorization;
using Authorization.Contracts.Authorization;
using Authorization.Entities.Entities;
using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorizationService> _logger;

    private readonly SignInManager<UserEntity> _signInManager;
    private readonly JwtHelper _jwtHelper;
    private readonly UserManager<UserEntity> _userManager;

    public AuthorizationService(
        IAuthorizationRepository authorizationRepository,
        IMapper mapper,
        JwtHelper jwtHelper, 
        SignInManager<UserEntity> signInManager, 
        UserManager<UserEntity> userManager,
        ILogger<AuthorizationService> logger)
    {
        _authorizationRepository = authorizationRepository;
        _mapper = mapper;
        _jwtHelper = jwtHelper;
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UserModel> GetUserById(Guid userId)
    {
        var user = await _authorizationRepository.GetUserById(userId);

        return _mapper.Map<UserModel>(user);
    }

    public async Task<(bool IsSuccess, Guid? UserId)> CreateUser(UserParameters user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);

        return await _authorizationRepository.CreateUser(userEntity);
    }

    public async Task<SignInResult> Authorize(AuthenticateParameters authenticateParameters)
    {
        var userEntity = await _userManager.FindByNameAsync(authenticateParameters.UserName);

        if (userEntity is null)
            return null;

        var result = await _signInManager.PasswordSignInAsync(
            user: userEntity,
            password: authenticateParameters.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        return result;
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