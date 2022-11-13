using System.Security.Claims;
using RestaurantAPI.Services.Contracts;

namespace RestaurantAPI.Services;

public class UserContextService : IUserContextService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

	public int? UserId => User is null
		? null
		: int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

	public UserContextService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}
}
