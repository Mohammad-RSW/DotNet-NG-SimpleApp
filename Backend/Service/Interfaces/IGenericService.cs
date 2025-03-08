namespace Service.Interfaces
{
    public interface IGenericService<T>
    {
        Task<ServiceResult<IEnumerable<T>>> GetAll();
        Task<ServiceResult<T>> GetById(int id);
        Task<ServiceResult<int>> Create(T createModel);
        Task<ServiceResult<bool>> Update(T updateModel);
        Task<ServiceResult<bool>> DeleteById(int id);
    }
}
