using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models;

public class ModifyRestaurantDto
{
    [MaxLength(25)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }
    public bool? HasDelivery { get; set; }
}
