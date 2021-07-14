using Microsoft.EntityFrameworkCore;
using Sushi.Room.Domain.AggregatesModel.UserAggregate;
using Sushi.Room.Domain.Extensions;
using Sushi.Room.Infrastructure.EntityTypeConfigurations;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace Sushi.Room.Infrastructure
{
    public class SushiRoomDbContext : DbContext
    {
        public SushiRoomDbContext(DbContextOptions<SushiRoomDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
        }
    }
    
    public class SushiRoomDbContextDesignFactory : IDesignTimeDbContextFactory<SushiRoomDbContext>
    {
        public SushiRoomDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SushiRoomDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=SushiRoomDb;User Id=postgres;Password=1qaz!QAZ");

            return new SushiRoomDbContext(optionsBuilder.Options);
        }
    }

    public static class DbInitializer
    {
        public static async Task Initialize(SushiRoomDbContext context)
        {
            if (!await context.Set<User>().AnyAsync())
            {
                var user = User.CreateNew("admin", "1qaz!QAZ".ToSha256(), "admin", "admin", true);
                user.DateOfCreate = DateTimeOffset.UtcNow;

                await context.AddAsync(user);

                await context.SaveChangesAsync();
            }
        }
    }
}
