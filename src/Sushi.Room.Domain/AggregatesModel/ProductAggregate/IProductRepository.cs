using Sushi.Room.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sushi.Room.Domain.AggregatesModel.ProductAggregate
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<(List<Product>, int)> GetProductsAsync(string searchValue, int pageNumber, int pageSize);

        Task<List<Product>> GetPublishedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize);
    }
}