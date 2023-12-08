using Authorization.Common;
using IdentityServer4.Models;

namespace Authorization.Configuration.Identity;

internal static class ApiScopes
{
    public static List<ApiScope> Get()
    {
        return new List<ApiScope>
        {
            new($"{ClientConstants.TheaterClientName}.read"),
            new($"{ClientConstants.TheaterClientName}.write")
        };
    }
}