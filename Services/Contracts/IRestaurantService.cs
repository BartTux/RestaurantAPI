using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services.Contracts;

public interface IRestaurantService
{
    Task<Restaurant> CreateAsync(CreateRestaurantDto createRestaurantDto);
    Task<PageResponse<RestaurantDto>> GetAllAsync(RestaurantQuery query);
    Task<RestaurantDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, ModifyRestaurantDto modifyRestaurantDto);
    Task DeleteAsync(int id);
}