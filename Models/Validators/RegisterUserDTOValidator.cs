using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
{
	public RegisterUserDTOValidator(RestaurantDbContext dbContext)
	{
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .CustomAsync(async (value, context, cancellationToken) =>
            {
                var isEmailInUse = await dbContext.Users.AnyAsync(x => x.Email == value);

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
