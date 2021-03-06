using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
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

            var user = _mapper.Map<UserForListDto>(userToAuthenticate);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token),
                user
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

            var userToRegister = _mapper.Map<User>(userForRegistration);

            var registeredUser = await this._repo.Register(userToRegister, userForRegistration.Password);

            var userToReturn = _mapper.Map<UserForDetailedDto>(registeredUser);
            
            return CreatedAtRoute("GetUser", new { Controller = "Users", registeredUser.Id }, userToReturn);
        }
    }
}