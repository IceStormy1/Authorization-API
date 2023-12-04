using Authorization.Configuration.Identity;
using Authorization.Core.Authorization;
using Authorization.Entities.Entities;
using Authorization.Sql;
using Authorization.Sql.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Configuration;

public static class IdentityServerExtensions
{
    public static IIdentityServerBuilder RegisterIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddIdentityServer(options =>
            {
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;

                options.EmitScopesAsSpaceDelimitedStringInJwt = true;

                options.MutualTls.Enabled = false;
                options.MutualTls.DomainName = "mtls";
                options.UserInteraction.LoginUrl = "/Account/Login";
                options.UserInteraction.LogoutUrl = "/Account/Logout";
                //options.MutualTls.AlwaysEmitConfirmationClaim = true;
            })
            .AddAspNetIdentity<UserEntity>()
            .AddDeveloperSigningCredential()
            .AddInMemoryClients(Clients.Get())
            .AddInMemoryApiResources(ApiResources.Get())
            .AddInMemoryApiScopes(ApiScopes.Get())
            .AddInMemoryIdentityResources(Resources.Get())
            .AddJwtBearerClientAuthentication()
            .AddAppAuthRedirectUriValidator()
            .AddProfileService<AuthorizationService>()
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = dbContextOptionsBuilder
                    => dbContextOptionsBuilder.DefaultDataBaseConfiguration(
                        configuration: configuration,
                        dbName: "Authorization",
                        npgSqlOptionsAction: builder => builder.MigrationsAssembly(typeof(MigrationTool).Assembly.GetName().Name)
                    );

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                //options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
            });
    }
}