using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces.Users;
using Service.DTOs.Users;
using System.Text.RegularExpressions;

namespace Service.Utility
{
    public class ServiceValidator
    {
        public static List<string> ValidateUserName(string username)
        {
            List<string> errors = new();

            if (username.Length < 5 || username.Length > 32)
            {
                errors.Add("Username must be between 5 and 32 characters.");
            }

            if (username.Contains('@') || username.Contains(' ') || username.Contains('-'))
            {
                errors.Add("Username cannot contain '@', '-' or space.");
            }

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                errors.Add("Username can only contain english letters, digits and underscores.");
            }

            return errors;
        }

        public static List<string> ValidatePassword(string password, string confirmPassword)
        {
            List<string> errors = new();

            if (!password.Equals(confirmPassword))
            {
                errors.Add("Passwords do not match.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password is required.");
            }

            if (password.Length < 8 || password.Length > 16)
            {
                errors.Add("Password must be between 8 and 16 characters.");
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errors.Add("Password must contain at least one lowercase letter.");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }

            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                errors.Add("Password must contain at least one digit.");
            }

            return errors;
        }

        public static List<string> ValidateEmail(string email)
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email is required.");
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                errors.Add("Invalid email format.");
            }

            if (email.Length > 254)
            {
                errors.Add("Email must be less than 254 characters.");
            }

            return errors;
        }

        public static Dictionary<string, List<string>> ValidateUserCredentials(
            string username, string email, string password, string confirmPassword)
        {
            Dictionary<string, List<string>> errors = new();

            if (!string.IsNullOrWhiteSpace(username))
            {
                errors.Add("UserName", ValidateUserName(username));
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email", ValidateEmail(email));
            }
            if (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(confirmPassword))
            {
                errors.Add("Password", ValidatePassword(password, confirmPassword));
            }

            return errors; 
        }

        public static Dictionary<string, List<string>> ValidateUserCredentials(
            string username, string email)
        {
            Dictionary<string, List<string>> errors = new();

            if (!string.IsNullOrWhiteSpace(username))
            {
                errors.Add("UserName", ValidateUserName(username));
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email", ValidateEmail(email));
            }

            return errors;
        }

        public static bool IsUsernameAndEmailUnique(string username, string email, int? selfId, IUserRepository _userRepository)
        {
            return _userRepository.IsUsernameAndEmailUnique(username, email, selfId).Result;
        }
    }
}
