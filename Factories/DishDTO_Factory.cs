using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Factories;

public static class DishDTO_Factory
{
    public static DishDTO Create(Dish dish)
        => new DishDTO(dish.Id, dish.Name, dish.Description, dish.Price);
}
