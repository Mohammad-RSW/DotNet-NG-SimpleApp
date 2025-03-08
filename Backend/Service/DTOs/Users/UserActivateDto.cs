using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.Users
{
    public class UserActivateDto
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
