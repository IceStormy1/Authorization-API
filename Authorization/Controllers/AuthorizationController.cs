using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Authorization.Abstraction.Authorization;

namespace Authorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        /// <response code="200">В случае успешной регистрации</response>
        /// <response code="400">В случае ошибок валидации</response>
        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration()
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">В случае успешной авторизации</response>
        /// <response code="404">В случае если пользователь не был найден</response>
        [HttpPost("authorize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Authorize()
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">В случае успешной авторизации</response>
        /// <response code="404">В случае если пользователь не был найден</response>
        [HttpGet("user/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            var user = await _authorizationService.GetUserById(userId);
            // TODO: вынести в отдельный контроллер и сервисы
            return user is null 
                ? NotFound()
                : Ok(await _authorizationService.GetUserById(userId));
        }
    }
}
