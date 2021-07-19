using System.Collections.Generic;
using Sushi.Room.Application.Services.DataModels;

namespace Sushi.Room.Web.Areas.Admin.Models.Products
{
    public class ProductViewModel
    {
        public List<ProductDto> Products { get; set; }
        public int TotalCount { get; set; }
        public string SearchValue { get; set; }
        public int CurrentPage { get; set; }
    }
}