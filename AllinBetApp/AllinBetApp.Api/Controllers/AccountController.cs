using AllinBetApp.Api.Models;
using AllinBetApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AllinBetApp.Api.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AccountController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_authenticationService.Register(user))
            {
                return Ok("Usuario registrado correctamente");
            }

            return BadRequest("No se pudo registrar el usuario");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            string token = _authenticationService.Login(user.Name, user.Password);

            if (token != null)
            {
                return Ok(new { Token = token });
            }

            return Unauthorized("Usuario o contraseña incorrecta");
        }
    }
}