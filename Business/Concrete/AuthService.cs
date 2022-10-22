using Business.Abstract;
using Business.Constants;
using Core.Utilities.Mail;
using Core.Utilities.Results;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.Entities.User;
using DataAccess.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
           
        }

        public async Task<IResult> ChangePassword(ChangePasswordDto updatePassword)
        {
            var user = await _userManager.FindByEmailAsync(updatePassword.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, updatePassword.OldPassword))
            {
               await _userManager.ChangePasswordAsync(user,updatePassword.OldPassword,updatePassword.NewPassword);
                return new SuccessResult(ResultMessages.AuthorizationMessages.SuccessChangePassword);
            }
            return new ErrorResult(ResultMessages.AuthorizationMessages.ErrorChangePassword);

        }
        
        public async Task<IResult> ChangeRole(ChangeRoleDto updaterole)
        {
            var user = await _userManager.FindByEmailAsync(updaterole.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, updaterole.Password))
            {
              await _userManager.RemoveFromRoleAsync(user, updaterole.CurrentRole);
              await _userManager.AddToRoleAsync(user, updaterole.NewRole);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public  SuccessDataResult<List<string>> GetAllRoles()
        {
            List<string> roles = _roleManager.Roles.Select(x => x.Name).ToList();
            return new SuccessDataResult<List<string>>(roles);
        }

        public async Task<IDataResult<IdentityResult>> CreateRole(IdentityRole role)
        {

            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                var result = await _roleManager.CreateAsync(role);
                return new SuccessDataResult<IdentityResult>(result);
            }

            return new ErrorDataResult<IdentityResult>();
        }


        public async Task<IDataResult<LoginResponseDto>> Login(LoginDto loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.UserName);
            var result = await _signInManager.PasswordSignInAsync(user, loginUser.Password, false, lockoutOnFailure: true);
            //if(user != null && await _userManager.IsEmailConfirmedAsync(user) && await _userManager.CheckPasswordAsync(user, loginUser.Password))
            if (result.Succeeded)
            {
                List<Claim> authClaims = await GetAuthClaims(user);

                var token = GetToken(authClaims);

                return new SuccessDataResult<LoginResponseDto>(new LoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ValidTo = token.ValidTo.ToLocalTime(),

                });
            }
            if(result.IsLockedOut)
            {
                return new ErrorDataResult<LoginResponseDto>("Your account is locked.");
            }
            if(result.IsNotAllowed)
            {
                return new ErrorDataResult<LoginResponseDto>("Your are not allowed to login.");
            }
            
            return new ErrorDataResult<LoginResponseDto>("Invalid login attempt.");
        }

        private async Task<List<Claim>> GetAuthClaims(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return authClaims;
        }

        public async Task<IDataResult<RegisterResponseDto>> Register(RegisterDto registerUser)
        {
            if (registerUser.FirstName == null || registerUser.LastName == null)
                return new ErrorDataResult<RegisterResponseDto>(new RegisterResponseDto { IsSuccessfulRegistration = false });
            
            var user = new User()
            {
                Name = registerUser.FirstName,
                Surname = registerUser.LastName,
                Email = registerUser.Email,
                UserName = registerUser.UserName
            };
            if (registerUser.RoleName == null)
                registerUser.RoleName = Roles.DefaultRole;

           

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return new ErrorDataResult<RegisterResponseDto>(new RegisterResponseDto { Errors = errors });
            }
            await _userManager.AddToRoleAsync(user, registerUser.RoleName);
            List<Claim> authClaims = await GetAuthClaims(user);
            var token = GetToken(authClaims);
            
            var emailActivationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
           
        
           

            return new SuccessDataResult<RegisterResponseDto>(new RegisterResponseDto { IsSuccessfulRegistration = true,Token = new JwtSecurityTokenHandler().WriteToken(token), EmailConfirmationToken = emailActivationToken });
        }

        public async Task<IResult> UpdateUser(UpdateUserDto updateUser)
        {
            if(updateUser.FirstName == null || updateUser.LastName == null)
                return new ErrorResult();
            var user = await _userManager.FindByEmailAsync(updateUser.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, updateUser.Password))
            {
                     
                user.UserName = updateUser.UserName;
                user.Name = updateUser.FirstName;
                user.Surname = updateUser.LastName;

                await _userManager.UpdateAsync(user);
                return new SuccessResult();
                
            }
            return new ErrorResult();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<IDataResult<IdentityResult>> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
                return new SuccessDataResult<IdentityResult>(result,"Email activated.");
            else
                return new ErrorDataResult<IdentityResult>(result,"Expired link. Try again.");
        }



        public async Task<IDataResult<string>> GenerateResetPasswordToken(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if(user != null)
            {
                var passwordResetToken =await _userManager.GeneratePasswordResetTokenAsync(user);

                return new SuccessDataResult<string>(data: passwordResetToken,message: "Successfull");
            }
            return new ErrorDataResult<string>();
        }

        public async Task<IResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if(user !=null)
            {   
               
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
                if (result.Succeeded)
                    return new SuccessResult("PASSWORD RESET COMPLETED.");
                else
                {
                    return new ErrorResult(result.Errors.FirstOrDefault().Description);
                }
            }
           
                return new ErrorResult("ERROR. Try again.");
        }

        public async Task<IResult> Logout()
        {          
            await _signInManager.SignOutAsync();
            return new SuccessResult();
        }
    }





}
