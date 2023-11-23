using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Authorization.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : Controller
{
    protected Guid UserId => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
}