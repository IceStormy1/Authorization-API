using System.Collections.Generic;
using Authorization.Common;
using IdentityServer4;
using IdentityServer4.Models;

namespace Authorization.Identity;

public static class ApiResources
{
    public static IEnumerable<ApiResource> Get()
    {
        var claims = new List<string>
        {
            "snils",
            "birthdate",
            "middle_name",
            "first_name",
            "last_name",
            "gender",
            "id"
        };

        return new ApiResource[]
        {
            new()
            {
                Name = ClientConstants.TheaterClientName,
                ApiSecrets = new Secret[]
                {
                    new(ClientConstants.ClientSecret.Sha256())
                },
                //TODO. Прописать клеймы клиента
                UserClaims = claims,
                Scopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    "se_token",
                    $"{ClientConstants.TheaterClientName}.read",
                    $"{ClientConstants.TheaterClientName}.write"
                },
            }
        };
    }
}