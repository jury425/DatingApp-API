using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        //// GET: api/Auth/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Auth
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            // ------------ REQUEST VALIDATION PART ------------ //

            // ------------------------------------------------- //

            userRegisterDto.Username = userRegisterDto.Username.ToLower();

            if (await _authRepository.UserExists(userRegisterDto.Username))
                return BadRequest("Użytkownik istnieje już w systemie.");

            User user = new User{ Username = userRegisterDto.Username };
            await _authRepository.Register(user, userRegisterDto.Password);

            // ------------ FIX RESPONSE ------------ //
            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            User user = await _authRepository.Login(userLoginDto.Username, userLoginDto.Password);

            if (user == null)
                return Unauthorized();

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
