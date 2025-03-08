using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.Users
{
    public class UserLoginDto
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
