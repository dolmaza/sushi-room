using System.Threading.Tasks;

namespace Sushi.Room.Domain.SeedWork
{
    public interface IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        Task AddAsync(TEntity entity);

        Task<TEntity> FindByIdAsync(int id);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        Task<bool> SaveChangesAsync();
    }
}