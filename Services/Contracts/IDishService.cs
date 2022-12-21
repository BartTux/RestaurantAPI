using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services.Contracts
{
    public interface IDishService
    {
        Task<IEnumerable<DishDto>> GetAllAsync(int restaurantId);
        Task<DishDto> GetAsync(int restaurantId, int dishId);
        Task<int> CreateAsync(int restaurantId, CreateDishDto dto);
        Task DeleteAsync(int restaurantId, int dishId);
        Task DeleteAllAsync(int restaurantId);
    }
}