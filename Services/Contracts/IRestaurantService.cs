using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services.Contracts;

public interface IRestaurantService
{
    Task<Restaurant> CreateAsync(CreateRestaurantDTO createRestaurantDto);
    Task<PageResponse<RestaurantDTO>> GetAllAsync(RestaurantQuery query);
    Task<RestaurantDTO> GetByIdAsync(int id);
    Task UpdateAsync(int id, ModifyRestaurantDTO modifyRestaurantDto);
    Task DeleteAsync(int id);
}