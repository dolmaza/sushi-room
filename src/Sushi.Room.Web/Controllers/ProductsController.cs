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

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("{culture}/{categoryId}/products", Name = RouteNames.WebProducts.Products)]
        public async Task<IActionResult> Products(string culture, int categoryId)
        {
            var products = await _productService.GetPublishedProductsByCategoryAsync(culture, categoryId, 1, 10);

            return View(new PublishedProductViewModel
            {
                Culture = culture,
                Products = products
            });
        }

        [HttpGet]
        [Route("{culture}/{categoryId}/products/{productId}/details", Name = RouteNames.WebProducts.ProductDetails)]
        public async Task<IActionResult> ProductDetails(string culture, int categoryId, int productId)
        {
            return View();
        }
    }
}
