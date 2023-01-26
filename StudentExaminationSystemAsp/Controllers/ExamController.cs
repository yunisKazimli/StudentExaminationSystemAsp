using Business.Interface.Examination;
using Business.Interface.Identity;
using Entities.Concrete.Examination;
using Entities.DTOs.Examination;
using Entities.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace StudentExaminationSystemAsp.Controllers
{
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpPost("addgroup")]
        public IActionResult AddGroup(string jsonData/*GroupDTO group*/)
        {
            GroupDTO group = JsonSerializer.Deserialize<GroupDTO>(jsonData);

            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _examService.AddGroup(group);

            if (result.Success) return Ok(result.Message);

            return Problem(result.Message);
        }

        [HttpPost("addusertogroup")]
        public IActionResult AddUserToGroup(string jsonData/*UserToGroupDTO userToGroup*/)
        {
            UserToGroupDTO userToGroup = JsonSerializer.Deserialize<UserToGroupDTO>(jsonData);

            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _examService.AddUserToGroup(userToGroup);

            if (result.Success) return Ok(result.Message);

            return Problem(result.Message);
        }

        [HttpPost("addquestion")]
        public IActionResult AddQuestions(string jsonData/*QuestionDTO questions*/)
        {
            QuestionDTO questions = JsonSerializer.Deserialize<QuestionDTO>(jsonData);

            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role == "Student") return Forbid();

            var result = _examService.AddQuestions(new QuestionDTO[] { questions }.ToList());

            if (result.Success) return Ok(result.Message);

            return Problem(result.Message);
        }

        [HttpPost("addstudentanswer")]
        public IActionResult AddStudentAnswer(string jsonData/*StudentAnswerDTO studentAnswer*/)
        {
            StudentAnswerDTO studentAnswer = JsonSerializer.Deserialize<StudentAnswerDTO>(jsonData);

            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);

            var result = _examService.AddStudentAnswer(studentAnswer, jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid").Value);

            if (result.Success) return Ok(result.Message);

            return Problem(result.Message);
        }

        [HttpGet("deletegroupbyid")]
        public IActionResult DeleteUserById(Guid groupId)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _examService.DeleteGroup(groupId);

            if (result.Success) return Ok(result.Message);

            return Problem(result.Message);
        }

        [HttpGet("deleteuserfromgroup")]
        public IActionResult DeleteUserFromGroup(UserToGroupDTO userToGroup)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _examService.DeleteUserFromGroup(userToGroup);

            if (result.Success) return Ok(result.Message);

            return Problem(result.Message);
        }

        [HttpGet("getallgroups")]
        public IActionResult GetAllGroups()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _examService.GetAllGroups();

            if (result.Success) return Ok(result.Data);

            return Problem(result.Message);
        }

        [HttpGet("getallgroupsbyinstructorid")]
        public IActionResult GetAllGroupsByInstructorId()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role == "Student") return Forbid();

            var result = _examService.GetAllGroupsByInstructorId(new Guid(jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid").Value));

            if (result.Success) return Ok(result.Data);

            return Problem(result.Message);
        }

        [HttpGet("getallquestions")]
        public IActionResult GetAllQuestions(string jsonData/*Guid GroupId*/)
        {
            Guid GroupId = JsonSerializer.Deserialize<Guid>(jsonData);

            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            var result = _examService.GetAllQuestionsInGroup(GroupId);

            if (result.Success) return Ok(result.Data);

            return Problem(result.Message);
        }

        [HttpGet("getallstudentanswers")]
        public IActionResult GetAllStudentAnswers(string jsonData/*AllStudentAnswersIdsDTO stdudentAnswersIds*/)
        {
            AllStudentAnswersIdsDTO stdudentAnswersIds = JsonSerializer.Deserialize<AllStudentAnswersIdsDTO>(jsonData);

            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            var result = _examService.GetAllStudentAnswers(stdudentAnswersIds.GroupId, stdudentAnswersIds.StudentId);

            if (result.Success) return Ok(result.Data);

            return Problem(result.Message);
        }
    }
}
