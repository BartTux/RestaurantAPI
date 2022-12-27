using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Contracts;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Factories;

namespace RestaurantAPI.Services;

public class DishService : IDishService
{
    private readonly ILogger<DishService> _logger;
    private readonly RestaurantDbContext _dbContext;

    public DishService(ILogger<DishService> logger, RestaurantDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<DishDTO>> GetAllAsync(int restaurantId)
    {
        var restaurant = await GetRestaurantByIdAsync(restaurantId);

        var dishesDto = restaurant.Dishes.Select(d => DishDTO_Factory.Create(d)); 
        return dishesDto;
    }

    public async Task<DishDTO> GetAsync(int restaurantId, int dishId)
    {
        var restaurant = await GetRestaurantByIdAsync(restaurantId);
        var dish = GetDishById(restaurant, dishId);

        var dishDto = DishDTO_Factory.Create(dish);
        return dishDto;
    }

    public async Task<int> CreateAsync(int restaurantId, CreateDishDTO createDishDto)
    {
        var restaurant = await GetRestaurantByIdAsync(restaurantId);

        var dish = DishFactory.Create(createDishDto, restaurantId);

        restaurant.Dishes.Add(dish);
        await _dbContext.SaveChangesAsync();

        return dish.Id;
    }

    public async Task DeleteAllAsync(int restaurantId)
    {
        _logger.LogError($"All dishes on Restaurant with id: { restaurantId } "
            + $"DELETE action invoked");

        var restaurant = await GetRestaurantByIdAsync(restaurantId);

        restaurant.Dishes.Clear();
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int restaurantId, int dishId)
    {
        _logger.LogError($"Dish with id: { dishId } on "
            + $"Restaurant with id: { restaurantId } DELETE action invoked");

        var restaurant = await GetRestaurantByIdAsync(restaurantId);
        var dish = GetDishById(restaurant, dishId);
        
        restaurant.Dishes.Remove(dish);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<Restaurant> GetRestaurantByIdAsync(int restaurantId) 
        => await _dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == restaurantId)
            ?? throw new NotFoundException("Restaurant not found...");
    
    private Dish GetDishById(Restaurant restaurant, int dishId) 
        => restaurant.Dishes
            .FirstOrDefault(d => d.Id == dishId)
            ?? throw new NotFoundException("Dish not found...");
}