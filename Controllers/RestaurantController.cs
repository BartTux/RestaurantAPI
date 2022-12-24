using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Contracts;

namespace RestaurantAPI.Controllers;

[ApiController]
[Route("api/restaurant")]
[Authorize]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }
    
    [HttpGet]
    [Authorize(Policy = "AtLeast2RestaurantsCreated")]
    public async Task<ActionResult<PageResponse<RestaurantDTO>>> GetAll(
        [FromQuery] RestaurantQuery query)
    {
        var restaurants = await _restaurantService.GetAllAsync(query);
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "HasNationality")]
    public async Task<ActionResult<RestaurantDTO>> Get([FromRoute] int id)
    {
        var restaurant = await _restaurantService.GetByIdAsync(id);
        return Ok(restaurant);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantDTO createRestaurantDto)
    {
        var restaurant = await _restaurantService.CreateAsync(createRestaurantDto);
        return Created($"api/restaurant/{ restaurant.Id }", null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _restaurantService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, 
                                            [FromBody] ModifyRestaurantDTO modifyRestaurantDto)
    {
        await _restaurantService.UpdateAsync(id, modifyRestaurantDto);
        return NoContent();
    }
}
