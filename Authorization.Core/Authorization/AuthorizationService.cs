using Authorization.Abstractions.Authorization;
using Authorization.Abstractions.Jwt;
using Authorization.Contracts.Authorization;
using Authorization.Entities.Entities;
using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authorization.Core.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMapper _mapper;

        private static JwtOptions _jwtOptions;

        public AuthorizationService(
            IAuthorizationRepository authorizationRepository,
            IMapper mapper,
            IOptions<JwtOptions> jwOptions)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = mapper;
            _jwtOptions = jwOptions.Value;
        }

        public async Task<UserModel> GetUserById(Guid userId)
        {
            var user = await _authorizationRepository.GetUserById(userId);

            return _mapper.Map<UserModel>(user);
        }

        public async Task<IList<UserModel>> GetUsers()
        {
            var users = await _authorizationRepository.GetUsers();

            return _mapper.Map<List<UserModel>>(users);
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

            var token = new JwtHelper(_jwtOptions).GenerateJwtToken(userEntity);

            return userEntity is null
                ? null
                : new AuthenticateResponse { Token = token };
        }
    }
}
