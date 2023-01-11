using Business.Interface.Examination;
using Business.Interface.Identity;
using Entities.Concrete.Examination;
using Entities.DTOs.Examination;
using Entities.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

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
        public IActionResult AddGroup(GroupDTO group)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _examService.AddGroup(group);

            if (result.Success) return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpPost("addusertogroup")]
        public IActionResult AddUserToGroup(UserToGroupDTO userToGroup)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role != "Admin") return Forbid();

            var result = _examService.AddUserToGroup(userToGroup);

            if (result.Success) return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpPost("addquestion")]
        public IActionResult AddQuestions(QuestionDTO questions, string[] optionBodyArray, bool[] isRightArray)
        {
            questions.Options = new List<OptionDTO>();

            for (int i = 0; i < optionBodyArray.Length; i++) questions.Options.Add(new OptionDTO() { Body = optionBodyArray[i], IsRight = isRightArray[i] });

            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            string role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            if (role == "Student") return Forbid();

            var result = _examService.AddQuestions(new QuestionDTO[] { questions }.ToList());

            if (result.Success) return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpPost("addstudentanswer")]
        public IActionResult AddStudentAnswer(StudentAnswerDTO studentAnswer)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);

            var result = _examService.AddStudentAnswer(studentAnswer, jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid").Value);

            if (result.Success) return Ok(result.Message);

            return BadRequest(result.Message);
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

            return BadRequest(result.Message);
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

            return BadRequest(result.Message);
        }

        [HttpGet("getallquestions")]
        public IActionResult GetAllQuestions(Guid GroupId)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            var result = _examService.GetAllQuestionsInGroup(GroupId);

            if (result.Success) return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet("getallstudentanswers")]
        public IActionResult GetAllStudentAnswers(Guid GroupId, Guid StudentId)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (token == null || token == "") return Unauthorized();

            var result = _examService.GetAllStudentAnswers(GroupId, StudentId);

            if (result.Success) return Ok(result.Data);

            return BadRequest(result.Message);
        }
    }
}
