using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            // validate the request
            // username and password template stuff until can parse through a DTO containing username and password
            username = username.ToLower();

            if (await this._repo.UserExists(username))
            {
                return BadRequest("This user already exists.");
            }

            var userToRegister = new User
            {
                Username = username
            };

            var registeredUser = await this._repo.Register(userToRegister, password);
            // temp until can return the route
            return StatusCode(201);
        }
    }
}