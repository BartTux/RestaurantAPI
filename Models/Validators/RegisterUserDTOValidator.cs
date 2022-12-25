using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
{
	public RegisterUserDTOValidator(RestaurantDbContext dbContext)
	{
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(30)
            .CustomAsync(async (value, context, cancellationToken) =>
            {
                var isEmailInUse = await dbContext.Users.AnyAsync(x => x.Email == value);

                if (isEmailInUse)
                    context.AddFailure("E-mail", "This e-mail address is already taken");
            });

        RuleFor(x => x.Password).MinimumLength(8);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);

        RuleFor(x => x.FirstName).MaximumLength(20);
        RuleFor(x => x.LastName).MaximumLength(30);
        RuleFor(x => x.Nationality).MaximumLength(20);
    }
}
