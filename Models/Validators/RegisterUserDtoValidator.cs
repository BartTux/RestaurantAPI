using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
	public RegisterUserDtoValidator(RestaurantDbContext dbContext)
	{
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .Custom((value, context) =>
            {
                var isEmailInUse = dbContext.Users.Any(x => x.Email == value);

                if (isEmailInUse)
                    context.AddFailure("Email", "This e-mail address is already taken");
            });

        RuleFor(x => x.Password)
            .NotNull()
            .MinimumLength(8);

        RuleFor(x => x.ConfirmPassword)
            .NotNull()
            .Equal(x => x.Password);

        RuleFor(x => x.FirstName).NotNull();

        RuleFor(x => x.Nationality).NotNull();
    }
}
