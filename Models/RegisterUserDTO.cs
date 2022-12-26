namespace RestaurantAPI.Models;

public record RegisterUserDTO(string Email,
                              string Password,
                              string ConfirmPassword,
                              string FirstName,
                              string? LastName,
                              string Nationality,
                              DateTime? DateOfBirth,

                              int RoleId);