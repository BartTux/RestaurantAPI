using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Profiles;

public class RestaurantMappingProfile : Profile
{
	public RestaurantMappingProfile()
	{
		CreateMap<RegisterUserDTO, User>();
	}
}
