using IdentityServer4.Models;

namespace Authorization.Configuration.Identity;

internal static class Resources
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
            new IdentityResources.Phone { UserClaims = new[] { "phone" } }
        };
    }
}