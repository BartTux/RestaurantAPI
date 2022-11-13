using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization;

public class ResourceOperationRequirementHandler 
    : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   ResourceOperationRequirement requirement,
                                                   Restaurant restaurant)
    {
        if (requirement.ResourceOperation
            is ResourceOperation.Create 
            or ResourceOperation.Read)
        {
            context.Succeed(requirement);
        }

        var userId = context.User
            .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;

        if (userId is null) 
            return Task.CompletedTask;

        if (restaurant.CreatedById == int.Parse(userId))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
