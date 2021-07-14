using System.Collections.Generic;
using System.Threading.Tasks;
using Sushi.Room.Application.Services.DataModels;

namespace Sushi.Room.Application.Services
{
    public interface IProductService
    {
        Task<(List<ProductDto>, int)> GetProductsAsync(string searchValue, int pageNumber, int pageSize);

        Task<ProductDto> GetSingleProductByIdAsync(int id);

        Task<int> AddNewProductAsync(int userId, ProductDto productDto);

        Task UpdateProductAsync(int userId, ProductDto productDto);

        Task DeleteProductAsync(int id);
    }
}