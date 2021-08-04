using System.Collections.Generic;
using Sushi.Room.Application.Services.DataModels;

namespace Sushi.Room.Web.Areas.Admin.Models.Categories
{
    public class CategoryProductViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryCaption { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}