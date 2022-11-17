using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization;

public class MinimumRestaurantsCreatedRequirementHandler
    : AuthorizationHandler<MinimumRestaurantsCreatedRequirement>
{
    private readonly RestaurantDbContext _dbContext;

    public MinimumRestaurantsCreatedRequirementHandler(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   MinimumRestaurantsCreatedRequirement requirement)
    {
        var userId = context.User
            .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;

        if (userId is null)
            return Task.CompletedTask;

        var restaurantsCount = _dbContext.Restaurants
            .Count(r => r.CreatedById == int.Parse(userId));

        if (restaurantsCount >= requirement.CreatedRestaurants)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
