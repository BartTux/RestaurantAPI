using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Factories;

public static class DishFactory
{
    public static Dish Create(CreateDishDTO createDishDto, int restaurantId) 
        => new Dish 
        {
            Name = createDishDto.Name,
            Description = createDishDto.Description,
            Price = createDishDto.Price,
            RestaurantId = restaurantId
        };
}
