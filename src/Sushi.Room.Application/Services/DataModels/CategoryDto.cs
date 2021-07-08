using System.ComponentModel.DataAnnotations;

namespace Sushi.Room.Application.Services.DataModels
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Display(Name = "დასახელება")]
        public string Caption { get; set; }

        [Display(Name = "დასახელება ინგ.")]
        public string CaptionEng { get; set; }

        public string ImageName { get; set; }

        [Display(Name = "გამოქვეყნებულია?")]
        public bool IsPublished { get; set; }

        [Display(Name = "სორტირების ინდექსი")]
        public int SortIndex { get; set; }
    }
}
