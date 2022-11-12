using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Contracts;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using AutoMapper;

namespace RestaurantAPI.Services;

public class DishService : IDishService
{
    private readonly IMapper _mapper;
    private readonly ILogger<DishService> _logger;
    private readonly RestaurantDbContext _dbContext;

    public DishService(IMapper mapper, 
                       ILogger<DishService> logger, 
                       RestaurantDbContext dbContext)
    {
        _mapper = mapper;
        _logger = logger;
        _dbContext = dbContext;
    }

    public IEnumerable<DishDto> GetAll(int restaurantId)
    {
        var restaurant = GetRestaurantById(restaurantId);

        var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);
        return dishDtos;
    }

    public DishDto Get(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurantById(restaurantId);
        var dish = GetDishById(restaurant, dishId);

        var dishDto = _mapper.Map<DishDto>(dish);
        return dishDto;
    }

    public int Create(int restaurantId, CreateDishDto createDishDto)
    {
        var restaurant = GetRestaurantById(restaurantId);
        var dish = _mapper.Map<Dish>(createDishDto);

        dish.RestaurantId = restaurantId;

        restaurant.Dishes.Add(dish);
        _dbContext.SaveChanges();

        return dish.Id;
    }

    public void DeleteAll(int restaurantId)
    {
        _logger.LogError($"All dishes on Restaurant with id: { restaurantId } "
            + $"DELETE action invoked");

        var restaurant = GetRestaurantById(restaurantId);

        restaurant.Dishes.Clear();
        _dbContext.SaveChanges();
    }

    public void Delete(int restaurantId, int dishId)
    {
        _logger.LogError($"Dish with id: { dishId } on "
            + $"Restaurant with id: { restaurantId } DELETE action invoked");

        var restaurant = GetRestaurantById(restaurantId);
        var dish = GetDishById(restaurant, dishId);

        restaurant.Dishes.Remove(dish);
        _dbContext.SaveChanges();
    }

    private Restaurant GetRestaurantById(int restaurantId)
    {
        var restaurant = _dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == restaurantId);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found...");

        return restaurant;
    }

    private Dish GetDishById(Restaurant restaurant, int dishId)
    {
        var dish = restaurant.Dishes
            .FirstOrDefault(d => d.Id == dishId);

        if (dish is null)
            throw new NotFoundException("Dish not found...");

        return dish;
    }
}