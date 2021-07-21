using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Application.Services;
using Sushi.Room.Web.Infrastructure;
using Sushi.Room.Web.Models.Categories;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Categories(string culture)
        {
            return View(new PublishedCategoryViewModel
            {
                Culture = culture,
                Categories = await _categoryService.GetPublishedCategoriesByCultureAsync(culture, 1, 10)
            });
        }
    }
}
