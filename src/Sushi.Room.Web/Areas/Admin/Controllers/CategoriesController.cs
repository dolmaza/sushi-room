using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Application.Services;
using Sushi.Room.Application.Services.DataModels;
using Sushi.Room.Domain.Exceptions;
using Sushi.Room.Web.Areas.Admin.Models.Categories;
using Sushi.Room.Web.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Sushi.Room.Web.Areas.Admin.Controllers
{
    public class CategoriesController : AdminBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("categories", Name = RouteNames.Admin.Category.Categories)]
        public async Task<IActionResult> Categories()
        {
            var categories = await _categoryService.GetCategoriesAsync();

            return View(new CategoryViewModel
            {
                Categories = categories
            });
        }

        [HttpGet]
        [Route("categories/create", Name = RouteNames.Admin.Category.Create)]
        public IActionResult Create()
        {
            return View(new CategoryEditorModel { Category = new CategoryDto() });
        }

        [HttpPost]
        [Route("categories/create", Name = RouteNames.Admin.Category.Create)]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return View(new CategoryEditorModel { Category = category });
            }

            try
            {
                var id = await _categoryService.AddNewCategoryAsync(category);

                InitSuccessMessage();
                return RedirectToRoute(RouteNames.Admin.Category.Update, new { id = id });
            }
            catch (Exception)
            {
                InitErrorMessage();
                return View(new CategoryEditorModel { Category = category });
            }
        }

        [HttpGet]
        [Route("categories/{id}/update", Name = RouteNames.Admin.Category.Update)]
        public async Task<IActionResult> Update(int id)
        {
            var categoryDto = await _categoryService.GetSingleCategoryByIdAsync(id);

            if (categoryDto == default)
            {
                return NotFound();
            }

            return View(new CategoryEditorModel { Category = categoryDto });
        }

        [HttpPost]
        [Route("categories/{id}/update", Name = RouteNames.Admin.Category.Update)]
        public async Task<IActionResult> Update(CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return View(new CategoryEditorModel { Category = category });
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(category);

                InitSuccessMessage();
                return RedirectToRoute(RouteNames.Admin.Category.Update, new { id = category.Id });
            }
            catch (SushiRoomDomainException ex)
            {
                InitErrorMessage(ex.Message);
                return View(new CategoryEditorModel { Category = category });
            }
            catch (Exception)
            {
                InitErrorMessage();
                return View(new CategoryEditorModel { Category = category });
            }
        }

        [HttpPost]
        [Route("categories/{id}/delete", Name = RouteNames.Admin.Category.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);

                return Ok(new { message = "კატეგორია წარამტებით წაიშალა!" });
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

        [HttpPost]
        [Route("categories/sync-sort-indexes", Name = RouteNames.Admin.Category.SyncSortIndexes)]
        public async Task<IActionResult> SyncSortIndexes(SyncSortIndexesEditorModel model)
        {
            try
            {
                await _categoryService.SyncSortIndexesAsync(model.SortIndexes);

                return Ok();
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
