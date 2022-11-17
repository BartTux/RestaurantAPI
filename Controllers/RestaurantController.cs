using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Contracts;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
[ApiController]
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
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        var restaurants = _restaurantService.GetAll();
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "HasNationality")]
    public ActionResult<RestaurantDto> Get([FromRoute] int id)
    {
        var restaurant = _restaurantService.GetById(id);
        return Ok(restaurant);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create([FromBody] CreateRestaurantDto createRestaurantDto)
    {
        var restaurant = _restaurantService.Create(createRestaurantDto);
        return Created($"api/restaurant/{ restaurant.Id }", null);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        _restaurantService.Delete(id);
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, 
                                [FromBody] ModifyRestaurantDto modifyRestaurantDto)
    {
        _restaurantService.Update(id, modifyRestaurantDto);
        return NoContent();
    }
}
