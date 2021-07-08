using Sushi.Room.Application.Services.DataModels;
using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;
using Sushi.Room.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sushi.Room.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _repository.GetCategoriesAsync();

            return categories.Select(GetCategoryToCategoryDto).ToList();
        }

        public async Task<CategoryDto> GetSingleCategoryByIdAsync(int id)
        {
            var category = await _repository.FindByIdAsync(id);

            if (category == default)
            {
                return default;
            }

            return GetCategoryToCategoryDto(category);
        }

        public async Task<int> AddNewCategoryAsync(CategoryDto categoryDto)
        {
            //ToDo: save image

            var category = Category.CreateNew(categoryDto.Caption, categoryDto.CaptionEng, categoryDto.ImageName, categoryDto.IsPublished);

            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            return category.Id;
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _repository.FindByIdAsync(categoryDto.Id);

            //ToDo: save image

            if (category == default)
            {
                throw new SushiRoomDomainException("კატეგორია ვერ მოიძებნა");
            }

            category.UpdateMetaData(categoryDto.Caption, categoryDto.CaptionEng, categoryDto.ImageName);

            if (categoryDto.IsPublished)
            {
                category.MarkAsPublished();
            }
            else
            {
                category.MarkAsUnpublished();
            }

            _repository.Update(category);

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _repository.FindByIdAsync(id);

            if (category == default)
            {
                throw new SushiRoomDomainException("კატეგორია ვერ მოიძებნა");
            }

            _repository.Remove(category);

            await _repository.SaveChangesAsync();
        }

        public async Task SyncSortIndexesAsync(List<KeyValuePair<int, int>> sortIndexes)
        {
            var categoriesDict = await _repository.GetCategoriesByIdsAsDictionaryAsync(sortIndexes.Select(si => si.Key).ToList());

            if (categoriesDict.Count > 0)
            {
                foreach (var sortIndexItem in sortIndexes)
                {
                    if (categoriesDict.ContainsKey(sortIndexItem.Key))
                    {
                        var category = categoriesDict[sortIndexItem.Key];

                        category.SetSortIndex(sortIndexItem.Value);
                        _repository.Update(category);
                    }
                }

                await _repository.SaveChangesAsync();
            }
        }

        public CategoryDto GetCategoryToCategoryDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Caption = category.Caption,
                CaptionEng = category.CaptionEng,
                IsPublished = category.IsPublished,
                SortIndex = category.SortIndex
            };
        }
    }
}