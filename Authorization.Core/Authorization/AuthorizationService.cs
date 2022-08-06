using Authorization.Abstractions.Authorization;
using Authorization.Contracts.Authorization;
using Authorization.Core.Helpers;
using Authorization.Entities.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authorization.Core.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMapper _mapper;

        public AuthorizationService(
            IAuthorizationRepository authorizationRepository,
            IMapper mapper)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = mapper;
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
    }
}
