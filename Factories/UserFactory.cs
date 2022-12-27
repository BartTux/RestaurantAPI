using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Factories;

public static class UserFactory
{
    public static User Create(RegisterUserDTO registerUserDto)
        => new User
        {
            Email = registerUserDto.Email,
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            DateOfBirth = registerUserDto.DateOfBirth,
            Nationality = registerUserDto.Nationality,
            RoleId = registerUserDto.RoleId
        };
}
