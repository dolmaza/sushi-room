using Sushi.Room.Application.Services.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sushi.Room.Application.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoriesAsync();

        Task<CategoryDto> GetSingleCategoryByIdAsync(int id);

        Task<int> AddNewCategoryAsync(CategoryDto categoryDto);

        Task UpdateCategoryAsync(CategoryDto categoryDto);

        Task DeleteCategoryAsync(int id);

        Task SyncSortIndexesAsync(List<KeyValuePair<int, int>> sortIndexes);

        Task<List<KeyValuePair<int, string>>> GetCategoriesForDropDownAsync();

        Task<List<PublishedCategoryDto>> GetPublishedCategoriesByCultureAsync(string culture, int pageNumber, int pageSize);

        Task<PublishedCategoryDto> GetSinglePublishedCategoryByIdAsync(string culture, int id);
    }
}
