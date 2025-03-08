using Dapper;
using Repository.Interfaces.Users;
using Repository.Models.Users;

namespace Repository.Repositories.Users
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DataAccess _dataAccess;

        public UserRepository(DataAccess dataAccess) : base(dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<UserProfileSummary>> GetAllProfiles()
        {
            string query = "SELECT UserName, FullName, Avatar, LastLogin FROM dbo.[User]";

            using (var conn = _dataAccess.GetConnection())
            {
                await conn.OpenAsync();
                return await conn.QueryAsync<UserProfileSummary>(query);
            }
        }

        public async Task<UserProfileDetailed> GetUserProfileDetailedByIdAsync(int id)
        {
            string query = "SELECT UserName, Email, FullName, Avatar, CreatedAt, LastLogin, IsVerified FROM dbo.[User] WHERE Id = @Id ";

            using (var conn = _dataAccess.GetConnection())
            {
                await conn.OpenAsync();
                return await conn.QuerySingleAsync<UserProfileDetailed>(query, new { Id = id });
            }
        }

        public async Task<UserProfileSummary> GetUserProfileSummaryByUsernameAsync(string username)
        {
            string query = "SELECT UserName, FullName, Avatar, LastLogin FROM dbo.[User] WHERE UserName = @Username ";

            using (var conn = _dataAccess.GetConnection())
            {
                await conn.OpenAsync();
                return await conn.QuerySingleAsync<UserProfileSummary>(query, new { Username = username });
            }
        }

        public async Task<string> LoginUserAsync(string username, byte[] hashedPW)
        {
            string query = "SELECT Id FROM dbo.[User] WHERE UserName = @Username AND Password = @Password";

            using (var conn = _dataAccess.GetConnection())
            {
                await conn.OpenAsync();
                return Convert.ToString(await conn.ExecuteScalarAsync<int>(query, new { Username = username, Password = hashedPW }));
            }
        }

        public async Task<bool> DeactivateUserByIdAsync(int id)
        {
            string query = "UPDATE dbo.[User] SET IsActive = 0 WHERE Id = @Id";

            using (var conn = _dataAccess.GetConnection())
            {
                await conn.OpenAsync();
                return await conn.ExecuteAsync(query, new { Id = id }) > 0;
            }
        }

        public async Task<bool> IsUsernameAndEmailUnique(string username, string email, int? selfId)
        {
            string query = "SELECT CASE WHEN COUNT(*) > 0 THEN 0 ELSE 1 END AS IsUnique FROM dbo.[User] WHERE UserName = @UserName OR Email = @Email";

            using (var conn = _dataAccess.GetConnection())
            {
                await conn.OpenAsync();

                if (selfId != null)
                {
                    query += " AND Id <> @Id";
                    return await conn.ExecuteScalarAsync<int>(query, new { UserName = username, Email = email, Id = selfId }) == 1;
                }

                return await conn.ExecuteScalarAsync<int>(query, new { UserName = username, Email = email }) == 1;
            }
        }

        public async Task<bool> ActivateUserAsync(string username, byte[] hashedPW)
        {
            string query = "UPDATE dbo.[User] SET IsActive = 1 WHERE UserName = @UserName AND Password = @Password";

            using (var conn = _dataAccess.GetConnection())
            {
                await conn.OpenAsync();
                return await conn.ExecuteAsync(query, new { UserName = username, Password = hashedPW }) > 0;
            }
        }
    }
}
