namespace blogg_api.Services
{
    public interface IAppRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetSingleAsync(int Id);
        Task<T> UpdateAsync(T newEntity);
        Task<T> DeleteAsync(int Id);
        Task<T> AddAsync(T newEntity);
    }
}
