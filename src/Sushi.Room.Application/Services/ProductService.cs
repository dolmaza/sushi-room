using Microsoft.Extensions.Options;
using Sushi.Room.Application.Constants;
using Sushi.Room.Application.Options;
using Sushi.Room.Application.Services.DataModels;
using Sushi.Room.Domain.AggregatesModel.ProductAggregate;
using Sushi.Room.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;

namespace Sushi.Room.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly AppSettings _appSettings;
        private readonly IUploadService _uploadService;

        public ProductService(IProductRepository repository, IOptions<AppSettings> appSettings, IUploadService uploadService, ICategoryRepository categoryRepository)
        {
            _repository = repository;
            _uploadService = uploadService;
            _categoryRepository = categoryRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<ProductDto>, int)> GetProductsAsync(string searchValue, int pageNumber, int pageSize)
        {
            var (products, count) = await _repository.GetProductsAsync(searchValue, pageNumber, pageSize);

            return (products.Select(GetProductDtoFromProduct).ToList(), count);
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _repository.GetProductsByCategoryAsync(categoryId);

            return products.Select(GetProductDtoFromProduct).ToList();
        }
        public async Task<ProductDto> GetSingleProductByIdAsync(int id)
        {
            var product = await _repository.FindByIdAsync(id);

            if (product == default)
            {
                return default;
            }

            return GetProductDtoFromProduct(product);
        }

        public async Task<int> AddNewProductAsync(int userId, ProductDto productDto)
        {
            var imageName = _uploadService.GetImageUniqName(productDto.ImageName);
            await _uploadService.SaveImageAsync(productDto.ImageBase64, imageName);

            var product = Product.CreateNew
                (
                    userId: userId,
                    title: productDto.Title,
                    titleEng: productDto.TitleEng,
                    description: productDto.Description,
                    descriptionEng: productDto.DescriptionEng,
                    imageName: imageName,
                    price: productDto.Price ?? 0,
                    discountPercent: productDto.DiscountPercent,
                    isPublished: productDto.IsPublished
                );
            
            product.SetCategories(productDto.CategoryIds.Select(categoryId => new ProductCategory(categoryId)).ToList());

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return product.Id;
        }

        public async Task UpdateProductAsync(int userId, ProductDto productDto)
        {
            var product = await _repository.FindByIdAsync(productDto.Id);

            if (product == default)
            {
                throw new SushiRoomDomainException("პროდუცტი ვერ მოიძებნა");
            }

            if (product.Price != productDto.Price)
            {
                product.SaveProductPriceChangeHistory(userId, productDto.Price ?? 0);
            }

            if (!string.IsNullOrEmpty(productDto.ImageBase64))
            {
                var imageName = _uploadService.GetImageUniqName(productDto.ImageName);
                await _uploadService.SaveImageAsync(productDto.ImageBase64, imageName, product.ImageName);

                product.SetImageName(imageName);
            }

            product.UpdateMetaData
            (
                userId: userId,
                title: productDto.Title,
                titleEng: productDto.TitleEng,
                description: productDto.Description,
                descriptionEng: productDto.DescriptionEng,
                price: productDto.Price ?? 0,
                discountPercent: productDto.DiscountPercent
            );

            if (productDto.IsPublished)
            {
                product.MarkAsPublished();
            }
            else
            {
                product.MarkAsUnpublished();
            }
            
            product.SetCategories(productDto.CategoryIds.Select(categoryId => new ProductCategory(categoryId)).ToList());

            _repository.Update(product);

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repository.FindByIdAsync(id);

            if (product == default)
            {
                throw new SushiRoomDomainException("პროდუქტი ვერ მოიძებნა");
            }

            _uploadService.DeleteImage(product.ImageName);

            _repository.Remove(product);

            await _repository.SaveChangesAsync();
        }

        public async Task<List<PublishedProductDto>> GetPublishedProductsByCategoryAsync(string culture, int categoryId, int pageNumber, int pageSize)
        {
            var products = await _repository.GetPublishedProductsByCategoryAsync(categoryId, pageNumber, pageSize);

            return products.Select(product => GetPublishedProductDtoFromProduct(culture, product)).ToList();
        }

        public async Task<PublishedProductDto> GetPublishedProductDetailsAsync(string culture, int id)
        {
            var product = await _repository.FindByIdAsync(id);

            if (product == default)
            {
                return default;
            }

            return GetPublishedProductDtoFromProduct(culture, product);
        }

        public async Task<List<PublishedProductDto>> GetPublishedProductsByIdsAsync(string culture, List<int> ids)
        {
            var products = await _repository.GetPublishedProductsByIdsAsync(ids);

            return products.Select(product => GetPublishedProductDtoFromProduct(culture, product)).ToList();
        }

        public async Task SyncSortIndexesAsync(int categoryId, List<KeyValuePair<int, int>> sortIndexes)
        {
            var category = await _categoryRepository.FindByIdAsync(categoryId);

            if (category == default)
            {
                throw new SushiRoomDomainException("კატეგორია ვერ მოიძებნა!");
            }
            
            var productCategoriesDict = category.ProductCategories.ToDictionary(key => key.ProductId, value => value);

            if (productCategoriesDict.Count > 0)
            {
                foreach (var sortIndexItem in sortIndexes)
                {
                    if (productCategoriesDict.ContainsKey(sortIndexItem.Key))
                    {
                        var productCategory = productCategoriesDict[sortIndexItem.Key];

                        productCategory.UpdateMetaData(sortIndexItem.Value);
                    }
                }

                _categoryRepository.Update(category);
                await _categoryRepository.SaveChangesAsync();
            }
        }

        
        private ProductDto GetProductDtoFromProduct(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                CategoryIds = product.ProductCategories.Select(p => p.CategoryId).ToList(),
                Title = product.Title,
                TitleEng = product.TitleEng,
                Description = product.Description,
                DescriptionEng = product.DescriptionEng,
                Price = product.Price,
                DiscountPercent = product.DiscountPercent,
                DiscountedPrice = product.CalculateDiscountedPrice(),
                IsPublished = product.IsPublished,
                CategoryCaptions = string.Join(", ",  product.ProductCategories.Select(pc => pc.Category.Caption).ToList()),
                ImageUrl = string.IsNullOrEmpty(product.ImageName)
                    ? default
                    : $"{_appSettings.WebsiteBaseUrl}{_appSettings.UploadFolderPath}{product.ImageName}"
            };
        }

        private PublishedProductDto GetPublishedProductDtoFromProduct(string culture, Product product)
        {
            return new PublishedProductDto
            {
                Id = product.Id,
                Title = GetProductTitleByCulture(culture, product),
                Description = GetProductDescriptionByCulture(culture, product),
                ImageUrl = GetImageUrl(product.ImageName),
                Price = product.Price,
                DiscountPercent = product.DiscountPercent,
                DiscountedPrice = product.CalculateDiscountedPrice()
            };
        }
        
        private string GetProductTitleByCulture(string culture, Product product)
        {
            return culture switch
            {
                Cultures.ka => product.Title,
                Cultures.en => product.TitleEng,
                _ => product.Title
            };
        }

        private string GetProductDescriptionByCulture(string culture, Product product)
        {
            return culture switch
            {
                Cultures.ka => product.Description,
                Cultures.en => product.DescriptionEng,
                _ => product.Title
            };
        }

        private string GetImageUrl(string imageName) => string.IsNullOrEmpty(imageName)
            ? default
            : $"{_appSettings.WebsiteBaseUrl}{_appSettings.UploadFolderPath}{imageName}";
    }
}