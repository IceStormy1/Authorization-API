using Authorization.Common;
using IdentityServer4.Models;

namespace Authorization.Configuration.Identity;

internal static class Clients
{
    public static List<Client> Get()
    {
        var clientSecret = ClientConstants.ClientSecret.Sha256();

        return new List<Client>
        {
            // interactive client using code flow + pkce
            new()
            {
                ClientId = ClientConstants.TheaterClientId + ".interactive",
                ClientSecrets = { new Secret(clientSecret) },
                AllowedGrantTypes = GrantTypes.Code,
                AccessTokenType = AccessTokenType.Reference,
                RedirectUris =
                {
                    "http://localhost:5444/signin-oidc"
                },
                FrontChannelLogoutUri = "http://localhost:5444/signout-oidc",
                PostLogoutRedirectUris = { "http://localhost:5444/signout-callback-oidc" },
                AllowOfflineAccess = true,
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    //"CoffeeAPI.read"
                    ClientConstants.TheaterClientName + ".read"
                },
                RequirePkce = true,
                RequireConsent = false,
                AllowPlainTextPkce = false,
                RefreshTokenUsage = TokenUsage.ReUse,
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = true,
            }
        };
    }
}