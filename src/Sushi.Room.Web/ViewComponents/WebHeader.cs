using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sushi.Room.Application.Constants;
using Sushi.Room.Web.Infrastructure;
using Sushi.Room.Web.Models;

namespace Sushi.Room.Web.ViewComponents
{
    public class WebHeader : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var culture = ViewContext.RouteData.Values["culture"]?.ToString();
            var url = @HttpContext.Request.Path.ToString();
            
            var kaDropDownItem = new LanguagePickerDropDownItemModel
            {
                Name = "ქართ.",
                Icon = "/images/ge.svg",
                Url = url == "/" 
                    ? Url.RouteUrl(RouteNames.WebCategories.Categories, new {culture = Cultures.ka }) 
                    : url.Replace("/en/", "/ka/")
            };
            
            var enDropDownItem = new LanguagePickerDropDownItemModel
            {
                Name = "Eng",
                Icon = "/images/us.svg",
                Url = url == "/" 
                    ? Url.RouteUrl(RouteNames.WebCategories.Categories, new {culture = Cultures.en }) 
                    : url.Replace("/ka/", "/en/")
            };
            return View(new HeaderComponentModel
            {
                Culture = culture,
                SelectedLanguagePickerDropDownItem = culture == Cultures.en ? enDropDownItem : kaDropDownItem,
                LanguagePickerDropDownItems = new List<LanguagePickerDropDownItemModel>
                {
                    kaDropDownItem,
                    enDropDownItem
                }
            });
        }
    }
}