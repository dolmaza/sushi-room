using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Application.Services;
using Sushi.Room.Web.Infrastructure;
using Sushi.Room.Web.Models.Products;
using System.Threading.Tasks;

namespace Sushi.Room.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("{culture}/{categoryId}/products", Name = RouteNames.WebProducts.Products)]
        public async Task<IActionResult> Products(string culture, int categoryId)
        {
            var category = await _categoryService.GetSinglePublishedCategoryByIdAsync(culture, categoryId);

            if (category == default)
            {
                return NotFound();
            }
            
            var products = await _productService.GetPublishedProductsByCategoryAsync(culture, categoryId, 1, 10);
            
            return View(new PublishedProductViewModel
            {
                Culture = culture,
                CategoryId = categoryId,
                CategoryCaption = category.Caption,
                Products = products
            });
        }

        [HttpGet]
        [Route("{culture}/{categoryId}/products/{productId}/details", Name = RouteNames.WebProducts.ProductDetails)]
        public async Task<IActionResult> ProductDetails(string culture, int categoryId, int productId)
        {
            var category = await _categoryService.GetSinglePublishedCategoryByIdAsync(culture, categoryId);

            if (category == default)
            {
                return NotFound();
            }
            
            var product = await _productService.GetPublishedProductDetailsAsync(culture, productId);

            if (product == default)
            {
                return NotFound();
            }
            return View(new PublishedProductDetailViewModel
            {
                Culture = culture,
                CategoryId = category.Id,
                CategoryCaption = category.Caption,
                Product = product
            });
        }

        [HttpPost]
        [Route("{culture}/products-by-ids", Name = RouteNames.WebProducts.ProductsByIds)]
        public async Task<IActionResult> GetPublishedProductsByIdsAsync(string culture, GetPublishedProductsByIdsModel request)
        {
            var products = await _productService.GetPublishedProductsByIdsAsync(culture, request.ProductIds);
            
            return Ok(new
            {
                Data = products
            });
        }
    }
}
