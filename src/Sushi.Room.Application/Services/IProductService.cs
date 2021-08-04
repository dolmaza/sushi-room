using Sushi.Room.Application.Services.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sushi.Room.Application.Services
{
    public interface IProductService
    {
        Task<(List<ProductDto>, int)> GetProductsAsync(string searchValue, int pageNumber, int pageSize);

        Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        
        Task<ProductDto> GetSingleProductByIdAsync(int id);

        Task<int> AddNewProductAsync(int userId, ProductDto productDto);

        Task UpdateProductAsync(int userId, ProductDto productDto);

        Task DeleteProductAsync(int id);

        Task<List<PublishedProductDto>> GetPublishedProductsByCategoryAsync(string culture, int categoryId, int pageNumber, int pageSize);

        Task<PublishedProductDto> GetPublishedProductDetailsAsync(string culture, int id);

        Task<List<PublishedProductDto>> GetPublishedProductsByIdsAsync(string culture, List<int> ids);

        Task SyncSortIndexesAsync(int categoryId, List<KeyValuePair<int, int>> sortIndexes);
    }
}