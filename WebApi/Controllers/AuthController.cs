using Business.Abstract;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("CreateRole")]
        public IActionResult CreateRole(IdentityRole role)
        {
            var result = _authService.CreateRole(role).Result;
            return Ok(result);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto userForAuthentication)
        {
            var result = _authService.Login(userForAuthentication).Result;
            return Ok(result);
        }

        [HttpPost("Registration")]
        public IActionResult RegisterUser([FromBody] RegisterDto userForRegistration)
        {
            var result = _authService.Register(userForRegistration).Result;
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);


        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUser)
        {
            var result = await _authService.UpdateUser(updateUser);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto updatePassword)
        {
            var result = await _authService.ChangePassword(updatePassword);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpPut("ChangeRole")]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleDto updateRole)
        {
            var result = await _authService.ChangeRole(updateRole);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }
    }
}
