using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization;

public class MinimumRestaurantsCreatedRequirement : IAuthorizationRequirement
{
    public int CreatedRestaurants { get; }

    public MinimumRestaurantsCreatedRequirement(int createdRestaurants)
    {
        CreatedRestaurants = createdRestaurants;
    }
}
