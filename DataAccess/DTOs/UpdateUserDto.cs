using System.ComponentModel.DataAnnotations;

namespace DataAccess.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Role can not be empty")]
        public string? RoleName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter your old password")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        [Required(ErrorMessage = "Password is required")]
        public string? ConfirmNewPassword { get; set; }
    }

    public class ChangeRoleDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string? CurrentRole { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string? NewRole { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }

    
}
