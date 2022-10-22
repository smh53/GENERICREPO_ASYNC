using Core.Utilities.Results;
using DataAccess.DTOs;

using DataAccess.Entities.User;
using DataAccess.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService 
    {
        Task<IDataResult<LoginResponseDto>> Login(LoginDto loginUser);
        Task<IResult> Logout();
        Task<IDataResult<IdentityResult>> ConfirmEmail(string token, string email);
        Task<IResult> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<IDataResult<RegisterResponseDto>> Register(RegisterDto registerUser);

        Task<IDataResult<IdentityResult>> CreateRole(IdentityRole role);

        Task<IResult> ChangePassword(ChangePasswordDto updatePassword);
        Task<IDataResult<string>> GenerateResetPasswordToken(ResetPasswordDto resetPassword);
        Task<IResult> UpdateUser(UpdateUserDto updateUser);
        Task<IResult> ChangeRole(ChangeRoleDto updaterole);

        SuccessDataResult<List<string>> GetAllRoles();
    }
}
