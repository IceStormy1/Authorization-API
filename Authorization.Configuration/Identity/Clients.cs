﻿using Authorization.Common;
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
                ClientId = $"{ClientConstants.TheaterClientId}.interactive",
                ClientSecrets = { new Secret(clientSecret) },
                AllowedGrantTypes = GrantTypes.Code,
                AccessTokenType = AccessTokenType.Reference,
                RedirectUris =
                {
                    "http://localhost:5444/signin-oidc",
                    "https://192.168.1.112:8085/signin-oidc",
                    "https://localhost:8085/signin-oidc",
                    "http://127.0.0.1:5173/auth/signinwin/main",
                    "http://localhost:8080/auth/signinwin/main",
                },
                FrontChannelLogoutUri = "http://localhost:5445/signout-oidc",
                PostLogoutRedirectUris =
                {
                    "http://localhost:5444/signout-callback-oidc",
                    "https://192.168.1.112:8085/signout-callback-oidc",
                    "https://localhost:8085/signout-callback-oidc",
                    "http://127.0.0.1:5173/signout-callback-oidc",
                    "http://127.0.0.1:5173/"
                },
                AllowOfflineAccess = true,
                AllowedCorsOrigins = new List<string>
                {
                    "http://127.0.0.1:5173",
                    "https://192.168.1.112:8085",
                    "https://localhost:8085",
                    "http://localhost:8080"
                },
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    ClientConstants.TheaterClientName + ".read"
                },
                RequirePkce = true,
                RequireConsent = false,
                AllowPlainTextPkce = false,
                RefreshTokenUsage = TokenUsage.ReUse,
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AccessTokenLifetime = (int)ClientConstants.TokenDuration.TotalSeconds
            }
        };
    }
}