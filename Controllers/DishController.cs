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
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAll([FromRoute] int restaurantId)
    {
        var dishes = await _dishService.GetAllAsync(restaurantId);
        return Ok(dishes);
    }

    [HttpGet("{dishId}")]
    public async Task<ActionResult<DishDto>> Get([FromRoute] int restaurantId,
                                                 [FromRoute] int dishId)
    {
        var dish = await _dishService.GetAsync(restaurantId, dishId);
        return Ok(dish);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] int restaurantId,
                                            [FromBody] CreateDishDto dto)
    {
        var dishId = await _dishService.CreateAsync(restaurantId, dto);
        return Created($"api/restaurant/{ restaurantId }/dish/{ dishId }", null);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll([FromRoute] int restaurantId)
    {
        await _dishService.DeleteAllAsync(restaurantId);
        return NoContent();
    }

    [HttpDelete("{dishId}")]
    public async Task<IActionResult> Delete([FromRoute] int restaurantId,
                                            [FromRoute] int dishId)
    {
        await _dishService.DeleteAsync(restaurantId, dishId);
        return NoContent();
    }
}