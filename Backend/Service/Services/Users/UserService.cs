using Service.Interfaces.Users;
using Repository.Models.Users;
using Repository.Interfaces.Users;
using System.Security.Cryptography;
using System.Text;
using Service.Utility;
using Service.DTOs.Users;

namespace Service.Services.Users
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ExceptionHandler _exceptionHandler;

        public UserService(
            IUserRepository userRepository, ServiceValidator serviceValidator, ExceptionHandler exceptionHandler) : base(userRepository)
        {
            _userRepository = userRepository;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<ServiceResult<IEnumerable<UserProfileSummary>>> GetAllProfiles()
        {
            return await ExceptionHandler.ExecuteWithHandlingAsync<IEnumerable<UserProfileSummary>>(
                async () => { return await _userRepository.GetAllProfiles(); }, 200);
        }

        public async Task<ServiceResult<UserProfileDetailed>> GetUserProfileDetailedById(int id)
        {
            return await ExceptionHandler.ExecuteWithHandlingAsync<UserProfileDetailed>(
                async () => { return await _userRepository.GetUserProfileDetailedByIdAsync(id); }, 200);
        }

        public async Task<ServiceResult<UserProfileSummary>> GetUserProfileSummaryByUsername(string username)
        {
            return await ExceptionHandler.ExecuteWithHandlingAsync<UserProfileSummary>(
                async () => { return await _userRepository.GetUserProfileSummaryByUsernameAsync(username); }, 200);
        }

        public async Task<ServiceResult<int>> RegisterUser(UserRegisterDto userRegisterDto)
        {
            Dictionary<string, List<string>> errors = ServiceValidator.ValidateUserCredentials(
                userRegisterDto.UserName, userRegisterDto.Email, userRegisterDto.Password, userRegisterDto.ConfirmPassword);

            if (!(errors.Values.All(list => list.Count == 0)))
            {
                return ServiceResult<int>.ValidationFailure(errors, 400);
            }

            try
            {
                if (!ServiceValidator.IsUsernameAndEmailUnique(userRegisterDto.UserName, userRegisterDto.Email, null, _userRepository))
                {
                    return ServiceResult<int>.Failure("Username or email already taken!", 400);
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Failure($"An unexpected error occurred: {ex.Message}", 500);
            }

            byte[] hashedPW = MD5.HashData(Encoding.UTF8.GetBytes(userRegisterDto.Password));

            if (string.IsNullOrWhiteSpace(userRegisterDto.Avatar))
            {
                userRegisterDto.Avatar = "DefaultAvatar.png";
            }
            else
            {
                // Fix for other image types
                string avatarUploadPath = @$"{Path.Combine(Directory.GetCurrentDirectory(), @"Static\Images\")}{userRegisterDto.UserName.ToLower()}.png";

                byte[] avatarBinaryData = Convert.FromBase64String(userRegisterDto.Avatar.Replace("data:image/png;base64,", ""));

                File.WriteAllBytes(avatarUploadPath, avatarBinaryData);

                userRegisterDto.Avatar = $"{userRegisterDto.UserName.ToLower()}.png";
            }

            User user = new()
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                Password = hashedPW,
                FullName = userRegisterDto.FullName,
                Avatar = userRegisterDto.Avatar,
            };

            return await ExceptionHandler.ExecuteWithHandlingAsync<int>(async () => { return await _userRepository.CreateAsync(user);}, 201);
        }

        public async Task<ServiceResult<bool>> EditUser(UserEditDto userEditDto)
        {
            Dictionary<string, List<string>> errors = ServiceValidator.ValidateUserCredentials(
                userEditDto.UserName, userEditDto.Email);

            if (!(errors.Values.All(list => list.Count == 0)))
            {
                return ServiceResult<bool>.ValidationFailure(errors, 400);
            }

            if (!string.IsNullOrWhiteSpace(userEditDto.UserName) || !string.IsNullOrWhiteSpace(userEditDto.Email))
            {
                try
                {
                    if (!ServiceValidator.IsUsernameAndEmailUnique(userEditDto.UserName, userEditDto.Email, userEditDto.Id, _userRepository))
                    {
                        return ServiceResult<bool>.Failure("Username or email already taken!", 400);
                    }
                }
                catch (Exception ex)
                {
                    return ServiceResult<bool>.Failure($"An unexpected error occurred: {ex.Message}", 500);
                }
            }

            User user = await _userRepository.GetByIdAsync(userEditDto.Id);

            if (!string.IsNullOrWhiteSpace(userEditDto.UserName) && user.UserName != userEditDto.UserName)
            {
                user.UserName = userEditDto.UserName;
            }

            if (!string.IsNullOrWhiteSpace(userEditDto.Email) && user.Email != userEditDto.Email)
            {
                user.Email = userEditDto.Email;
            }

            if (!string.IsNullOrWhiteSpace(userEditDto.FullName) && user.FullName != userEditDto.FullName)
            {
                user.FullName = userEditDto.FullName;
            }

            if (!string.IsNullOrWhiteSpace(userEditDto.Avatar))
            {
                // Fix for other image types
                string avatarUploadPath = @$"{Path.Combine(Directory.GetCurrentDirectory(), @"Static\Images\")}{user.UserName.ToLower()}.png";

                byte[] avatarBinaryData = Convert.FromBase64String(userEditDto.Avatar.Replace("data:image/png;base64,", ""));

                // if userEditDto.Avatar && user.Username != userEditDto.Username => File.Delete(old_avatar)? (Atomic With Write)
                File.WriteAllBytes(avatarUploadPath, avatarBinaryData);

                user.Avatar = $"{user.UserName.ToLower()}.png";
            }

            user.UpdatedAt = DateTime.UtcNow;

            return await ExceptionHandler.ExecuteWithHandlingAsync<bool>(async () => { return await _userRepository.UpdateAsync(user); }, 200);
        }

        public async Task<ServiceResult<string>> LoginUser(UserLoginDto userLoginDto)
        {
            byte[] hashedPW = MD5.HashData(Encoding.UTF8.GetBytes(userLoginDto.Password));

            var result = await ExceptionHandler.ExecuteWithHandlingAsync<string>(
                async () => { return await _userRepository.LoginUserAsync(userLoginDto.UserName, hashedPW); }, 200);
            int userId = 0;

            if (result.IsSuccess)
            {
                if (!int.TryParse(result.Data, out userId))
                {
                    userId = 0;
                }
            }

            if (userId == 0)
            {
                result.IsSuccess = false;
                result.Data = null;
                result.StatusCode = 404;
                result.ErrorMessage = "Username and password do not match!";

                return result;
            }

            result.Data = Token.Generate(userId);

            return result;
        }

        public async Task<ServiceResult<bool>> DeactivateUser(int id) 
        {
            return await ExceptionHandler.ExecuteWithHandlingAsync<bool>(
                async () => { return await _userRepository.DeactivateUserByIdAsync(id);}, 200);
        }

        public async Task<ServiceResult<bool>> ActivateUser(UserActivateDto userActivateDto)
        {
            byte[] hashedPW = MD5.HashData(Encoding.UTF8.GetBytes(userActivateDto.Password));

            return await ExceptionHandler.ExecuteWithHandlingAsync<bool>(
                async () => { return await _userRepository.ActivateUserAsync(userActivateDto.UserName, hashedPW); }, 200);
        }
    }
}
