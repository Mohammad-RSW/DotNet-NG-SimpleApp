using Repository.Models.Users;
using Service.DTOs.Users;
using System.Threading.Tasks.Dataflow;

namespace Service.Interfaces.Users
{
    public interface IUserService : IGenericService<User>
    {
        Task<ServiceResult<IEnumerable<UserProfileSummary>>> GetAllProfiles();
        Task<ServiceResult<UserProfileDetailed>> GetUserProfileDetailedById(int id);
        Task<ServiceResult<UserProfileSummary>> GetUserProfileSummaryByUsername(string username);
        Task<ServiceResult<int>> RegisterUser(UserRegisterDto userRegisterDto);
        Task<ServiceResult<bool>> EditUser(UserEditDto userEditDto);
        Task<ServiceResult<string>> LoginUser(UserLoginDto userLoginDto);
        Task<ServiceResult<bool>> DeactivateUser(int id);
        Task<ServiceResult<bool>> ActivateUser(UserActivateDto userActivateDto);
    }
}
