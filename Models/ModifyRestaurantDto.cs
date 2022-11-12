using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models;

public class ModifyRestaurantDto
{
    [Required]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool HasDelivery { get; set; }
}
