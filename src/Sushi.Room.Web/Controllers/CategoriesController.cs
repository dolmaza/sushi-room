using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Application.Constants;
using Sushi.Room.Application.Services;
using Sushi.Room.Web.Infrastructure;
using Sushi.Room.Web.Models.Categories;
using System.Threading.Tasks;
using Sushi.Room.Application.Services.DataModels;

namespace Sushi.Room.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("")]
        [Route("{culture}/categories", Name = RouteNames.WebCategories.Categories)]
        public async Task<IActionResult> Categories(string culture = Cultures.ka)
        {
            return View(new PublishedCategoryViewModel
            {
                Culture = culture,
            });
        }
        
        [HttpGet]
        [Route("{culture}/categories/data", Name = RouteNames.WebCategories.CategoriesData)]
        public async Task<IActionResult> CategoriesData(string culture, int pageNumber)
        {
            var categories = await _categoryService.GetPublishedCategoriesByCultureAsync(culture, pageNumber, 20);

            return Ok(new
            {
                categories = categories
            });
        }
    }
}
