using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Abstraction.Authorization;
using Authorization.Contracts.Authorization;
using Authorization.Entities.Entities;
using AutoMapper;

namespace Authorization.Core
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

        public async Task<bool> CreateUser(UserModel user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            
            return await _authorizationRepository.CreateUser(userEntity);
        }
    }
}
