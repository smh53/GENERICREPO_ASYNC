using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Context;
using DataAccess.DTOs;
using DataAccess.Entities.User;
using DataAccess.EntityFramework;
using Microsoft.AspNetCore.Identity;
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
        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
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
            if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
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

                var token = GetToken(authClaims);

                return new SuccessDataResult<LoginResponseDto>(new LoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ValidTo = token.ValidTo,
                  
                });
            }
            return new ErrorDataResult<LoginResponseDto>();
        }

        public async Task<IDataResult<RegisterResponseDto>> Register(RegisterDto registerUser)
        {
            if (registerUser == null)
                return new ErrorDataResult<RegisterResponseDto>(new RegisterResponseDto { IsSuccessfulRegistration = false });

            var user = new User()
            {
                Name = registerUser.FirstName,
                Surname = registerUser.LastName,
                Email = registerUser.Email,
                UserName = registerUser.UserName
            };
            if (!await _roleManager.RoleExistsAsync(registerUser.RoleName))
            {

               await _roleManager.CreateAsync(new IdentityRole(registerUser.RoleName));

            }
           

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return new ErrorDataResult<RegisterResponseDto>(new RegisterResponseDto { Errors = errors });
            }
            await _userManager.AddToRoleAsync(user, registerUser.RoleName);
            return new SuccessDataResult<RegisterResponseDto>(new RegisterResponseDto { IsSuccessfulRegistration = true });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

    }





}
