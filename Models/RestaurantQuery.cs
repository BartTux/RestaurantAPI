namespace RestaurantAPI.Models;

public record RestaurantQuery(string? SearchPhraze,
                              int PageNumber,
                              int PageSize,
                              string? SortBy,
                              SortDirection SortDirection);