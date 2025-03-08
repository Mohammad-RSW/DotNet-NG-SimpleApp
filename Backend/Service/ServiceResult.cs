using System.Runtime.CompilerServices;

namespace Service
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, List<string>>? ValidationErrors { get; set; }

        public static ServiceResult<T> Success(T data, int statusCode) => 
            new ServiceResult<T> { IsSuccess = true, Data = data, StatusCode = statusCode };

        public static ServiceResult<T> Failure(string errorMessage, int statusCode) => 
            new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage, ValidationErrors = null, StatusCode = statusCode };

        public static ServiceResult<T> ValidationFailure(Dictionary<string, List<string>> validationErrors, int statusCode) => 
            new ServiceResult<T> { IsSuccess = false, ValidationErrors = validationErrors, ErrorMessage = null, StatusCode = statusCode };
    }
}
