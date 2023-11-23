using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Authorization.UI.Services
{
    public class TokenService : ITokenService
    {
        public readonly IOptions<IdentityServerSettings> identityServerSettings;
        private readonly ILogger<TokenService> logger;
        public readonly DiscoveryDocumentResponse discoveryDocument;
        private readonly HttpClient httpClient;

        public TokenService(IOptions<IdentityServerSettings> identityServerSettings, ILogger<TokenService> logger)
        {
            this.identityServerSettings = identityServerSettings;
            this.logger = logger;
            httpClient = new HttpClient();
            discoveryDocument = httpClient.GetDiscoveryDocumentAsync(this.identityServerSettings.Value.DiscoveryUrl).Result;

            if (discoveryDocument.IsError)
            {
                throw new Exception("Unable to get discovery document", discoveryDocument.Exception);
            }
        }

        public async Task<TokenResponse> GetToken(string scope)
        {
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = identityServerSettings.Value.ClientName,
                ClientSecret = identityServerSettings.Value.ClientPassword,
                Scope = scope
            });

            if (tokenResponse.IsError)
            {
                throw new Exception("Unable to get token", tokenResponse.Exception);
            }
            logger.LogInformation($"Bearer {tokenResponse.AccessToken}");
            return tokenResponse;
        }
    }
}
