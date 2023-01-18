using Business.Interface.Identity;
using CorePackage.Entities.Concrete;
using DnsClient;
using Entities.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

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
        public IActionResult Register(string jsonData/*RegisterDTO register*/)
        {
            RegisterDTO register = JsonSerializer.Deserialize<RegisterDTO>(jsonData);

            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _authService.Register(register);

            if (result.Success) return Ok(result.Message);

            return Problem(result.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(string jsonData/*LoginDTO login*/)
        {
            LoginDTO login = JsonSerializer.Deserialize<LoginDTO>(jsonData);

            var result = _authService.Login(login);

            if (result.Success) return Ok(result.Message);

            return Problem(result.Message);
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

            return Problem(result.Message);
        }
    }
}
