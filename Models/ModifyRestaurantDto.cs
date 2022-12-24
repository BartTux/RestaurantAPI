﻿namespace RestaurantAPI.Models;

public record ModifyRestaurantDTO(string? Name,
                                  string? Description,
                                  string? Category,
                                  bool? HasDelivery,
                                  string? ContactEmail,
                                  string? ContactNumber);
