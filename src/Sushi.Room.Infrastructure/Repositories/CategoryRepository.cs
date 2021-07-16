using Microsoft.EntityFrameworkCore;
using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sushi.Room.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(SushiRoomDbContext context) : base(context)
        {
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await Query()
                .OrderBy(ob => ob.SortIndex)
                .ThenByDescending(ob => ob.DateOfCreate)
                .ToListAsync();
        }

        public async Task<Dictionary<int, Category>> GetCategoriesByIdsAsDictionaryAsync(List<int> categoryIds)
        {
            return await Query().Where(c => categoryIds.Contains(c.Id)).ToDictionaryAsync(key => key.Id, value => value);
        }

        public async Task<List<KeyValuePair<int, string>>> GetCategoriesForDropDownAsync()
        {
            return await Query()
                .Where(c => c.IsPublished)
                .Select(c => new KeyValuePair<int, string>(c.Id, c.Caption))
                .ToListAsync();
        }
    }
}
