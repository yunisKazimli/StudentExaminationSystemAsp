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
        IDataResult<string> Login(LoginDTO login);
        IDataResult<List<Entities.DTOs.Identity.GetDTOs.UserGetDTO>> GetAllUsers();
        IDataResult<List<RoleGetDTO>> GetAllRoles();
    }
}
