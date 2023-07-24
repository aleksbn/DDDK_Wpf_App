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

    public class RegisterDTO : LoginDTO
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

    public class UpdateModeratorDTO : RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string OldEmail { get; set; }
    }

    public class User : LoginDTO
    {

    }

    public class LoginResponseDTO
    {
        public string token { get; set; }
        public string[] role { get; set; }
    }
}
