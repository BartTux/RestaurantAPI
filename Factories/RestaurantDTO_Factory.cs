using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Factories;

public static class RestaurantDTO_Factory
{
    public static RestaurantDTO Create(Restaurant restaurant)
        => new RestaurantDTO
        (
            Id: restaurant.Id,
            Name: restaurant.Name,
            Description: restaurant.Description,
            Category: restaurant.Category,
            HasDelivery: restaurant.HasDelivery,
            City: restaurant.Address.City,
            Street: restaurant.Address.Street,
            PostalCode: restaurant.Address.PostalCode,
            Dishes: restaurant.Dishes
                .Select(d => new DishDTO(d.Id, d.Name, d.Description, d.Price))
                .ToList()
        );
    
}
