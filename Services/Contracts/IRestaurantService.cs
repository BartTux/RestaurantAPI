using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services.Contracts;

public interface IRestaurantService
{
    Restaurant Create(CreateRestaurantDto createRestaurantDto);
    PageResponse<RestaurantDto> GetAll(RestaurantQuery query);
    RestaurantDto GetById(int id);
    void Update(int id, ModifyRestaurantDto modifyRestaurantDto);
    void Delete(int id);
}