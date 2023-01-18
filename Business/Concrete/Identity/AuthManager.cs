using Business.Interface.Identity;
using CorePackage.Entities.Concrete;
using CorePackage.Helpers.Result.Abstract;
using CorePackage.Helpers.Result.Concrete.ErrorResults;
using CorePackage.Helpers.Result.Concrete.SuccessResults;
using CorePackage.Security.Hashing;
using CorePackage.Security.Jwt;
using DataAccess.Interface.Identity;
using DnsClient;
using Entities.DTOs.Examination.GetDTOs;
using Entities.DTOs.Identity;
using MongoDB.Driver.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.Identity
{
    public class AuthManager : IAuthService
    {
        private readonly IAuthDal _authDal;

        public AuthManager(IAuthDal authDal)
        {
            _authDal = authDal;
        }

        public IDataResult<List<Entities.DTOs.Identity.GetDTOs.UserGetDTO>> GetAllUsers()
        {
            try
            {
                List<User> users = _authDal.GetSome<User>(el => true);
                List<UserRole> userRoles = _authDal.GetSome<UserRole>(el => true);
                List<Role> roles = _authDal.GetSome<Role>(el => true);

                List<Entities.DTOs.Identity.GetDTOs.UserGetDTO> usersGetModel =
                    (
                        from el
                        in users
                        select new Entities.DTOs.Identity.GetDTOs.UserGetDTO()
                        {
                            UserId = el.UserId,
                            UserName = el.UserName,
                            Role = roles.FirstOrDefault(el1 => el1.RoleId == userRoles.Where(el1 => el1.UserRoleId == el.UserRoleId).FirstOrDefault().RoleId).RoleName
                        }
                    ).ToList();

                return new SuccessDataResult<List<Entities.DTOs.Identity.GetDTOs.UserGetDTO>>(usersGetModel);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Entities.DTOs.Identity.GetDTOs.UserGetDTO>>(ex.Message); 
            }
        }

        public IDataResult<string> Login(LoginDTO login)
        {
            try
            {
                User user = _authDal.Get<User>(el => el.UserName == login.Username);

                if (user == null) throw new Exception("This username not found");

                if (!HashingHelper.VerifyPassword(login.Password, user.PasswordHash, user.PasswordSalt)) throw new Exception("Username or password is wrong");

                user.UserRole = _authDal.Get<UserRole>(el => el.UserRoleId == user.UserRoleId);

                user.UserRole.Role = _authDal.Get<Role>(el => el.RoleId == user.UserRole.RoleId);

                return new SuccessDataResult<string>(user.UserRole.Role.RoleName + " " + TokenGenerator.Token(user, user.UserRole.Role.RoleName));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<string>(e.Message);
            }
        }

        public IResult Register(RegisterDTO register)
        {
            try
            {
                if (register.Password != register.PasswordConfirm) throw new Exception("Password and password confirm are not match");

                if (_authDal.Get<User>(el => el.UserName == register.Username) != null) throw new Exception("User with this username already was added");

                byte[] hash;
                byte[] hashSalt;

                HashingHelper.HashPassword(register.Password, out hash, out hashSalt);

                UserRole newUserRole = new UserRole()
                {
                    UserRoleId = Guid.NewGuid(),
                    RoleId = new Guid(register.RoleId),
                    Role = _authDal.Get<Role>(el => el.RoleId == new Guid(register.RoleId))
                };

                _authDal.Add(newUserRole);

                User newUser = new User()
                {
                    UserId = Guid.NewGuid(),
                    UserName = register.Username,
                    PasswordHash = hash,
                    PasswordSalt = hashSalt,
                    UserRoleId = newUserRole.UserRoleId,
                    UserRole = newUserRole
                };

                _authDal.Add(newUser);

                return new SuccessResult("User reegistered successfully");
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }
    }
}
