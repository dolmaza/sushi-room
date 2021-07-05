using Microsoft.EntityFrameworkCore;
using Sushi.Room.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sushi.Room.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        private readonly SushiRoomDbContext _context;

        public Repository(SushiRoomDbContext context)
        {
            _context = context;
        }

        protected IQueryable<TEntity> Query()
        {
            return _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await _context.FindAsync<TEntity>(id);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            foreach (var item in _context.ChangeTracker.Entries<Entity>())
            {
                if (item.State == EntityState.Added)
                {
                    item.Entity.DateOfCreate = DateTimeOffset.UtcNow;
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
