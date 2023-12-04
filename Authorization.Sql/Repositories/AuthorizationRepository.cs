using Authorization.Abstractions.Authorization;
using Authorization.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Authorization.Sql.Repositories;

public class AuthorizationRepository : IAuthorizationRepository
{
    private readonly AuthorizationDbContext _authorizationDbContext;

    public AuthorizationRepository(AuthorizationDbContext authorizationDbContext)
    {
        _authorizationDbContext = authorizationDbContext;
    }

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