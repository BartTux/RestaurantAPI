using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services.Contracts;

public interface IRestaurantService
{
    Restaurant Create(CreateRestaurantDto createRestaurantDto, int userId);
    IEnumerable<RestaurantDto>? GetAll();
    RestaurantDto? GetById(int id);
    void Update(int id, ModifyRestaurantDto modifyRestaurantDto, ClaimsPrincipal user);
    void Delete(int id, ClaimsPrincipal user);
}