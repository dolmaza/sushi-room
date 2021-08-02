using Microsoft.EntityFrameworkCore;
using Sushi.Room.Domain.AggregatesModel.ProductAggregate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;

namespace Sushi.Room.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(SushiRoomDbContext context) : base(context)
        {
        }

        public async Task<(List<Product>, int)> GetProductsAsync(string searchValue, int pageNumber, int pageSize)
        {
            var query = from product in Query()
                join productCategory in Context.Set<ProductCategory>() on product.Id equals productCategory.ProductId
                join category in Context.Set<Category>() on productCategory.CategoryId equals category.Id 
                orderby product.DateOfCreate descending
                where string.IsNullOrEmpty(searchValue) || product.Title.Contains(searchValue)
                                                        || product.TitleEng.Contains(searchValue)
                                                        || product.Description.Contains(searchValue)
                                                        || product.DescriptionEng.Contains(searchValue)
                                                        || category.Caption.Contains(searchValue)
                                                        || category.CaptionEng.Contains(searchValue)
                select product;

            var count = query.Count();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, count);
        }

        public async Task<List<Product>> GetPublishedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize)
        {
            var query = from product in Query()
                join productCategory in Context.Set<ProductCategory>() on product.Id equals productCategory.ProductId
                join category in Context.Set<Category>() on productCategory.CategoryId equals category.Id 
                orderby productCategory.SortIndex, product.DateOfCreate descending 
                where product.IsPublished && category.Id == categoryId
                select product;
            
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}