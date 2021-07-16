using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sushi.Room.Application.Options;
using Sushi.Room.Application.Services.DataModels;
using Sushi.Room.Domain.AggregatesModel.ProductAggregate;
using Sushi.Room.Domain.Exceptions;

namespace Sushi.Room.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly AppSettings _appSettings;
        private readonly IUploadService _uploadService;

        public ProductService(IProductRepository repository, IOptions<AppSettings> appSettings, IUploadService uploadService)
        {
            _repository = repository;
            _uploadService = uploadService;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<ProductDto>, int)> GetProductsAsync(string searchValue, int pageNumber, int pageSize)
        {
            var (products, count) = await _repository.GetProductsAsync(searchValue, pageNumber, pageSize);

            return (products.Select(GetProductDtoFromProduct).ToList(), count);
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
                    categoryId: productDto.CategoryId, 
                    userId: userId, 
                    title: productDto.Title, 
                    titleEng: productDto.TitleEng, 
                    description: productDto.Description, 
                    descriptionEng: productDto.DescriptionEng, 
                    imageName: imageName, 
                    price: productDto.Price ?? 0, 
                    isPublished: productDto.IsPublished
                );

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return product.Id;
        }

        public async Task UpdateProductAsync(int userId, ProductDto productDto)
        {
            var product = await _repository.FindByIdAsync(productDto.Id);

            var imageName = _uploadService.GetImageUniqName(productDto.ImageName);
            await _uploadService.SaveImageAsync(productDto.ImageBase64, imageName, product.ImageName);

            if (product == default)
            {
                throw new SushiRoomDomainException("პროდუცტი ვერ მოიძებნა");
            }

            if (product.Price != productDto.Price)
            {
                product.SaveProductPriceChangeHistory(userId, productDto.Price ?? 0);
            }

            product.UpdateMetaData
            (
                categoryId: productDto.CategoryId, 
                userId: userId, 
                title: productDto.Title, 
                titleEng: productDto.TitleEng, 
                description: productDto.Description, 
                descriptionEng: productDto.DescriptionEng, 
                imageName: imageName, 
                price: productDto.Price ?? 0
            );

            if (productDto.IsPublished)
            {
                product.MarkAsPublished();
            }
            else
            {
                product.MarkAsUnpublished();
            }
            
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

        private ProductDto GetProductDtoFromProduct(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                CategoryCaption = product.Category.Caption,
                CategoryId = product.CategoryId,
                Title = product.Title,
                TitleEng = product.TitleEng,
                Description = product.Description,
                DescriptionEng = product.DescriptionEng,
                Price = product.Price,
                IsPublished = product.IsPublished,
                ImageUrl = string.IsNullOrEmpty(product.ImageName) 
                    ? default 
                    : $"{_appSettings.WebsiteBaseUrl}{_appSettings.UploadFolderPath}{product.ImageName}"
            };
        }
    }
}