using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services.Contracts
{
    public interface IDishService
    {
        Task<IEnumerable<DishDTO>> GetAllAsync(int restaurantId);
        Task<DishDTO> GetAsync(int restaurantId, int dishId);
        Task<int> CreateAsync(int restaurantId, CreateDishDTO dto);
        Task DeleteAsync(int restaurantId, int dishId);
        Task DeleteAllAsync(int restaurantId);
    }
}