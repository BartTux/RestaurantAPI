using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Profiles;

public class RestaurantMappingProfile : Profile
{
	public RestaurantMappingProfile()
	{
        CreateMap<Dish, DishDTO>();

        CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<ModifyRestaurantDTO, Restaurant>()
			.ForAllMembers(opts => opts.Condition((src, dest, value) => value is not null));

		CreateMap<CreateDishDTO, Dish>();

		CreateMap<RegisterUserDTO, User>();
	}
}
