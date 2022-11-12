using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using AutoMapper;
using RestaurantAPI.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantAPI.Authorization;

namespace RestaurantAPI.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IMapper _mapper;
    private readonly ILogger<RestaurantService> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly RestaurantDbContext _dbContext;

    public RestaurantService(IMapper mapper,
                             ILogger<RestaurantService> logger,
                             IAuthorizationService authorizationService,
                             RestaurantDbContext dbContext)
    {
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
        _dbContext = dbContext;
    }

    public IEnumerable<RestaurantDto> GetAll()
    {
        var restaurants = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .ToList();

        if (restaurants is null)
            throw new NotFoundException("Restaurant not found...");

        var restaurantDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

        return restaurantDtos;
    }

    public RestaurantDto GetById(int id)
    {
        var restaurant = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found...");

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
        
        return restaurantDto;
    }

    public Restaurant Create(CreateRestaurantDto createRestaurantDto, int userId)
    {
        var restaurant = _mapper.Map<Restaurant>(createRestaurantDto);
        restaurant.CreatedById = userId;

        _dbContext.Restaurants.Add(restaurant);
        _dbContext.SaveChanges();

        return restaurant;
    }

    public void Update(int id, ModifyRestaurantDto modifyRestaurantDto, ClaimsPrincipal user)
    {
        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found...");

        var authorizationResult = _authorizationService.AuthorizeAsync(
            user,
            restaurant,
            new ResourceOperationRequirement(ResourceOperation.Update)
        ).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        _mapper.Map(modifyRestaurantDto, restaurant);
        _dbContext.SaveChanges();
    }

    public void Delete(int id, ClaimsPrincipal user)
    {
        _logger.LogError($"Restaurant with id: { id } DELETE action invoked");

        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var authorizationResult = _authorizationService.AuthorizeAsync(
            user,
            restaurant,
            new ResourceOperationRequirement(ResourceOperation.Delete)
        ).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        _dbContext.Restaurants.Remove(restaurant);
        _dbContext.SaveChanges();
    }
}
