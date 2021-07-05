using Microsoft.AspNetCore.Mvc;

namespace Sushi.Room.Web.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
