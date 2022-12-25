using FluentValidation;

namespace RestaurantAPI.Models.Validators;

public class CreateRestaurantDTOValidator : AbstractValidator<CreateRestaurantDTO>
{
	public CreateRestaurantDTOValidator()
	{
		RuleFor(x => x.Name).MaximumLength(30);

		RuleFor(x => x.Description).MaximumLength(100);

		RuleFor(x => x.Category).MaximumLength(20);

		RuleFor(x => x.ContactEmail)
			.EmailAddress()
			.MaximumLength(30);

		RuleFor(x => x.ContactNumber).MaximumLength(15);

		RuleFor(x => x.City).MaximumLength(20);

		RuleFor(x => x.Street).MaximumLength(30);

		RuleFor(x => x.PostalCode).MaximumLength(8);
    }
}
