using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators;

public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
{
	private readonly int[] _allowedPageSizes = { 5, 10, 15 };

	private readonly string[] _allowedSortByColumnNames =
	{
		nameof(Restaurant.Name),
		nameof(Restaurant.Description),
		nameof(Restaurant.Category),
		nameof(Restaurant.Address.City),
		nameof(Restaurant.Address.Street)
	};

	public RestaurantQueryValidator()
	{
		RuleFor(r => r.PageNumber)
			.GreaterThanOrEqualTo(1);

		RuleFor(r => r.PageSize).Custom((value, context) =>
		{
			if (!_allowedPageSizes.Contains(value))
				context.AddFailure("PageSize", $"PageSize must be in [{ string.Join(", ", _allowedPageSizes) }]");
		});

		RuleFor(r => r.SortBy)
			.Must(value => _allowedSortByColumnNames.Contains(value))
			.WithMessage($"SortBy is optional or must be in [{ string.Join(", ", _allowedSortByColumnNames) }]")
			.When(r => r.SortBy is not null);
	}
}
