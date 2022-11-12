using RestaurantAPI.Models;

namespace RestaurantAPI.Services.Contracts;

public interface IAccountService
{
    string Login(LoginDto loginDto);
    void RegisterUser(RegisterUserDto registerUserDto);
}