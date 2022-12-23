using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models;

public class ModifyRestaurantDto
{
    [MaxLength(30)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }

    [MaxLength(30)]
    public string? Category { get; set; }
    public bool? HasDelivery { get; set; }

    [MaxLength(50)]
    public string? ContactEmail { get; set; }

    [MaxLength(20)]
    public string? ContactNumber { get; set; }
}
