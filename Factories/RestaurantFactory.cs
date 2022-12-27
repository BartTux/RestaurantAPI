using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Factories;

public static class RestaurantFactory
{
    public static Restaurant Create(CreateRestaurantDTO createRestaurantDto, int userId)
        => new Restaurant
        {
            Name = createRestaurantDto.Name,
            Description = createRestaurantDto.Description,
            Category = createRestaurantDto.Category,
            HasDelivery = createRestaurantDto.HasDelivery,
            ContactEmail = createRestaurantDto.ContactEmail,
            ContactNumber = createRestaurantDto.ContactNumber,
            CreatedById = userId,
            Address = new Address
            {
                City = createRestaurantDto.City,
                Street = createRestaurantDto.Street,
                PostalCode = createRestaurantDto.PostalCode
            }
        };
    

    public static void Set(Restaurant restaurant, ModifyRestaurantDTO modifyRestaurantDto)
    {
        if (modifyRestaurantDto.Name is not null)
            restaurant.Name = modifyRestaurantDto.Name;

        if (modifyRestaurantDto.Description is not null)
            restaurant.Description = modifyRestaurantDto.Description;

        if (modifyRestaurantDto.Category is not null)
            restaurant.Category = modifyRestaurantDto.Category;

        if (modifyRestaurantDto.HasDelivery is not null)
            restaurant.HasDelivery = (bool)modifyRestaurantDto.HasDelivery;

        if (modifyRestaurantDto.ContactEmail is not null)
            restaurant.ContactEmail = modifyRestaurantDto.ContactEmail;

        if (modifyRestaurantDto.ContactNumber is not null)
            restaurant.ContactNumber = modifyRestaurantDto.ContactNumber;

        if (modifyRestaurantDto.City is not null)
            restaurant.Address.City = modifyRestaurantDto.City;

        if (modifyRestaurantDto.Street is not null)
            restaurant.Address.Street = modifyRestaurantDto.Street;

        if (modifyRestaurantDto.PostalCode is not null)
            restaurant.Address.PostalCode = modifyRestaurantDto.PostalCode;
    }
}
