namespace Repository.Models.Users
{
    public class UserProfileSummary
    {
        public required string UserName { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
