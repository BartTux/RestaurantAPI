using RestaurantAPI.Models;

namespace RestaurantAPI.Services.Contracts;

public interface IAccountService
{
    Task<string> LoginAsync(LoginDTO loginDTO);
    Task RegisterUserAsync(RegisterUserDTO registerUserDTO);
}