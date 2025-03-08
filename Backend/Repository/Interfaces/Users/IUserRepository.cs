using Repository.Models.Users;

namespace Repository.Interfaces.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<UserProfileSummary>> GetAllProfiles();
        Task<UserProfileDetailed> GetUserProfileDetailedByIdAsync(int id);
        Task<UserProfileSummary> GetUserProfileSummaryByUsernameAsync(string username);
        Task<string> LoginUserAsync(string username, byte[] hashedPW);
        Task<bool> DeactivateUserByIdAsync(int id);
        Task<bool> IsUsernameAndEmailUnique(string username, string email, int? selfId);
        Task<bool> ActivateUserAsync(string username, byte[] hashedPW);
    }
}
