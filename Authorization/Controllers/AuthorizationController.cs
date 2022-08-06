using Authorization.Contracts.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IAuthorizationService = Authorization.Abstractions.Authorization.IAuthorizationService;

namespace Authorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService) 
            => _authorizationService = authorizationService;

        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        /// <response code="200">В случае успешной регистрации</response>
        /// <response code="400">В случае ошибок валидации</response>
        [AllowAnonymous]
        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration(UserParameters parameters)
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
        [HttpGet("user/{userId:guid}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
        {
            var user = await _authorizationService.GetUserById(userId);
         
            return user is null 
                ? NotFound()
                : Ok(await _authorizationService.GetUserById(userId));
        }

        //todo: переделать под параметры (пейджинация и поиск)
        /// <summary>
        /// Возвращает первые 300 пользователей отсортированные по никнейму
        /// </summary>
        /// <returns></returns>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <response code="200">В случае, если пользователь был найден в системе</response>
        /// <response code="404">В случае если пользователь не был найден</response>
        [Authorize]
        [HttpPost("users")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersByParameters()
        {
            return Ok(await _authorizationService.GetUsers());
        }
    }
}
