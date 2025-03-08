using Microsoft.Data.SqlClient;

namespace Service.Utility
{
    public class ExceptionHandler
    {
        public static async Task<ServiceResult<T>> ExecuteWithHandlingAsync<T>(Func<Task<T>> func, int successStatus)
        {
            try
            {
                var result = await func();
                return ServiceResult<T>.Success(result, successStatus);
            }
            catch (SqlException ex)
            {
                return ServiceResult<T>.Failure($"SQL Error: {ex.Message}", 500);
            }
            catch (InvalidOperationException ex)
            {
                return ServiceResult<T>.Failure($"Invalid Operation: {ex.Message}", 400);
            }
            catch (ArgumentNullException ex)
            {
                return ServiceResult<T>.Failure($"Argument Null: {ex.Message}", 400);
            }
            catch (TaskCanceledException ex)
            {
                return ServiceResult<T>.Failure($"Task was canceled: {ex.Message}", 408);
            }
            catch (Exception ex)
            {
                return ServiceResult<T>.Failure($"An error occurred: {ex.Message}", 500);
            }
        }
    }
}