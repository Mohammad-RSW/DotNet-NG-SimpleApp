using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.Users
{   
    public class UserRegisterDto
    {
        [Required]
        //[StringLength(32, MinimumLength = 5)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        //[StringLength(254)]
        public required string Email { get; set; }

        [Required]
        //[StringLength(16, MinimumLength = 8)]
        public required string Password { get; set; }

        [Required]
        //[Compare("Password")]
        public required string ConfirmPassword { get; set; }

        [StringLength(100)]
        public string FullName { get; set; } = "";

        public string? Avatar { get; set; }
    }
}
