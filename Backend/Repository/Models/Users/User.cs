using System.ComponentModel.DataAnnotations;

namespace Repository.Models.Users
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 5)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public required string Email { get; set; }

        [Required]
        [MaxLength(16)]
        public required byte[] Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; } = "";

        [StringLength(100)]
        public string? Avatar { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = null;

        public DateTime? LastLogin { get; set; } = null;

        public bool IsActive { get; set; } = true;

        public bool IsVerified { get; set; } = false;

        public bool IsAdmin { get; set; } = false;
    }
}
