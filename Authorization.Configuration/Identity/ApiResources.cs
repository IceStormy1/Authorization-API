using Authorization.Common;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Authorization.Configuration.Identity;

internal static class ApiResources
{
    public static IEnumerable<ApiResource> Get()
    {
        var claims = new List<string>
        {
            "snils",
            "middle_name",
            "first_name",
            "last_name",
            JwtClaimTypes.Subject
        };

        return new ApiResource[]
        {
            new()
            {
                Name = ClientConstants.TheaterClientName,
                DisplayName = ClientConstants.TheaterClientName,
                ApiSecrets = new Secret[]
                {
                    new(ClientConstants.ClientSecret.Sha256())
                },
                UserClaims = claims,
                Scopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    $"{ClientConstants.TheaterClientName}.read",
                    $"{ClientConstants.TheaterClientName}.write"
                },
            },
            new()
            {
                Name = ClientConstants.TheaterClientId + ".interactive",
                DisplayName = ClientConstants.TheaterClientId + ".interactive",
                UserClaims = claims,
                ApiSecrets = new Secret[]
                {
                    new(ClientConstants.ClientSecret.Sha256())
                },
                Scopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    $"{ClientConstants.TheaterClientName}.read",
                    $"{ClientConstants.TheaterClientName}.write"
                },
            }
        };
    }
}