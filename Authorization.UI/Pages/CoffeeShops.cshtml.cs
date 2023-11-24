using Authorization.UI.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.UI.Pages
{
    public class CoffeeShops : PageModel
    {
        public List<CoffeeShopModel> Shops = new();
        private readonly ILogger<CoffeeShops> _logger;
        private readonly ILocalStorageService _localStorageService;
        private AuthenticateResult _authenticateResult;

        public CoffeeShops( ILogger<CoffeeShops> logger, ILocalStorageService localStorageService)
        {
            _logger = logger;
            _localStorageService = localStorageService;
        }

        public async Task OnGetAsync()
        {
            _authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var accessToken = _authenticateResult.Properties?.GetTokenValue("access_token") ?? string.Empty;

            _logger.LogInformation("Token: Bearer {Token}", accessToken);

            Shops = new List<CoffeeShopModel>();
        }

        public async Task OnAfterRenderAsync()
        {
            await _localStorageService.SetItemAsync("auth", _authenticateResult);
        }
    }
}
