﻿namespace RestaurantAPI.Models;

public class RestaurantQuery
{
    public string? SearchPhraze { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }
}
