using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserContex _user;
        private readonly Utilidad _utilidad;

        public LgController(UserContex user, Utilidad utilidad)
        {
            _user = user;
            _utilidad = utilidad;

        }

        [HttpPost]
        [Route("Registrarte")]


        public async Task<IActionResult> Registrarte(UserDTO Us)
        {
            var model = new RegiUs
            {
                Username = Us.Username,
                Password = _utilidad.encriptar(Us.Password),
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
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidad.GeneratJTW(encontrados) });

            }
        }
    }
}

    

