using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Sql;

public static class ServicesCollectionRegister
{
    public static void AddAllDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<AuthorizationDbContext>(x
            => x.UseNpgsql(configuration.GetConnectionString("Authorization")));
    }
}