using Authorization.Abstraction.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Entities.Entities;

namespace Authorization.Sql
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly AuthorizationDbContext _authorizationDbContext;

        public AuthorizationRepository(AuthorizationDbContext authorizationDbContext)
        {
            _authorizationDbContext = authorizationDbContext;
        }

        public async Task<UserEntity> GetUserById(Guid userId)
            => await _authorizationDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == userId);

        public async Task<IList<UserEntity>> GetUsers()
            => await _authorizationDbContext.Users
                .AsNoTracking()
                .Take(300)
                .ToListAsync();

        public async Task<bool> CreateUser(UserEntity userEntity)
        {
            _authorizationDbContext.Users.Add(userEntity);
            await _authorizationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
