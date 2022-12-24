namespace RestaurantAPI.Models;

public record RestaurantDTO(int Id,
                            string Name,
                            string? Description,
                            string Category,
                            bool HasDelivery,

                            string City,
                            string Street,
                            string PostalCode,

                            List<DishDTO> Dishes);
