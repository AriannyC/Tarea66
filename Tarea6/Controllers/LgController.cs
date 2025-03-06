using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tarea6.Contex;
using Tarea6.DTO;
using Tarea6.Encriptor;
using Tarea6.Models;
using Tarea6.Contex;
using Tarea6.DTO;
using Tarea6.Encriptor;
using Tarea6.Models;

namespace Tarea7.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class LgController : ControllerBase
    {
        private readonly UserContex _user;
        private readonly Utilidad _utilidad;
        private readonly IConfiguration _configuration;

        public LgController(UserContex user, Utilidad utilidad, IConfiguration configuration)
        {
            _user = user;
            _utilidad = utilidad;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Registrarte")]
        public async Task<IActionResult> Registrarte(UserDTO Us)
        {
            var model = new RegiUs
            {
                Username = Us.Username,
                Password = _utilidad.encriptar(Us.Password),
                refreshtoken1 = Guid.NewGuid().ToString(),
                TokenExpired = DateTime.UtcNow.AddMinutes(7)
            };

            await _user.RegiUss.AddAsync(model);
            await _user.SaveChangesAsync();

            if (model.IdR != 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
            }
        }

        [HttpPost]
        [Route("LOGIN")]
        public async Task<IActionResult> LGN(LDTO OB)
        {
            var encontrados = await _user.RegiUss
                .Where(u => u.Username == OB.Username && u.Password == _utilidad.encriptar(OB.Password))
                .FirstOrDefaultAsync();

            if (encontrados == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            }
            else
            {
                var resto = _utilidad.refrestoken();

                encontrados.refreshtoken1 = resto.refreshtoken;
                encontrados.TokenExpired = resto.Expires;
                await _user.SaveChangesAsync();

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = resto.Expires
                };
                Response.Cookies.Append("refresh", resto.refreshtoken, cookieOptions);

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidad.GeneratJTW(encontrados) });
            }
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh"];



            var usuario = await _user.RegiUss.FirstOrDefaultAsync(u => u.refreshtoken1 == refreshToken);

            if (usuario == null)
            {
                return Unauthorized("Invalid Refresh Token");
            }

            if (usuario.TokenExpired < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = _utilidad.GeneratJTW(usuario);
            var newRefreshToken = _utilidad.refrestoken();
            usuario.refreshtoken1 = newRefreshToken.refreshtoken;
            usuario.TokenExpired = newRefreshToken.Expires;

            await _user.SaveChangesAsync();

            return Ok(token);

        }
    }
}
