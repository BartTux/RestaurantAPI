namespace RestaurantAPI.Models;

public record CreateDishDTO(string Name, string? Description, decimal Price);