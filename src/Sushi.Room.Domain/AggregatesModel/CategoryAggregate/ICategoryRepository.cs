using Sushi.Room.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sushi.Room.Domain.AggregatesModel.CategoryAggregate
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetCategoriesAsync();

        Task<Dictionary<int, Category>> GetCategoriesByIdsAsDictionaryAsync(List<int> categoryIds);

        Task<List<KeyValuePair<int, string>>> GetCategoriesForDropDownAsync();
    }
}
