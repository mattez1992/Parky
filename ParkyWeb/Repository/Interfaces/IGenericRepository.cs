
namespace ParkyWeb.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> AddAsync(string url, T entity, string token);
        Task<bool> Delete(string url, int id, string token);
        Task<IEnumerable<T>> GetAllAsync(string url, string token);
        Task<T> GetAsync(string url, int id, string token);
        Task<bool> UpdateAsync(string url, T updateObject, string token);
    }
}