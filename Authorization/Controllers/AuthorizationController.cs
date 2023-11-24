using Authorization.Contracts.Authorization;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IAuthorizationService = Authorization.Abstractions.Authorization.IAuthorizationService;

namespace Authorization.Controllers;

[ApiController]
[Authorize]
[Route("")]
public class AuthorizationController : BaseController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IIdentityServerInteractionService _interaction;
    
    public AuthorizationController(
        IAuthorizationService authorizationService,
        IIdentityServerInteractionService interaction)
    {
        _authorizationService = authorizationService;
        _interaction = interaction;
    }

    /// <summary>
    /// Регистрирует нового пользователя
    /// </summary>
    /// <response code="200">В случае успешной регистрации</response>
    /// <response code="400">В случае ошибок валидации</response>
    [AllowAnonymous]
    [HttpPost("api/auth/registration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registration([FromBody] UserParameters parameters)
    {
        var (isSuccess, userId) = await _authorizationService.CreateUser(parameters);

        return isSuccess
            ? Ok(userId) 
            : BadRequest("Указанный пользователь уже существует");
    }

    /// <summary>
    /// Возвращает пользователя по идентификатору
    /// </summary>
    /// <returns></returns>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <response code="200">В случае, если пользователь был найден в системе</response>
    /// <response code="404">В случае если пользователь не был найден</response>
    [HttpGet("api/auth/user/")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById()
    {
        var user = await _authorizationService.GetUserById(Guid.NewGuid());
         
        return user is null 
            ? NotFound()
            : Ok(user);
    }

    /// <summary>
    /// Вход в систему
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpGet("auth/login")]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = "~/")
    {
        // validate returnUrl - either it is a valid OIDC URL or back to a local page
        if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
        {
            // user might have clicked on a malicious link - should be logged
            throw new Exception("invalid return URL");
        }

        // start challenge and roundtrip the return URL and scheme 
        var authenticationProperties = new AuthenticateParameters
        {
            ReturnUrl = returnUrl //Url.Action(nameof(Login))
        };

        //return Challenge(authenticationProperties);
        return View(authenticationProperties);
    }

    /// <summary>
    /// Вход в систему
    /// </summary>
    /// <response code="200">В случае успешной регистрации</response>
    /// <response code="400">В случае ошибок валидации</response>
    [AllowAnonymous]
    [HttpPost("auth/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromForm] AuthenticateParameters parameters)
    {
        if (!ModelState.IsValid)
            return View(parameters);

        var authenticateResult = await _authorizationService.Authorize(parameters);

        if(authenticateResult is null || !authenticateResult.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Указан неверный логин или пароль");
            return View(parameters);
        }

        return Redirect(parameters.ReturnUrl);
    }
}