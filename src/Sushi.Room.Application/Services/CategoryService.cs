using System;
using Microsoft.Extensions.Options;
using Sushi.Room.Application.Options;
using Sushi.Room.Application.Services.DataModels;
using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;
using Sushi.Room.Domain.Exceptions;
using System.Collections.Generic;
using System.IO;
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

            var imageName = _uploadService.GetImageUniqName(categoryDto.ImageName);
            await _uploadService.SaveImageAsync(categoryDto.ImageBase64, imageName, category.ImageName);

            if (category == default)
            {
                throw new SushiRoomDomainException("კატეგორია ვერ მოიძებნა");
            }

            category.UpdateMetaData(categoryDto.Caption, categoryDto.CaptionEng, imageName);

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

        private CategoryDto GetCategoryToCategoryDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Caption = category.Caption,
                CaptionEng = category.CaptionEng,
                IsPublished = category.IsPublished,
                SortIndex = category.SortIndex,
                ImageUrl = string.IsNullOrEmpty(category.ImageName) 
                    ? default 
                    : $"{_appSettings.WebsiteBaseUrl}{_appSettings.UploadFolderPath}{category.ImageName}"
            };
        }
    }
}