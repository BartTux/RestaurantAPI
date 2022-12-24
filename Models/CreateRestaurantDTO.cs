namespace RestaurantAPI.Models;

public record CreateRestaurantDTO(string Name,
                                  string? Description,
                                  string Category,
                                  bool HasDelivery,
                                  string ContactEmail,
                                  string ContactNumber,
                                  string City,
                                  string Street,
                                  string PostalCode);