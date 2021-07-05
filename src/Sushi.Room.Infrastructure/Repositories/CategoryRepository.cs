using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;

namespace Sushi.Room.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(SushiRoomDbContext context) : base(context)
        {
        }
    }
}
