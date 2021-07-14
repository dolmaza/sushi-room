using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sushi.Room.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    [Authorize]
    public class AdminBaseController : Controller
    {
        protected readonly string _defaultSuccessMessage = "მონაცემები შეინახა წარმატებით!";
        protected readonly string _defaultErrorMessage = "უკაცრავად დაფიქსირდა შეცდომა!";
        public void InitSuccessMessage(string title = null)
        {
            TempData["Temp_Success_Message"] = title ?? _defaultSuccessMessage;
        }

        public void InitErrorMessage(string title = null)
        {
            TempData["Temp_Error_Message"] = title ?? _defaultErrorMessage;
        }

        public int GetAuthorizedUserId()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return string.IsNullOrEmpty(userId) ? default : Convert.ToInt32(userId);
        }
    }
}
