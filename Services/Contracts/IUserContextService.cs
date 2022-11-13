using System.Security.Claims;

namespace RestaurantAPI.Services.Contracts
{
    public interface IUserContextService
    {
        ClaimsPrincipal? User { get; }
        int? UserId { get; }
    }
}