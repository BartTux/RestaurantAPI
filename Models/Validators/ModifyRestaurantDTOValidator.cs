using FluentValidation;

namespace RestaurantAPI.Models.Validators;

public class ModifyRestaurantDTOValidator : AbstractValidator<ModifyRestaurantDTO>
{
	public ModifyRestaurantDTOValidator()
	{
		RuleFor(x => x.Name).MaximumLength(50);
		RuleFor(x => x.Description).MaximumLength(100);
        RuleFor(x => x.Category).MaximumLength(100);
        RuleFor(x => x.ContactEmail).MaximumLength(50);
        RuleFor(x => x.ContactNumber).MaximumLength(30);

        RuleFor(x => x.City).MaximumLength(20);
        RuleFor(x => x.Street).MaximumLength(30);
        RuleFor(x => x.PostalCode).MaximumLength(8);
    }
}
