using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.UI.Pages;

public class LogoutModel : PageModel
{
    private readonly IConfiguration config;

    public LogoutModel(IConfiguration config)
    {
        this.config = config;
    }

    public IActionResult OnGetAsync()
    {
        return SignOut(
            new AuthenticationProperties
            {
                RedirectUri = config["applicationUrl"]
            },
            OpenIdConnectDefaults.AuthenticationScheme,
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
}