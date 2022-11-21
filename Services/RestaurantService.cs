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
    private readonly IMapper _mapper;
    private readonly ILogger<RestaurantService> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContextService _userContextService;
    private readonly RestaurantDbContext _dbContext;

    public RestaurantService(IMapper mapper,
                             ILogger<RestaurantService> logger,
                             IAuthorizationService authorizationService,
                             IUserContextService userContextService,
                             RestaurantDbContext dbContext)
    {
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
        _userContextService = userContextService;
        _dbContext = dbContext;
    }

    public PageResponse<RestaurantDto> GetAll(RestaurantQuery query)
    {
        var baseQuery = _dbContext
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .Where(r => query.SearchPhraze == null
                || (r.Name.ToLower().Contains(query.SearchPhraze.ToLower())
                || r.Description.ToLower().Contains(query.SearchPhraze.ToLower())
                || r.Dishes.Any(d => d.Name.ToLower().Contains(query.SearchPhraze.ToLower()))
                || r.Address.City.ToLower().Contains(query.SearchPhraze.ToLower()))
            );

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

        var restaurants = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        if (restaurants is null)
            throw new NotFoundException("Restaurant not found...");

        var restaurantDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

        var restaurantResults = new PageResponse<RestaurantDto>(
            restaurantDtos,
            baseQuery.Count(),
            query.PageSize, 
            query.PageNumber
        );

        return restaurantResults;
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

    public Restaurant Create(CreateRestaurantDto createRestaurantDto)
    {
        var restaurant = _mapper.Map<Restaurant>(createRestaurantDto);
        restaurant.CreatedById = _userContextService.UserId;

        _dbContext.Restaurants.Add(restaurant);
        _dbContext.SaveChanges();

        return restaurant;
    }

    public void Update(int id, ModifyRestaurantDto modifyRestaurantDto)
    {
        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found...");

        var authorizationResult = _authorizationService.AuthorizeAsync(
            _userContextService.User,
            restaurant,
            new ResourceOperationRequirement(ResourceOperation.Update)
        ).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        _mapper.Map(modifyRestaurantDto, restaurant);
        _dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        _logger.LogError($"Restaurant with id: { id } DELETE action invoked");

        var restaurant = _dbContext
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var authorizationResult = _authorizationService.AuthorizeAsync(
            _userContextService.User,
            restaurant,
            new ResourceOperationRequirement(ResourceOperation.Delete)
        ).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        _dbContext.Restaurants.Remove(restaurant);
        _dbContext.SaveChanges();
    }
}
