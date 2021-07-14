using System.Collections.Generic;
using System.Threading.Tasks;
using Sushi.Room.Domain.SeedWork;

namespace Sushi.Room.Domain.AggregatesModel.ProductAggregate
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<(List<Product>, int)> GetProductsAsync(string searchValue, int pageNumber, int pageSize);
    }
}