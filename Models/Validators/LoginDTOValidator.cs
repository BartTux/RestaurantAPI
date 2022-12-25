using FluentValidation;

namespace RestaurantAPI.Models.Validators;

public class LoginDTOValidator : AbstractValidator<LoginDTO>
{
	public LoginDTOValidator()
	{
		RuleFor(x => x.Email).MaximumLength(30);
	}
}
