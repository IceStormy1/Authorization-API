using Authorization.Abstractions.Authorization;
using Authorization.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<UserEntity> FindUser(string userName, string password)
            => await _authorizationDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => string.Equals(password, user.Password)
                                             && (string.Equals(user.UserName, userName)
                                                 || string.Equals(user.Email, userName)));

        public async Task<IReadOnlyCollection<UserEntity>> GetUsers()
            => await _authorizationDbContext.Users
                .AsNoTracking()
                .OrderBy(x=>x.UserName)
                .Take(300)
                .ToListAsync();

        public async Task<(bool IsSuccess, Guid? UserId)> CreateUser(UserEntity userEntity)
        {
            var user = await _authorizationDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => string.Equals(userEntity.UserName, user.UserName));

            if (user != null)
                return (false, null);

            _authorizationDbContext.Users.Add(userEntity);
            await _authorizationDbContext.SaveChangesAsync();

            return (true, userEntity.Id);
        }
    }
}
