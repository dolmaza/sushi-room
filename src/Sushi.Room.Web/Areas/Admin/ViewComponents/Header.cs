using Microsoft.AspNetCore.Mvc;

namespace Sushi.Room.Web.Areas.Admin.ViewComponents
{
    public class Header : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
