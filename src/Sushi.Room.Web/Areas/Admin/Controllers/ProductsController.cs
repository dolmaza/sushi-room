using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Application.Services;
using Sushi.Room.Application.Services.DataModels;
using Sushi.Room.Domain.Exceptions;
using Sushi.Room.Web.Areas.Admin.Models.Categories;
using Sushi.Room.Web.Areas.Admin.Models.Products;
using Sushi.Room.Web.Infrastructure;

namespace Sushi.Room.Web.Areas.Admin.Controllers
{
    public class ProductsController : AdminBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("products", Name = RouteNames.Admin.Product.Products)]
        public async Task<IActionResult> Products()
        {
            var (products, totalCount) = await _productService.GetProductsAsync(default, 1, 10);
            
            return View(new ProductViewModel
            {
                Products = products,
                TotalCount = totalCount
            });
        }

        [HttpGet]
        [Route("products/create", Name = RouteNames.Admin.Product.Create)]
        public IActionResult Create()
        {
            return View(new ProductEditorModel { Product = new ProductDto()});
        }
        
        [HttpPost]
        [Route("products/create", Name = RouteNames.Admin.Product.Create)]
        public async Task<IActionResult> Create(ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return View(new ProductEditorModel { Product = product });
            }

            try
            {
                var id = await _productService.AddNewProductAsync(GetAuthorizedUserId(), product);

                InitSuccessMessage();
                return RedirectToRoute(RouteNames.Admin.Product.Update, new { id = id });
            }
            catch (Exception)
            {
                InitErrorMessage();
                return View(new ProductEditorModel { Product = product });
            }
        }
        
        [HttpGet]
        [Route("products/{id}/update", Name = RouteNames.Admin.Product.Update)]
        public async Task<IActionResult> Update(int id)
        {
            var productDto = await _productService.GetSingleProductByIdAsync(id);

            if (productDto == default)
            {
                return NotFound();
            }

            return View(new ProductEditorModel { Product = productDto });
        }

        [HttpPost]
        [Route("products/{id}/update", Name = RouteNames.Admin.Product.Update)]
        public async Task<IActionResult> Update(ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return View(new ProductEditorModel { Product = product });
            }

            try
            {
                await _productService.UpdateProductAsync(GetAuthorizedUserId(), product);

                InitSuccessMessage();
                return RedirectToRoute(RouteNames.Admin.Product.Update, new { id = product.Id });
            }
            catch (SushiRoomDomainException ex)
            {
                InitErrorMessage(ex.Message);
                return View(new ProductEditorModel { Product = product });
            }
            catch (Exception)
            {
                InitErrorMessage();
                return View(new ProductEditorModel { Product = product });
            }
        }
        
        [HttpPost]
        [Route("products/{id}/delete", Name = RouteNames.Admin.Product.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);

                return Ok(new { message = "პროდუქტი წარამტებით წაიშალა!" });
            }
            catch (SushiRoomDomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = _defaultErrorMessage });
            }
        }
    }
}