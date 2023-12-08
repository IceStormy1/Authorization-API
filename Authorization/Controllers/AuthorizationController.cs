using Authorization.Contracts.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Authorization.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using IAuthorizationService = Authorization.Abstractions.Authorization.IAuthorizationService;

namespace Authorization.Controllers;

[ApiController]
[Authorize]
[Route("")]
public class AuthorizationController : BaseController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly UserManager<UserEntity> _userManager;

    public AuthorizationController(
        IAuthorizationService authorizationService,
        UserManager<UserEntity> userManager)
    {
        _authorizationService = authorizationService;
        _userManager = userManager;
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
    [HttpGet("api/auth/user/{userId:guid}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
         
        return user is null 
            ? NotFound()
            : Ok(user);
    }
}