using System.Collections.Generic;
using Authorization.Common;
using IdentityServer4.Models;

namespace Authorization.Identity;

public static class Clients
{
    public static List<Client> Get()
    {
        var clientSecret = ClientConstants.ClientSecret.Sha256();
        return new List<Client>
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = ClientConstants.TheaterClientId + ".m2m.client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret(clientSecret) },
                AccessTokenType = AccessTokenType.Reference,
                AllowedScopes = { $"{ClientConstants.TheaterClientName}.read", $"{ClientConstants.TheaterClientName}.write" }
            },
            // interactive client using code flow + pkce
            new Client
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
                AllowPlainTextPkce = false
            },

            //new()
            //{
            //    ClientName = ClientConstants.TheaterClientName,
            //    Enabled = true,
            //    ClientId = ClientConstants.TheaterClientId,
            //    AllowedGrantTypes = GrantTypes.ClientCredentials,
            //    AccessTokenType = AccessTokenType.Reference,
            //    RequireConsent = false,
            //    RequirePkce = false, // todo
            //    RequireClientSecret = false,
            //    AllowAccessTokensViaBrowser = true,
            //    AlwaysIncludeUserClaimsInIdToken = true,
            //    RefreshTokenUsage = TokenUsage.ReUse,AllowOfflineAccess = true,
            //    RedirectUris = new List<string>
            //    {
            //        "http://127.0.0.1:5173/login",
            //        "https://127.0.0.1:5173/login",

            //        "http://localhost:5444/login",
            //        "http://localhost:5001/login",
            //    },
            //    PostLogoutRedirectUris = new List<string>
            //    {
            //        "http://127.0.0.1:5173/logout",
            //        "https://127.0.0.1:5173/logout",

            //        "http://localhost:5444/logout",
            //        "http://localhost:5001/logout",
            //    },
            //    ClientSecrets = new List<Secret>
            //    {
            //        new(clientSecret),
            //    },
            //    AllowedScopes = new List<string>
            //    {
            //        IdentityServerConstants.StandardScopes.OpenId,
            //        IdentityServerConstants.StandardScopes.Profile,
            //        IdentityServerConstants.StandardScopes.Email,
            //        IdentityServerConstants.StandardScopes.Phone,
            //        "read",
            //        "write",
            //        "se_token"
            //    },
            //    Claims = new List<ClientClaim>
            //    {
            //    },
            //    AllowedCorsOrigins = new List<string>
            //    {
            //        "https://127.0.0.1:5173",
            //        "https://localhost:5000",

            //        "http://127.0.0.1:5173",
            //        "http://localhost:5000",

            //        "http://localhost:5444",
            //        "http://localhost:5001",
            //    }
        
    };
    }
}