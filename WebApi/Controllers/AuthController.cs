using Business.Abstract;
using Core.Utilities.Mail;
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
        private readonly IMailService _mailService;
        public AuthController(IAuthService authService, IMailService mailService)
        {
            _authService = authService;
            _mailService = mailService;
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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto userForRegistration)
        {
            var result = _authService.Register(userForRegistration).Result;

            if (result.Success && userForRegistration.Email != null)
            {
                var confirmationLink = Url.Action(nameof(ConfirmEmail), ControllerContext.ActionDescriptor.ControllerName, new { token = result.Data.EmailConfirmationToken, email = userForRegistration.Email }, Request.Scheme);
                var mail = new MailRequest()
                {
                    Subject = MailConstants.RegistrationSubject,
                    Body = MailConstants.RegistrationBodyText + confirmationLink,
                    ToEmail = userForRegistration.Email
                };
                 await _mailService.SendEmailAsync(mail);
                return Ok(result);
            }
            return BadRequest(result);


        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
           
            var result = await _authService.ConfirmEmail(token,email);
            return Ok(result.Message);
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

        [HttpGet("GetAllRoles")]
            public IActionResult GetAllRoles()
        {
            var result = _authService.GetAllRoles();
            return Ok(result);
        }

        [HttpPost("SendPasswordResetEmail")]
        public async Task<IActionResult> SendPasswordResetEmail([FromBody] ResetPasswordDto resetPassword)
        {
            var resetPasswordToken = await _authService.GenerateResetPasswordToken(resetPassword);

            if (resetPasswordToken.Success && resetPassword.Email != null)
            {
                var resetPasswordLink = "http://localhost:4200/auth/reset-password";
                var mail = new MailRequest()
                {
                    Subject = MailConstants.ResetPasswordSubject,
                    Body = MailConstants.ResetPasswordBodyText + resetPasswordLink + " Your password reset code: " + resetPasswordToken.Data,
                    
                    ToEmail = resetPassword.Email
                };
                await _mailService.SendEmailAsync(mail);
                return Ok(mail.Body);
            }
            return BadRequest(resetPasswordToken.Message);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {

            var result = await _authService.ResetPassword(resetPasswordDto);
            return Ok(result);
        }

        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {

            var result = await _authService.Logout();
            return Ok(result);
        }
    }
}
