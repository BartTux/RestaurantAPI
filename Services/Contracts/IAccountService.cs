using RestaurantAPI.Models;

namespace RestaurantAPI.Services.Contracts;

public interface IAccountService
{
    Task<string> LoginAsync(LoginDto loginDto);
    Task RegisterUserAsync(RegisterUserDto registerUserDto);
}