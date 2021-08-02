using Microsoft.EntityFrameworkCore;
using Sushi.Room.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sushi.Room.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        protected readonly SushiRoomDbContext Context;

        public Repository(SushiRoomDbContext context)
        {
            Context = context;
        }

        protected IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await Context.FindAsync<TEntity>(id);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            foreach (var item in Context.ChangeTracker.Entries<Entity>())
            {
                if (item.State == EntityState.Added)
                {
                    item.Entity.DateOfCreate = DateTimeOffset.UtcNow;
                }
            }

            await Context.SaveChangesAsync();

            return true;
        }
    }
}
