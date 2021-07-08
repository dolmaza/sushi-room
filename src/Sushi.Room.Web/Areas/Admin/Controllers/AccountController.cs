using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Application.Services;
using Sushi.Room.Web.Areas.Admin.Models.Account;
using Sushi.Room.Web.Infrastructure;
using Sushi.Room.Web.Infrastructure.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sushi.Room.Web.Areas.Admin.Controllers
{
    [Route("admin")]
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("login", Name = RouteNames.Admin.Account.Login)]
        [Route("")]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginEditorModel { ReturnUrl = returnUrl });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginEditorModel model)
        {
            var user = await _userService.GetAuthorizedUserAsync(model.UserName, model.Password);
            if (user == null || !user.IsActive)
            {
                model.ErrorMessage = "UserName ან პაროლი არასწორია";
                return View(model);
            }
            else
            {

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var identity = IdentityConfig.CreateClaimsIdentity(user);

                await HttpContext.SignInAsync
                (
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddHours(1) }
                );

                return Redirect(string.IsNullOrEmpty(model.ReturnUrl) ? Url.RouteUrl(RouteNames.Admin.Dashboard.DashboardPage) : model.ReturnUrl);
            }
        }

        [HttpGet]
        [Route("logout", Name = RouteNames.Admin.Account.Logout)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToRoute(RouteNames.Admin.Account.Login);
        }
    }
}
