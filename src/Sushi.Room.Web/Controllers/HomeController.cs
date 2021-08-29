using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sushi.Room.Application.Constants;
using Sushi.Room.Web.Infrastructure;
using Sushi.Room.Web.Models;
using System.Diagnostics;

namespace Sushi.Room.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        public IActionResult Index()
        {
            return RedirectToRoute(RouteNames.WebCategories.Categories, new { culture = Cultures.ka });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
