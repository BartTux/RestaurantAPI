namespace RestaurantAPI.Models;

public record RegisterUserDTO(string Email,
                              string Password,
                              string ConfirmPassword,
                              string FirstName,
                              string? LastName,
                              string Nationality,
                              DateTime? DateOfBirth,

                              int RoleId);

//public class RegisterUserDTO
//{
//    public string Email { get; set; }
//    public string Password { get; set; }
//    public string ConfirmPassword { get; set; }
//    public string FirstName { get; set; }
//    public string? LastName { get; set; }
//    public string Nationality { get; set; }
//    public DateTime? DateOfBirth { get; set; }

//    public int RoleId { get; set; } = 1;
//}
