namespace Repository.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
        Task<int> CreateAsync(T createModel);
        Task<bool> UpdateAsync(T updateModel);
    }
}
