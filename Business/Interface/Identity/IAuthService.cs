using CorePackage.Helpers.Result.Abstract;
using Entities.DTOs.Examination.GetDTOs;
using Entities.DTOs.Identity;
using Entities.DTOs.Identity.GetDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface.Identity
{
    public interface IAuthService
    {
        IResult Register(RegisterDTO register);
        IResult DeleteUserById(Guid userId);
        IDataResult<string> Login(LoginDTO login);
        IDataResult<List<UserGetDTO>> GetAllUsers();
        IDataResult<List<RoleGetDTO>> GetAllRoles();
    }
}
