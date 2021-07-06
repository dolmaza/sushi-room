using FluentValidation;
using Sushi.Room.Application.Services;
using Sushi.Room.Application.Services.DataModels;

namespace Sushi.Room.Web.Validators.Users
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator(IUserService userService)
        {
            RuleFor(user => user.UserName)
                .NotEmpty()
                .WithMessage("გთხოვთ შეავსოთ ველი")
                .NotNull()
                .WithMessage("გთხოვთ შეავსოთ ველი")
                .MustAsync(async (user, value, cancellationToken) => !(await userService.IsUserUsernameNotUnique(value, user.Id)))
                .WithMessage("მომხმარებელი ასეთი User Name-ით უკვე არსებობს")
                .Matches("^[A-Za-z][A-Za-z0-9]*$")
                .WithMessage("ფორმატი არასწორია");

            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("გთხოვთ შეავსოთ ველი")
                .NotNull()
                .WithMessage("გთხოვთ შეავსოთ ველი");

            RuleFor(user => user.LastName)
                .NotEmpty()
                .WithMessage("გთხოვთ შეავსოთ ველი")
                .NotNull()
                .WithMessage("გთხოვთ შეავსოთ ველი");
        }
    }
}
