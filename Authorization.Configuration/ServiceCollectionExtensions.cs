using System.Net.Security;
using Authorization.Abstractions.Jwt;
using Authorization.Common;
using Authorization.Core.Authorization;
using Authorization.Sql;
using Authorization.Sql.Extensions;
using Authorization.Sql.Repositories;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
        return services
            .Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)))
            .Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            })
            .Configure<SslClientAuthenticationOptions>(x =>
            {
                x.RemoteCertificateValidationCallback = (_, _, _, _) => true;
            })
            .Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme, opts =>
            {
                opts.SlidingExpiration = true;
                opts.ExpireTimeSpan = ClientConstants.TokenDuration;
                //opts.EventsType = typeof(CustomCookieAuthenticationEvents);

            });
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

        services.AddAuthorization(options =>
        {
            options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
            {
                policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme, IdentityServerConstants.DefaultCookieAuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var serviceTypes = typeof(AuthorizationService).Assembly
            .GetTypes()
            .Where(x => x.Name.EndsWith(ServiceSuffix) && !x.IsAbstract && !x.IsInterface)
            .ToList();

        return services.RegisterImplementations(serviceTypes);
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var repositories = typeof(AuthorizationRepository).Assembly
            .GetTypes()
            .Where(x => x.Name.EndsWith(RepositorySuffix) && !x.IsAbstract && !x.IsInterface)
            .ToList();

        return services.RegisterImplementations(repositories);
    }

    public static IServiceCollection AddAllDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContextPool<AuthorizationDbContext>(x
            => x.DefaultDataBaseConfiguration(configuration, "Authorization")
        );
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