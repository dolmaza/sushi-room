using FluentValidation;
using Sushi.Room.Application.Services.DataModels;

namespace Sushi.Room.Web.Validators.Products
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(product => product.Title)
                .NotEmpty()
                .WithMessage("გთხოვთ შეავსოთ ველი")
                .NotNull()
                .WithMessage("გთხოვთ შეავსოთ ველი");
        }
    }
}