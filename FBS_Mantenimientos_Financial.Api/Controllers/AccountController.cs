using FBS_Mantenimientos_Financial.Api.Services;
using FBS_Mantenimientos_Financial.Domain.Entities;
using FBS_Mantenimientos_Financial.Domain.Entities.Autenticar;
using FBS_Mantenimientos_Financial.Domain.Modelos.Autenticar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FBS_Mantenimientos_Financial.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;

        public AccountController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Authenticate([FromBody] UsuarioLogin request, CancellationToken cancellationToken)
        {
            var user = await _usuarioService.Autenticar(request);

            if (user == null)
            {
                return BadRequest();
            }
            SetRefreshTokenInCookie(user.RefrescarToken);
            return Ok(user);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefrescarToken([FromBody] RefrescarTokenDTO refreshTokenDTO)
        {
            var refreshToken = Request.Cookies["refrescarToken"];
            var response = await _usuarioService.RefrescarTokenAsync(refreshTokenDTO.RefrescarToken);
            if (!string.IsNullOrEmpty(response.RefrescarToken))
            {
                SetRefreshTokenInCookie(response.RefrescarToken.ToString());
            }
            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevocarToken([FromBody] RevocarTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refrescarToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { mensaje = "Es requerido el token" });

            var response = await _usuarioService.RevocarToken(token);

            if (!response)
                return NotFound(new { mensaje = "Token no encontrado" });

            return Ok(new { mensaje = "Token revocado" });
        }

        private void SetRefreshTokenInCookie(string refrescarToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(2),
            };
            Response.Cookies.Append("refrescarToken", refrescarToken, cookieOptions);
        }
    }
}