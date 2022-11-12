using Microsoft.AspNetCore.Authorization;
using NLog;
using System.Security.Claims;

namespace RestaurantAPI.Authorization;

public class MinAgeRequirementHandler : AuthorizationHandler<MinAgeRequirement>
{
    private readonly ILogger<MinAgeRequirementHandler> _logger;

    public MinAgeRequirementHandler(ILogger<MinAgeRequirementHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
                                                   MinAgeRequirement requirement)
    {
        var dateOfBirthClaimValue = context.User.FindFirst(c => c.Type == "DateOfBirth")?.Value;
        var userEmailClaimValue = context.User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;

        _logger.LogInformation($"User: { userEmailClaimValue } with date of birth: [{dateOfBirthClaimValue}]");

        if (string.IsNullOrEmpty(dateOfBirthClaimValue))
        {
            _logger.LogInformation("Authorization failed");
            return Task.CompletedTask;
        }

        var dateOfBirth = DateTime.Parse(dateOfBirthClaimValue);

        if (dateOfBirth.AddYears(requirement.MinAge) > DateTime.Today)
        {
            _logger.LogInformation("Authorization failed");
            return Task.CompletedTask;
        }

        _logger.LogInformation("Authorization succedded");
        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
