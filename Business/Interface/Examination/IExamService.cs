using CorePackage.Helpers.Result.Abstract;
using Entities.DTOs.Examination;
using Entities.DTOs.Examination.GetDTOs;
using Entities.DTOs.Identity.GetDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface.Examination
{
    public interface IExamService
    {
        IResult AddGroup(GroupDTO group);
        IResult AddUserToGroup(UserToGroupDTO userToGroup);
        IResult AddQuestions(List<QuestionDTO> questions);
        IResult AddStudentAnswer(StudentAnswerDTO studentAnswer, string studentId);
        IResult DeleteGroup(Guid groupId);
        IResult DeleteUserGroupByUserId(Guid userId);
        IResult DeleteUserFromGroup(UserToGroupDTO userToGroup);
        IResult DeleteQuestion(DeleteQuestionDTO deleteQuestion);
        IDataResult<List<GroupGetDTO>> GetAllGroups();
        IDataResult<List<GroupGetDTO>> GetAllGroupsByInstructorId(Guid instructorId);
        IDataResult<List<GroupGetDTO>> GetAllGroupsByStudentId(Guid studentId);
        IDataResult<List<QuestionGetDTO>> GetAllQuestionsInGroup(Guid GroupId);
        IDataResult<List<StudentAnswerGetDTO>> GetAllStudentAnswers(Guid GroupId, Guid StudentId);
    }
}
