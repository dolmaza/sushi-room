using FluentValidation;
using Sushi.Room.Application.Services.DataModels;

namespace Sushi.Room.Web.Validators.Categories
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(category => category.Caption)
                .NotEmpty()
                .WithMessage("გთხოვთ შეავსოთ ველი")
                .NotNull()
                .WithMessage("გთხოვთ შეავსოთ ველი");

        }
    }
}
