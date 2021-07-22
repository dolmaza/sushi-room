using Microsoft.EntityFrameworkCore;
using Sushi.Room.Domain.AggregatesModel.ProductAggregate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sushi.Room.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(SushiRoomDbContext context) : base(context)
        {
        }

        public async Task<(List<Product>, int)> GetProductsAsync(string searchValue, int pageNumber, int pageSize)
        {
            var query = Query()
                .Where(p => string.IsNullOrEmpty(searchValue)
                            || p.Title.Contains(searchValue)
                            || p.TitleEng.Contains(searchValue)
                            || p.Description.Contains(searchValue)
                            || p.DescriptionEng.Contains(searchValue)
                            || p.Category.Caption.Contains(searchValue)
                            || p.Category.CaptionEng.Contains(searchValue))
                .OrderByDescending(ob => ob.DateOfCreate);

            var count = query.Count();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, count);
        }

        public async Task<List<Product>> GetPublishedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize)
        {
            return await Query()
                 .Where(p => p.CategoryId == categoryId)
                 .Where(p => p.IsPublished)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .OrderByDescending(ob => ob.DateOfCreate)
                 .ToListAsync();
        }
    }
}