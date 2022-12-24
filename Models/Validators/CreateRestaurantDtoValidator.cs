using FluentValidation;

namespace RestaurantAPI.Models.Validators;

public class CreateRestaurantDTOValidator : AbstractValidator<CreateRestaurantDTO>
{
	public CreateRestaurantDTOValidator()
	{
		RuleFor(x => x.Name)
			.MaximumLength(25);

		RuleFor(x => x.Category).NotNull();

		RuleFor(x => x.ContactEmail)
			.EmailAddress();

		RuleFor(x => x.ContactNumber).NotNull();

		RuleFor(x => x.City)
			.MaximumLength(50);

		RuleFor(x => x.Street)
			.MaximumLength(50);

        RuleFor(x => x.PostalCode).NotNull();
    }
}
