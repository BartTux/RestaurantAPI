using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Profiles;

public class RestaurantMappingProfile : Profile
{
	public RestaurantMappingProfile()
	{
        CreateMap<Dish, DishDto>();

		CreateMap<CreateRestaurantDto, Restaurant>()
			.ForMember(r => r.Address, c => c.MapFrom(dto => new Address 
			{ 
				City = dto.City,
				Street = dto.Street,
				PostalCode = dto.PostalCode
			}));

        CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<ModifyRestaurantDto, Restaurant>()
			.ForAllMembers(opts => opts.Condition((src, dest, value) => value is not null));

		CreateMap<CreateDishDto, Dish>();

		CreateMap<RegisterUserDto, User>();
	}
}
