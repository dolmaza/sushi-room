using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Web.Infrastructure;

namespace Sushi.Room.Web.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        [HttpGet]
        [Route("dashboard", Name = RouteNames.Admin.Dashboard.DashboardPage)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
