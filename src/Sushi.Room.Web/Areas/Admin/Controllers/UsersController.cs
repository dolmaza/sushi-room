using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Application.Services;
using Sushi.Room.Application.Services.DataModels;
using Sushi.Room.Domain.Exceptions;
using Sushi.Room.Web.Areas.Admin.Models.Users;
using Sushi.Room.Web.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Sushi.Room.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("users", Name = RouteNames.Admin.User.Users)]
        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetUsersAsync();

            return View(new UserViewModel
            {
                Users = users
            });
        }

        [HttpGet]
        [Route("users/create", Name = RouteNames.Admin.User.Create)]
        public IActionResult Create()
        {
            return View(new UserEditorModel { User = new UserDto() });
        }

        [HttpPost]
        [Route("users/create", Name = RouteNames.Admin.User.Create)]
        public async Task<IActionResult> Create(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return View(new UserEditorModel { User = user });
            }

            try
            {
                var id = await _userService.AddNewUserAsync(user);

                InitSuccessMessage();
                return RedirectToRoute(RouteNames.Admin.User.Update, new { id = id });
            }
            catch (Exception)
            {
                InitErrorMessage();
                return View(new UserEditorModel { User = user });
            }
        }

        [HttpGet]
        [Route("users/{id}/update", Name = RouteNames.Admin.User.Update)]
        public async Task<IActionResult> Update(int id)
        {
            var userDto = await _userService.GetSingleUserByIdAsync(id);

            if (userDto == default)
            {
                return NotFound();
            }

            return View(new UserEditorModel { User = userDto });
        }

        [HttpPost]
        [Route("users/{id}/update", Name = RouteNames.Admin.User.Update)]
        public async Task<IActionResult> Update(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return View(new UserEditorModel { User = user });
            }

            try
            {
                await _userService.UpdateUserAsync(user);

                InitSuccessMessage();
                return RedirectToRoute(RouteNames.Admin.User.Update, new { id = user.Id });
            }
            catch (SushiRoomDomainException ex)
            {
                InitErrorMessage(ex.Message);
                return View(new UserEditorModel { User = user });
            }
            catch (Exception)
            {
                InitErrorMessage();
                return View(new UserEditorModel { User = user });
            }
        }

        [HttpPost]
        [Route("users/{id}/delete", Name = RouteNames.Admin.User.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);

                return Ok(new { message = "მომხმარებელი წარამტებით წაიშალა!" });
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
