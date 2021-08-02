using Microsoft.Extensions.Options;
using Sushi.Room.Application.Constants;
using Sushi.Room.Application.Options;
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
        private readonly AppSettings _appSettings;
        private readonly IUploadService _uploadService;
        public CategoryService(ICategoryRepository repository, IOptions<AppSettings> appSettings, IUploadService uploadService)
        {
            _repository = repository;
            _appSettings = appSettings.Value;
            _uploadService = uploadService;
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
            var imageName = _uploadService.GetImageUniqName(categoryDto.ImageName);
            await _uploadService.SaveImageAsync(categoryDto.ImageBase64, imageName);

            var category = Category.CreateNew(categoryDto.Caption, categoryDto.CaptionEng, imageName, categoryDto.IsPublished);

            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            return category.Id;
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _repository.FindByIdAsync(categoryDto.Id);

            if (category == default)
            {
                throw new SushiRoomDomainException("კატეგორია ვერ მოიძებნა");
            }

            if (!string.IsNullOrEmpty(categoryDto.ImageBase64))
            {
                var imageName = _uploadService.GetImageUniqName(categoryDto.ImageName);
                await _uploadService.SaveImageAsync(categoryDto.ImageBase64, imageName, category.ImageName);

                category.SetImageName(imageName);
            }

            category.UpdateMetaData(categoryDto.Caption, categoryDto.CaptionEng);

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

            _uploadService.DeleteImage(category.ImageName);

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

        public async Task<List<KeyValuePair<int, string>>> GetCategoriesForDropDownAsync()
        {
            return await _repository.GetCategoriesForDropDownAsync();
        }

        public async Task<List<PublishedCategoryDto>> GetPublishedCategoriesByCultureAsync(string culture, int pageNumber, int pageSize)
        {
            var categories = await _repository.GetPublishedCategoriesAsync(pageNumber, pageSize);

            return categories.Select(category => GetCategoryToPublishedCategoryDto(culture, category)).ToList();
        }

        public async Task<PublishedCategoryDto> GetSinglePublishedCategoryByIdAsync(string culture, int id)
        {
            var category = await _repository.FindByIdAsync(id);

            if (category == default || !category.IsPublished)
            {
                return default;
            }

            return GetCategoryToPublishedCategoryDto(culture, category);
        }

        private CategoryDto GetCategoryToCategoryDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Caption = category.Caption,
                CaptionEng = category.CaptionEng,
                IsPublished = category.IsPublished,
                SortIndex = category.SortIndex,
                ImageUrl = GetImageUrl(category.ImageName)
            };
        }

        private PublishedCategoryDto GetCategoryToPublishedCategoryDto(string culture, Category category)
        {
            return new PublishedCategoryDto
            {
                Id = category.Id,
                Caption = GetCategoryCaptionByCulture(culture, category),
                ImageUrl = GetImageUrl(category.ImageName)
            };
        }

        private string GetCategoryCaptionByCulture(string culture, Category category)
        {
            return culture switch
            {
                Cultures.ka => category.Caption,
                Cultures.en => category.CaptionEng,
                _ => category.Caption
            };
        }

        private string GetImageUrl(string imageName) => string.IsNullOrEmpty(imageName)
            ? default
            : $"{_appSettings.WebsiteBaseUrl}{_appSettings.UploadFolderPath}{imageName}";
    }
}