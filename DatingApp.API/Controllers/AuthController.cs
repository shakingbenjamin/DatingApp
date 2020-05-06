using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._repo = repo;
            this._config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            //throw new Exception("Get off my lawn!");
            var userToAuthenticate = await this._repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userToAuthenticate == null)
            {
                return Unauthorized();
            }

            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, userToAuthenticate.Id.ToString()),
                new Claim(ClaimTypes.Name, userToAuthenticate.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._config.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegistrationDto userForRegistration)
        {
            // apicontroller attribute does this for us
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userForRegistration.Username = userForRegistration.Username.ToLower();

            if (await this._repo.UserExists(userForRegistration.Username))
            {
                return BadRequest("This user already exists.");
            }

            var userToRegister = new User
            {
                Username = userForRegistration.Username
            };

            var registeredUser = await this._repo.Register(userToRegister, userForRegistration.Password);
            // temp until can return the route
            return StatusCode(201);
        }
    }
}