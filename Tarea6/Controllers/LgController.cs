using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Tarea6.Contex;
using Tarea6.DTO;
using Tarea6.Encriptor;
using Tarea6.Models;

namespace Tarea6.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]

    [ApiController]
    public class LgController : ControllerBase
    {
        public static RegiUs res=new RegiUs();
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

            var encontrados = await _user.RegiUss.Where(u => u.Username == OB.Username
            && u.Password == _utilidad.encriptar(OB.Password)).FirstOrDefaultAsync();

            if (encontrados == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            }
            else
            {
                var resto = _utilidad.refrestoken();
                _utilidad.SetRefreshToken(resto);



                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidad.GeneratJTW(encontrados) });
               
            }
           

            

        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh"];

            if (!res.refreshtoken1.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (res.TokenExpired < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token =_utilidad.GeneratJTW(res);
            var newRefreshToken = _utilidad.refrestoken();
            _utilidad.SetRefreshToken(newRefreshToken);

            return Ok(token);
        }


    }
}

    

