using System.ComponentModel.DataAnnotations;

namespace DDDK_Wpf.DTOs
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class RegisterUserDTO : LoginDTO
    {
        [Required]
        public string Role { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
        [Required]
        [EmailAddress]
        public string EmailConfirmation { get; set; }
    }

    public class UpdateUserDTO : RegisterUserDTO
    {
        [Required]
        [EmailAddress]
        public string OldEmail { get; set; }
    }

    public class UserDTO
    {
        public string id { get; set; }
        public string email { get; set; }
        public string role { get; set; }

        public override string ToString()
        {
            return email + " - " + role;
        }
    }

    public class LoginResponseDTO
    {
        public string token { get; set; }
        public string[] role { get; set; }
    }
}
