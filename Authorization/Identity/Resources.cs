using IdentityServer4.Models;
using System.Collections.Generic;

namespace Authorization.Identity;

public static class Resources
{
    public static IEnumerable<IdentityResource> Get()
    {
        var profile = new IdentityResources.Profile
        {
            Required = true, 
            UserClaims = new List<string>{ "snils", "id"}
        };

        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            profile,
            new IdentityResources.Email { Required = true, UserClaims = new[] { "email" } },
            new IdentityResources.Phone { UserClaims = new[] { "phone" } },
            new IdentityResource() { DisplayName = "External se-token", Name = "se_token", UserClaims = new List<string> { "se_token" } },
        };
    }
}