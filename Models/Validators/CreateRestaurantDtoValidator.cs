using FluentValidation;

namespace RestaurantAPI.Models.Validators;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
	public CreateRestaurantDtoValidator()
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
