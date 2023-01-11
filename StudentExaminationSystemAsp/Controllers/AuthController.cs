using Business.Interface.Identity;
using DnsClient;
using Entities.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace StudentExaminationSystemAsp.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO register)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _authService.Register(register);

            if (result.Success) return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO login)
        {
            var result = _authService.Login(login);

            if (result.Success) return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpGet("getallusers")]
        public IActionResult GetAllUsers()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _authService.GetAllUsers();

            if (result.Success) return Ok(result.Data);

            return BadRequest(result.Message);
        }
    }
}
