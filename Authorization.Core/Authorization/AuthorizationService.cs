using Authorization.Abstractions.Authorization;
using Authorization.Contracts.Authorization;
using Authorization.Entities.Entities;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace Authorization.Core.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationRepository _authorizationRepository;
    private readonly IMapper _mapper;

    private readonly JwtHelper _jwtHelper;

    public AuthorizationService(
        IAuthorizationRepository authorizationRepository,
        IMapper mapper,
        JwtHelper jwtHelper)
    {
        _authorizationRepository = authorizationRepository;
        _mapper = mapper;
        _jwtHelper = jwtHelper;
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

    public async Task<AuthenticateResponse> Authorize(AuthenticateParameters authenticateParameters)
    {
        var userEntity = await _authorizationRepository
            .FindUser(authenticateParameters.UserName, authenticateParameters.Password);

        if (userEntity == null)
            return null;

        var token = _jwtHelper.GenerateJwtToken(userEntity);

        return new AuthenticateResponse { Token = token };
    }
}