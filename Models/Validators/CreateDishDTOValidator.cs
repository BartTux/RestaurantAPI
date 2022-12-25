using FluentValidation;

namespace RestaurantAPI.Models.Validators;

public class CreateDishDTOValidator : AbstractValidator<CreateDishDTO>
{
	public CreateDishDTOValidator()
	{
		RuleFor(x => x.Name).MaximumLength(30);
		RuleFor(x => x.Description).MaximumLength(100);
	}
}
