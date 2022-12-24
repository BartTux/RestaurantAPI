using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Contracts;
using RestaurantAPI.Authorization;
using AutoMapper;
using System.Linq.Expressions;

namespace RestaurantAPI.Services;

public class RestaurantService : IRestaurantService
{
    private readonly ILogger<RestaurantService> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContextService _userContextService;
    private readonly RestaurantDbContext _dbContext;

    public RestaurantService(ILogger<RestaurantService> logger,
                             IAuthorizationService authorizationService,
                             IUserContextService userContextService,
                             RestaurantDbContext dbContext)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _userContextService = userContextService;
        _dbContext = dbContext;
    }

    public async Task<PageResponse<RestaurantDTO>> GetAllAsync(RestaurantQuery query)
    {
        var baseQuery = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .Where(r => query.SearchPhraze == null
                || r.Name.ToLower().Contains(query.SearchPhraze.ToLower())
                || r.Description.ToLower().Contains(query.SearchPhraze.ToLower())
                || r.Dishes.Any(d => d.Name.ToLower().Contains(query.SearchPhraze.ToLower()))
                || r.Address.City.ToLower().Contains(query.SearchPhraze.ToLower()));

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var columnsDictionary = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category },
                { nameof(Restaurant.Address.City), r => r.Address.City },
                { nameof(Restaurant.Address.Street), r => r.Address.Street }
            };

            var selectedColumn = columnsDictionary[query.SortBy];

            baseQuery = query.SortDirection is SortDirection.ASC
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var recordsNumber = await baseQuery.CountAsync();
        var restaurants = await baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToListAsync()
            ?? throw new NotFoundException("Restaurant not found...");

        var restaurantsDto = restaurants.Select(r => new RestaurantDTO
        (
            Id: r.Id,
            Name: r.Name,
            Description: r.Description,
            Category: r.Category,
            HasDelivery: r.HasDelivery,
            City: r.Address.City,
            Street: r.Address.Street,
            PostalCode: r.Address.PostalCode,
            Dishes: r.Dishes
                .Select(d => new DishDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price
                })
                .ToList()
        ));

        var restaurantResults = new PageResponse<RestaurantDTO>(
            restaurantsDto,
            recordsNumber,
            query.PageSize, 
            query.PageNumber);

        return restaurantResults;
    }

    public async Task<RestaurantDTO> GetByIdAsync(int id)
    {
        var restaurant = await _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id)
            ?? throw new NotFoundException("Restaurant not found...");

        var restaurantDto = new RestaurantDTO
        (
            Id: restaurant.Id,
            Name: restaurant.Name,
            Description: restaurant.Description,
            Category: restaurant.Category,
            HasDelivery: restaurant.HasDelivery,
            City: restaurant.Address.City,
            Street: restaurant.Address.Street,
            PostalCode: restaurant.Address.PostalCode,
            Dishes: restaurant.Dishes
                .Select(d => new DishDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price
                })
                .ToList()
        );

        return restaurantDto;
    }

    public async Task<Restaurant> CreateAsync(CreateRestaurantDTO createRestaurantDto)
    {
        var restaurant = new Restaurant
        {
            Name = createRestaurantDto.Name,
            Description = createRestaurantDto.Description,
            Category = createRestaurantDto.Category,
            HasDelivery = createRestaurantDto.HasDelivery,
            ContactEmail = createRestaurantDto.ContactEmail,
            ContactNumber = createRestaurantDto.ContactNumber,
            CreatedById = _userContextService.UserId, // where user context service is from?
            Address = new Address
            {
                City = createRestaurantDto.City,
                Street = createRestaurantDto.Street,
                PostalCode = createRestaurantDto.PostalCode
            }
        };

        await _dbContext.Restaurants.AddAsync(restaurant);
        await _dbContext.SaveChangesAsync();

        return restaurant;
    }

    public async Task UpdateAsync(int id, ModifyRestaurantDTO modifyRestaurantDto)
    {
        var restaurant = await _dbContext
            .Restaurants
            .FirstOrDefaultAsync(r => r.Id == id)
            ?? throw new NotFoundException("Restaurant not found...");

        var authorizationResult = _authorizationService
            .AuthorizeAsync(
                _userContextService.User,
                restaurant,
                new ResourceOperationRequirement(ResourceOperation.Update))
            .Result;

        if (!authorizationResult.Succeeded) 
            throw new ForbidException();

        if(modifyRestaurantDto.Name is not null) 
            restaurant.Name = modifyRestaurantDto.Name;

        if(modifyRestaurantDto.Description is not null) 
            restaurant.Description = modifyRestaurantDto.Description;

        if(modifyRestaurantDto.Category is not null) 
            restaurant.Category = modifyRestaurantDto.Category;

        if(modifyRestaurantDto.HasDelivery is not null) 
            restaurant.HasDelivery = (bool)modifyRestaurantDto.HasDelivery;

        if(modifyRestaurantDto.ContactEmail is not null) 
            restaurant.ContactEmail = modifyRestaurantDto.ContactEmail;

        if(modifyRestaurantDto.ContactNumber is not null) 
            restaurant.ContactNumber = modifyRestaurantDto.ContactNumber;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogError($"Restaurant with id: { id } DELETE action invoked");

        var restaurant = await _dbContext
            .Restaurants
            .FirstOrDefaultAsync(r => r.Id == id)
            ?? throw new NotFoundException("Restaurant not found");

        var authorizationResult = _authorizationService
            .AuthorizeAsync(
                _userContextService.User,
                restaurant,
                new ResourceOperationRequirement(ResourceOperation.Delete))
            .Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        _dbContext.Restaurants.Remove(restaurant);
        await _dbContext.SaveChangesAsync();
    }
}