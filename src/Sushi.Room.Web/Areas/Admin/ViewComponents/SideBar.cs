using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Web.Areas.Admin.Models;
using Sushi.Room.Web.Infrastructure;
using System.Collections.Generic;

namespace Sushi.Room.Web.Areas.Admin.ViewComponents
{
    public class SideBar : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var requestedUrl = Request.Path.ToString().Split('?')[0];

            var usersUrl = Url.RouteUrl(RouteNames.Admin.User.Users);
            var categoriesUrl = Url.RouteUrl(RouteNames.Admin.Category.Categories);
            var productsUrl = Url.RouteUrl(RouteNames.Admin.Product.Products);
            var homeUrl = Url.RouteUrl(RouteNames.Admin.Dashboard.DashboardPage);

            var model = new List<SidebarMenuModel>
            {
                new SidebarMenuModel
                {
                    Caption = "მთავარი",
                    IconName = "fa fa-home",
                    Url = homeUrl,
                    IsActive = homeUrl == requestedUrl
                }
            };

            model.Add(new SidebarMenuModel
            {
                Caption = "მომხმარებლები",
                IconName = "fa fa-users",
                Url = usersUrl,
                IsActive = usersUrl == requestedUrl
            });

            model.Add(new SidebarMenuModel
            {
                Caption = "კატეგორიები",
                IconName = "fa fa-list",
                Url = categoriesUrl,
                IsActive = categoriesUrl == requestedUrl
            });

            model.Add(new SidebarMenuModel
            {
                Caption = "პროდუქტები",
                IconName = "fa fa-th-list",
                Url = productsUrl,
                IsActive = productsUrl == requestedUrl
            });

            return View(model);
        }
    }
}
