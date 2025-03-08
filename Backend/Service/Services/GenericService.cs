using Microsoft.Data.SqlClient;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Utility;

namespace Service.Services
{
    public class GenericService<T> : IGenericService<T>
    {
        private readonly IGenericRepository<T> _genericRepository;

        public GenericService(IGenericRepository<T> genericRepository)    
        {
            _genericRepository = genericRepository;
        }

        public async Task<ServiceResult<IEnumerable<T>>> GetAll() 
        {
            return await ExceptionHandler.ExecuteWithHandlingAsync<IEnumerable<T>>(
                async () => { return await _genericRepository.GetAllAsync(); }, 200);
        }

        public async Task<ServiceResult<T>> GetById(int id) 
        {
            return await ExceptionHandler.ExecuteWithHandlingAsync<T>(
                async () => { return await _genericRepository.GetByIdAsync(id); }, 200);
        }

        public async Task<ServiceResult<bool>> DeleteById(int id)
        {
            return await ExceptionHandler.ExecuteWithHandlingAsync<bool>(
                async () => { return await _genericRepository.DeleteByIdAsync(id); }, 204);
        }

        public async Task<ServiceResult<int>> Create(T createModel) 
        {
            return await ExceptionHandler.ExecuteWithHandlingAsync<int>(
                async () => { return await _genericRepository.CreateAsync(createModel); }, 201);
        }

        public async Task<ServiceResult<bool>> Update(T updateModel) 
        {

            return await ExceptionHandler.ExecuteWithHandlingAsync<bool>(
                async () => { return await _genericRepository.UpdateAsync(updateModel); }, 200);
        }
    }
}
