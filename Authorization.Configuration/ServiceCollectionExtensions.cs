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

    public static IServiceCollection AddSameSiteCookiePolicy(this IServiceCollection services)
    {
        bool DisallowsSameSiteNone(string userAgent)
        {
            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking stack
            if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
                return true;

            // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") && userAgent.Contains("Version/") && userAgent.Contains("Safari"))
                return true;

            // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions, 
            // but pre-Chromium Edge does not require SameSite=None.
            return userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6");
        }

        void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite != SameSiteMode.None)
                return;

            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            if (!httpContext.Request.IsHttps || DisallowsSameSiteNone(userAgent))
            {
                // For .NET Core < 3.1 set SameSite = (SameSiteMode)(-1)
                options.SameSite = SameSiteMode.Unspecified;
            }
        }

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            options.OnAppendCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);

            options.OnDeleteCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
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