using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tarea6.Models;
using Tarea6.Repository;

namespace Tarea6.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _context;

        public UsersController(UserRepository context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Getusers()
        {
            return await _context.Getallasync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.GetBYId(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5 actualizar
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _context.Updateasync(user);
            return NoContent();

        }

        // POST: api/Users crear
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {

            var ss = await _context.Getallasync();
            if (ss.Any(u => u.Email == user.Email))
            {
                return BadRequest("El correo electrónico ya está en uso.");
            }
            await _context.Addasync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.GetBYId(id);
            if (user == null)
            {
                return NotFound();
            }

            await _context.Deleteasync(user);
            return NoContent();
        }

    }
}
