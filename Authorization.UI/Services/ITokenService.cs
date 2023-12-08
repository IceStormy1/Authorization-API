using IdentityModel.Client;

namespace Authorization.UI.Services;

public interface ITokenService
{
    Task<TokenResponse> GetToken(string scope);
}