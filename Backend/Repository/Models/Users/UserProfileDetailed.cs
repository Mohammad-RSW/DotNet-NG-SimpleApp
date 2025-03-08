namespace Repository.Models.Users
{
    public class UserProfileDetailed
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsVerified { get; set; }
    }
}
