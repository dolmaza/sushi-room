using Microsoft.EntityFrameworkCore;
using Sushi.Room.Domain.AggregatesModel.UserAggregate;
using Sushi.Room.Domain.Extensions;
using Sushi.Room.Infrastructure.EntityTypeConfigurations;
using System;
using System.Threading.Tasks;

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

    public static class DbInitializer
    {
        public static async Task Initialize(SushiRoomDbContext context)
        {
            if (!await context.Set<User>().AnyAsync())
            {
                var adminUser = User.CreateNew(UserRole.Admin, "admin", "1qaz!QAZ".ToSha256(), "admin", "admin", true);
                adminUser.DateOfCreate = DateTimeOffset.UtcNow;

                await context.AddAsync(adminUser);

                await context.SaveChangesAsync();
            }
        }
    }
}
