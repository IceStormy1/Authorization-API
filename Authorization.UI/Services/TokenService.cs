using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Authorization.UI.Services;

public class TokenService : ITokenService
{
    public readonly IOptions<IdentityServerSettings> IdentityServerSettings;
    private readonly ILogger<TokenService> _logger;
    public readonly DiscoveryDocumentResponse DiscoveryDocument;
    private readonly HttpClient _httpClient;

    public TokenService(
        IOptions<IdentityServerSettings> identityServerSettings, 
        ILogger<TokenService> logger)
    {
        IdentityServerSettings = identityServerSettings;
        _logger = logger;
        _httpClient = new HttpClient();
        DiscoveryDocument = _httpClient.GetDiscoveryDocumentAsync(IdentityServerSettings.Value.DiscoveryUrl).Result;

        if (DiscoveryDocument.IsError)
        {
            throw new Exception("Unable to get discovery document", DiscoveryDocument.Exception);
        }
    }

    public async Task<TokenResponse> GetToken(string scope)
    {
        var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = DiscoveryDocument.TokenEndpoint,
            ClientId = "Theater.client.interactive",
            ClientSecret = IdentityServerSettings.Value.ClientPassword,
            Scope = scope
        });

        if (tokenResponse.IsError)
        {
            throw new Exception("Unable to get token", tokenResponse.Exception);
        }
        _logger.LogInformation($"Bearer {tokenResponse.AccessToken}");

        return tokenResponse;
    }
}