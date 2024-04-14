
namespace Pharmacy.Shared
{
    public interface IService<Entity> where Entity : class
    {
        Task<bool> AddAsync(Entity entity);

        Task<bool> RemoveAsync(Entity entity);

        Task<bool> UpdateAsync(Entity entity);

        Task<Entity> GetByIdAsync(int id);

        Task<IEnumerable<Entity>> GetAllAsync();
    }
}