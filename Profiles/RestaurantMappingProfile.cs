﻿using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Profiles;

public class RestaurantMappingProfile : Profile
{
	public RestaurantMappingProfile()
	{
		CreateMap<Restaurant, RestaurantDto>()
			.ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
			.ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
			.ForMember(m => m.PostalCode, c => c.MapFrom(p => p.Address.PostalCode));

		CreateMap<Dish, DishDto>();

		CreateMap<CreateRestaurantDto, Restaurant>()
			.ForMember(r => r.Address, c => c.MapFrom(dto => new Address 
			{ 
				City = dto.City,
				Street = dto.Street,
				PostalCode = dto.PostalCode
			}));

		CreateMap<ModifyRestaurantDto, Restaurant>();

		CreateMap<CreateDishDto, Dish>();

		CreateMap<RegisterUserDto, User>();

		//CreateMap<LoginDto, User>();
	}
}
