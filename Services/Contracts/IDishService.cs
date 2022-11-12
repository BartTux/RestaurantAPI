using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services.Contracts
{
    public interface IDishService
    {
        IEnumerable<DishDto> GetAll(int restaurantId);
        DishDto Get(int restaurantId, int dishId);
        int Create(int restaurantId, CreateDishDto dto);
        void Delete(int restaurantId, int dishId);
        void DeleteAll(int restaurantId);
    }
}