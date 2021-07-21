using Sushi.Room.Application.Services.DataModels;
using System.Collections.Generic;

namespace Sushi.Room.Web.Models.Categories
{
    public class PublishedCategoryViewModel
    {
        public string Culture { get; set; }
        public List<PublishedCategoryDto> Categories { get; set; }
    }
}
