using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Contracts;

namespace RestaurantAPI.Controllers;

[ApiController]
[Route("api/restaurant/{restaurantId}/dish")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<DishDto>> GetAll([FromRoute] int restaurantId) 
    {
        var dishes = _dishService.GetAll(restaurantId);
        return Ok(dishes);
    }

    [HttpGet("{dishId}")]
    public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = _dishService.Get(restaurantId, dishId);
        return Ok(dish);
    }

    [HttpPost]
    public IActionResult Create([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
    {
        var dishId = _dishService.Create(restaurantId, dto);
        return Created($"api/restaurant/{ restaurantId }/dish/{ dishId }", null);
    }

    [HttpDelete]
    public IActionResult DeleteAll([FromRoute] int restaurantId)
    {
        _dishService.DeleteAll(restaurantId);
        return NoContent();
    }

    [HttpDelete("{dishId}")]
    public IActionResult Delete([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        _dishService.Delete(restaurantId, dishId);
        return NoContent();
    }
}
