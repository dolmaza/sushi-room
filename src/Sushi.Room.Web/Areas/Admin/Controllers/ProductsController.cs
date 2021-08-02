using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Application.Services;
using Sushi.Room.Application.Services.DataModels;
using Sushi.Room.Domain.Exceptions;
using Sushi.Room.Web.Areas.Admin.Models.Products;
using Sushi.Room.Web.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Sushi.Room.Web.Areas.Admin.Controllers
{
    public class ProductsController : AdminBaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("products", Name = RouteNames.Admin.Product.Products)]
        public async Task<IActionResult> Products(string searchValue, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (products, totalCount) = await _productService.GetProductsAsync(searchValue, pageNumber, pageSize);

            return View(new ProductViewModel
            {
                Products = products,
                TotalCount = totalCount,
                SearchValue = searchValue,
                CurrentPage = pageNumber
            });
        }

        [HttpGet]
        [Route("products/create", Name = RouteNames.Admin.Product.Create)]
        public async Task<IActionResult> Create()
        {
            return View(new ProductEditorModel
            {
                Product = new ProductDto
                {
                    Categories = await _categoryService.GetCategoriesForDropDownAsync()
                }
            });
        }

        [HttpPost]
        [Route("products/create", Name = RouteNames.Admin.Product.Create)]
        public async Task<IActionResult> Create(ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                product.Categories = await _categoryService.GetCategoriesForDropDownAsync();

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

                product.Categories = await _categoryService.GetCategoriesForDropDownAsync();

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

            productDto.Categories = await _categoryService.GetCategoriesForDropDownAsync();

            return View(new ProductEditorModel { Product = productDto });
        }

        [HttpPost]
        [Route("products/{id}/update", Name = RouteNames.Admin.Product.Update)]
        public async Task<IActionResult> Update(ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                product.Categories = await _categoryService.GetCategoriesForDropDownAsync();

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

                product.Categories = await _categoryService.GetCategoriesForDropDownAsync();

                return View(new ProductEditorModel { Product = product });
            }
            catch (Exception ex)
            {
                InitErrorMessage();

                product.Categories = await _categoryService.GetCategoriesForDropDownAsync();

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