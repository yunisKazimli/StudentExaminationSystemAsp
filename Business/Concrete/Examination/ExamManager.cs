using Business.Interface.Examination;
using Business.Interface.Identity;
using CorePackage.Entities.Concrete;
using CorePackage.Helpers.Result.Abstract;
using CorePackage.Helpers.Result.Concrete;
using CorePackage.Helpers.Result.Concrete.ErrorResults;
using CorePackage.Helpers.Result.Concrete.SuccessResults;
using DataAccess.Interface.Examination;
using Entities.Concrete.Examination;
using Entities.DTOs.Examination;
using Entities.DTOs.Examination.GetDTOs;
using Entities.DTOs.Identity.GetDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.Examination
{
    public class ExamManager : IExamService
    {
        public readonly IExamDal _examDal;
        public readonly IAuthService _authService;

        public ExamManager(IExamDal examDal, IAuthService authService)
        {
            _examDal = examDal;
            _authService = authService;
        }

        public IResult AddGroup(GroupDTO group)
        {
            try
            {
                if (_examDal.Get<Group>(el => el.GroupName == group.GroupName) != null) throw new Exception("Group with this name already was created");

                Group newGroup = new Group()
                {
                    GroupId = Guid.NewGuid(),
                    GroupName = group.GroupName,
                    IsActive = true
                };

                _examDal.Add(newGroup);

                return new SuccessResult("Group created successfully");
            }
            catch(Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IResult AddQuestions(List<QuestionDTO> questions)
        {
            try
            {
                string particularError = "";

                int errorCount = 0;

                foreach(QuestionDTO qdto in questions)
                {
                    if (_examDal.Get<Question>(el => el.QuestionBody == qdto.QuestionBody && el.GroupId == new Guid(qdto.GroupId)) != null)
                    {
                        particularError += $"Question - \'{qdto.QuestionBody}\' is already added\n";
                        errorCount++;

                        continue;
                    }

                    Question newQuestion = new Question()
                    {
                        QuestionId = Guid.NewGuid(),
                        QuestionBody = qdto.QuestionBody,
                        GroupId = new Guid(qdto.GroupId),
                        Group = _examDal.Get<Group>(el => el.GroupId == new Guid(qdto.GroupId))
                    };

                    _examDal.Add(newQuestion);

                    if (qdto.Options.Count == 0)
                    {
                        Option newOption = new Option()
                        {
                            Id = Guid.NewGuid(),
                            Body = "empty",
                            QuestionId = newQuestion.QuestionId,
                            Question = _examDal.Get<Question>(el => el.QuestionId == newQuestion.QuestionId),
                            IsRight = true
                        };

                        newOption.Question.Group = _examDal.Get<Group>(el => el.GroupId == new Guid(qdto.GroupId));

                        _examDal.Add(newOption);
                    }

                    else
                    {
                        foreach (OptionDTO qdtoo in qdto.Options)
                        {
                            Option newOption = new Option()
                            {
                                Id = Guid.NewGuid(),
                                Body = qdtoo.Body,
                                QuestionId = newQuestion.QuestionId,
                                Question = _examDal.Get<Question>(el => el.QuestionId == newQuestion.QuestionId),
                                IsRight = qdtoo.IsRight
                            };

                            newOption.Question.Group = _examDal.Get<Group>(el => el.GroupId == new Guid(qdto.GroupId));

                            _examDal.Add(newOption);
                        }
                    }
                }

                if (errorCount == 0) return new SuccessResult("All questions were added successfully");
                else if (errorCount < questions.Count) return new SuccessResult("Some questions were added successfully but some was wrong.Follow list of errors\n" + particularError);
                throw new Exception("All questions are already in group");

            }
            catch(Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IResult AddStudentAnswer(StudentAnswerDTO studentAnswer, string studentId)
        {
            try
            {
                if (_examDal.Get<StudentAnswer>(el => el.UserId == new Guid(studentId) && el.QuestionId == new Guid(studentAnswer.QuestionId)) != null)
                    throw new Exception("This user is already answered to this question");

                StudentAnswer newStudentAnswer = new StudentAnswer()
                {
                    StudentAnswerId = Guid.NewGuid(),
                    QuestionId = new Guid(studentAnswer.QuestionId),
                    Question = _examDal.Get<Question>(el => el.QuestionId == new Guid(studentAnswer.QuestionId)),
                    UserId = new Guid(studentId),
                    StudentOpenAnswerBody = studentAnswer.AnswerBody == null || studentAnswer.AnswerBody == "" ? "empty" : studentAnswer.AnswerBody
                };

                _examDal.Add(newStudentAnswer);

                foreach(string odto in studentAnswer.OptionsId)
                {
                    StudentAnswerOption newStudentAnswerOption = new StudentAnswerOption()
                    {
                        StudentAnswerOptionId = Guid.NewGuid(),
                        OptionId = new Guid(odto),
                        Option = _examDal.Get<Option>(el => el.Id == new Guid(odto)),
                        StudentAnswerId = newStudentAnswer.StudentAnswerId,
                        StudentAnswer = _examDal.Get<StudentAnswer>(el => el.StudentAnswerId == newStudentAnswer.StudentAnswerId)
                    };

                    _examDal.Add(newStudentAnswerOption);
                }

                return new SuccessResult("Answers added successfully");
            }
            catch(Exception ex )
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IResult AddUserToGroup(UserToGroupDTO userToGroup)
        {
            try
            {
                if (_examDal.Get<UserGroup>(el => el.GroupId == new Guid(userToGroup.GroupId) && el.UserId == new Guid(userToGroup.UserId)) != null)
                    throw new Exception("This user is already in this group");

                UserGetDTO userGetDTO = _authService.GetAllUsers().Data.FirstOrDefault(el => el.UserId == new Guid(userToGroup.UserId));

                GroupGetDTO group = GetAllGroups().Data.FirstOrDefault(el => el.GroupId == new Guid(userToGroup.GroupId));

                if (userGetDTO.Role == "Instructor" && group.Instructor != null)
                {
                    DeleteUserFromGroup(new UserToGroupDTO() { GroupId = userToGroup.GroupId, UserId = group.Instructor.UserId.ToString() });
                }

                UserGroup newUserGroup = new UserGroup()
                {
                    UserGroupId = Guid.NewGuid(),
                    UserId = new Guid(userToGroup.UserId),
                    GroupId = new Guid(userToGroup.GroupId),
                    Group = _examDal.Get<Group>(el => el.GroupId == new Guid(userToGroup.GroupId))
                };

                _examDal.Add(newUserGroup);

                return new SuccessResult("User added to group successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IResult DeleteGroup(Guid groupId)
        {
            try
            {
                Group group = _examDal.Get<Group>(el => el.GroupId == groupId);

                _examDal.Delete(group);

                List<UserGroup> userGroup = _examDal.GetSome<UserGroup>(el => el.GroupId == groupId);

                foreach(UserGroup ug in userGroup) _examDal.Delete(ug);

                return new SuccessResult("Group deleted successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IResult DeleteQuestion(DeleteQuestionDTO deleteQuestion)
        {
            try
            {
                Question foundQuestion = _examDal.Get<Question>(el => el.GroupId == deleteQuestion.GroupId && el.QuestionId == deleteQuestion.QuestionId);

                _examDal.Delete(foundQuestion);
                
                return new SuccessResult("Question deleted successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IResult DeleteUserFromGroup(UserToGroupDTO userToGroup)
        {
            try
            {
                UserGroup userGroup = _examDal.Get<UserGroup>(el => el.GroupId == new Guid(userToGroup.GroupId) && el.UserId == new Guid(userToGroup.UserId));

                _examDal.Delete(userGroup);

                return new SuccessResult("User deleted from group successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IResult DeleteUserGroupByUserId(Guid userId)
        {
            try
            {
                UserGroup userGroup = _examDal.Get<UserGroup>(el => el.UserId == userId);

                _examDal.Delete(userGroup);

                return new SuccessResult("UserGroup deleted successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IDataResult<List<GroupGetDTO>> GetAllGroups()
        {
            try
            {
                List<Group> groups = _examDal.GetSome<Group>(el => true);

                List<UserGroup> userGroups = _examDal.GetSome<UserGroup>(el => true);

                List<UserGetDTO> allUsers = _authService.GetAllUsers().Data;

                List<GroupGetDTO> groupGetModel = new List<GroupGetDTO>();

                for(int i = 0; i < groups.Count; i++)
                {
                    GroupGetDTO newGroup = new GroupGetDTO()
                    {
                        GroupId = groups[i].GroupId,
                        GroupName = groups[i].GroupName,
                        Students = new List<UserGetDTO>()
                    };

                    groupGetModel.Add(newGroup);
                }

                for(int i = 0;i < userGroups.Count;i++)
                {
                    UserGetDTO user = allUsers.FirstOrDefault(el => el.UserId == userGroups[i].UserId);

                    GroupGetDTO group = groupGetModel.FirstOrDefault(el => el.GroupId == userGroups[i].GroupId);

                    if (user.Role == "Instructor")
                        groupGetModel.Find(el => el == group).Instructor = user;
                    else
                        groupGetModel.Find(el => el == group).Students.Add(user);
                }

                return new SuccessDataResult<List<GroupGetDTO>>(groupGetModel);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<List<GroupGetDTO>>(ex.Message);
            }
        }

        public IDataResult<List<GroupGetDTO>> GetAllGroupsByInstructorId(Guid instructorId)
        {
            try
            {
                List<Group> groups = _examDal.GetSome<Group>(el => true);

                List<UserGroup> userGroups = _examDal.GetSome<UserGroup>(el => true);

                List<UserGetDTO> allUsers = _authService.GetAllUsers().Data;

                List<GroupGetDTO> groupGetModel = new List<GroupGetDTO>();

                for (int i = 0; i < userGroups.Count; i++)
                {
                    if (groupGetModel.Where(el => el.GroupId == userGroups[i].GroupId).Count() == 0)
                    {
                        groupGetModel.Add(new GroupGetDTO());

                        groupGetModel[groupGetModel.Count - 1].GroupId = userGroups[i].GroupId;
                        groupGetModel[groupGetModel.Count - 1].GroupName = groups.FirstOrDefault(el => el.GroupId == userGroups[i].GroupId).GroupName;
                        groupGetModel[groupGetModel.Count - 1].Students = new List<UserGetDTO>();
                    }

                    UserGetDTO user = allUsers.FirstOrDefault(el => el.UserId == userGroups[i].UserId);

                    GroupGetDTO group = groupGetModel.FirstOrDefault(el => el.GroupId == userGroups[i].GroupId);

                    if (user.Role == "Instructor")
                        groupGetModel.Find(el => el == group).Instructor = user;
                    else
                        groupGetModel.Find(el => el == group).Students.Add(user);
                }

                groupGetModel = groupGetModel.Where(el => el.Instructor != null && el.Instructor.UserId == instructorId).ToList();

                return new SuccessDataResult<List<GroupGetDTO>>(groupGetModel);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<GroupGetDTO>>(ex.Message);
            }
        }

        public IDataResult<List<QuestionGetDTO>> GetAllQuestionsInGroup(Guid GroupId)
        {
            try
            {
                List<QuestionGetDTO> questionsGetModel = new List<QuestionGetDTO>();

                List<Question> questions = _examDal.GetSome<Question>(el => el.GroupId == GroupId);

                List<Option> options = _examDal.GetSome<Option>(el => el.Question.Group.GroupId == GroupId);

                for(int i = 0; i < questions.Count; i++)
                {
                    questionsGetModel.Add
                        (
                            new QuestionGetDTO()
                            {
                                QuestionId = questions[i].QuestionId,
                                GroupId = GroupId,
                                QuestionBody = questions[i].QuestionBody,
                                Options = 
                                    (
                                        from el
                                        in options.Where(el1 => el1.QuestionId == questions[i].QuestionId)
                                        select new OptionGetDTO()
                                        {
                                            OptionId = el.Id,
                                            OptionBody = el.Body,
                                            IsRight = el.IsRight
                                        }
                                    ).ToList()
                            }
                        );
                }

                return new SuccessDataResult<List<QuestionGetDTO>>(questionsGetModel);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<List<QuestionGetDTO>>(ex.Message);
            }
        }

        public IDataResult<List<StudentAnswerGetDTO>> GetAllStudentAnswers(Guid GroupId, Guid StudentId)
        {
            try
            {
                List<StudentAnswerGetDTO> studentAnswerGetModel = new List<StudentAnswerGetDTO>();

                List<StudentAnswer> studentAnswers = _examDal.GetSome<StudentAnswer>(el => el.Question.GroupId == GroupId && el.UserId == StudentId);

                List<StudentAnswerOption> studentAnswerOptions = _examDal.GetSome<StudentAnswerOption>(el => el.StudentAnswer.Question.GroupId == GroupId && el.StudentAnswer.UserId == StudentId);

                List<Option> options = _examDal.GetSome<Option>(el => el.Question.GroupId == GroupId);

                for(int i = 0; i < studentAnswers.Count; i++)
                {
                    studentAnswerGetModel.Add
                        (
                            new StudentAnswerGetDTO()
                            {
                                StudentAnswerId = studentAnswers[i].StudentAnswerId,
                                GroupId = GroupId,
                                UserId = StudentId,
                                QuestionId = studentAnswers[i].QuestionId,
                                Options =
                                    (
                                        from el
                                        in studentAnswerOptions.Where(el => el.StudentAnswerId == studentAnswers[i].StudentAnswerId)
                                        select new OptionGetDTO()
                                        {
                                            OptionId = el.OptionId,
                                            OptionBody = options.FirstOrDefault(el1 => el1.Id == el.OptionId).Body,
                                            IsRight = options.FirstOrDefault(el1 => el1.Id == el.OptionId).IsRight
                                        }
                                    ).ToList(),
                                OpenAnswer = studentAnswers[i].StudentOpenAnswerBody
                            }
                        );
                }

                return new SuccessDataResult<List<StudentAnswerGetDTO>>(studentAnswerGetModel);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<List<StudentAnswerGetDTO>>(ex.Message);
            }
        }
    }
}
