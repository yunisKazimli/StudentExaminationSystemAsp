using Business.Interface.Examination;
using Business.Interface.Identity;
using CorePackage.Entities.Concrete;
using DnsClient;
using Entities.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace StudentExaminationSystemAsp.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IExamService _examService;

        public AuthController(IAuthService authService, IExamService examService)
        {
            _authService = authService;
            _examService = examService;
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

        [HttpGet("deleteUserById")]
        public IActionResult DeleteUserById(Guid userId)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _authService.DeleteUserById(userId);

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

            var userResult = _authService.GetAllUsers();

            if (!userResult.Success) return Problem(userResult.Message);

            var groupsResult = _examService.GetAllGroups();

            if (!groupsResult.Success) return Problem(groupsResult.Message);

            var users = userResult.Data;

            var groups = groupsResult.Data;

            List<Entities.DTOs.Identity.GetDTOs.UserGetDTO> finalResult =
                (
                    from el
                    in users
                    select new Entities.DTOs.Identity.GetDTOs.UserGetDTO()
                    {
                        UserId = el.UserId,
                        UserName = el.UserName,
                        Role = el.Role,
                        Groups = groups.Where(el1 => (el1.Instructor != null && el1.Instructor.UserId == el.UserId) || el1.Students.Where(el2 => el2.UserId == el.UserId).Count() > 0).ToList()
                    }
                ).ToList();

            return Ok(finalResult);
        }

        [HttpGet("getallroles")]
        public IActionResult GetAllRoles()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _authService.GetAllRoles();

            if (result.Success) return Ok(result.Data);

            return Problem(result.Message);
        }
    }
}
