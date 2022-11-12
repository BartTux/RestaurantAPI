using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization;

public class MinAgeRequirement : IAuthorizationRequirement
{
    public int MinAge { get; }

    public MinAgeRequirement(int minAge)
    {
        MinAge = minAge;
    }
}
