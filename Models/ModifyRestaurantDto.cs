namespace RestaurantAPI.Models;

public record ModifyRestaurantDto(string? Name,
                                  string? Description,
                                  string? Category,
                                  bool? HasDelivery,
                                  string? ContactEmail,
                                  string? ContactNumber);
