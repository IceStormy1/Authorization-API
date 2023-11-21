using Authorization.Abstractions.Jwt;
using Authorization.Core.Authorization;
using Authorization.Sql;
using Authorization.Sql.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Configuration;

public static class ServiceCollectionExtensions
{
    private const string ServiceSuffix = "Service";
    private const string RepositorySuffix = "Repository";

    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
    }

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = services.BuildServiceProvider()
                    .GetRequiredService<IOptions<JwtOptions>>().Value;

                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateIssuer = true,

                    ValidAudience = jwtOptions.Audience,
                    ValidateAudience = true,

                    IssuerSigningKey = jwtOptions.GetSymmetricSecurityKey(), // HS256
                    ValidateIssuerSigningKey = true,

                    ValidateLifetime = true
                };
            });

        var defaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = defaultPolicy;

            //options.AddRoleModelPolicies<UserPolices>(configuration, nameof(RoleModel.User));
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var serviceTypes = typeof(AuthorizationService).Assembly
            .GetTypes()
            .Where(x => x.Name.EndsWith(ServiceSuffix) && !x.IsAbstract && !x.IsInterface)
            .ToList();

        services
            .AddSingleton<JwtHelper>()
            .RegisterImplementations(serviceTypes);

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var repositories = typeof(AuthorizationRepository).Assembly
            .GetTypes()
            .Where(x => x.Name.EndsWith(RepositorySuffix) && !x.IsAbstract && !x.IsInterface)
            .ToList();

        return services.RegisterImplementations(repositories);
    }

    public static void AddAllDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<AuthorizationDbContext>(x
            =>
        {
            x.UseNpgsql(configuration.GetConnectionString("Authorization"));
            x.EnableSensitiveDataLogging();
        });
    }

    private static IServiceCollection RegisterImplementations(this IServiceCollection services, IEnumerable<Type> implementationTypes)
    {
        try
        {
            foreach (var implementationType in implementationTypes)
            {
                var serviceInterface = implementationType.GetInterfaces();

                if (serviceInterface.Length > 0)
                {
                    foreach (var @interface in serviceInterface)
                    {
                        services.AddScoped(@interface, implementationType);
                    }
                }
                else
                {
                    services.AddScoped(implementationType);
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return services;
    }
}