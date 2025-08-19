using System;
using AutoMapper;
using Blog.Api.Domain.Models;
using Blog.Common.Models.Queries;

namespace ECommerce.Api.Application.Mapping;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<User, LoginUserViewModel>()
			.ReverseMap();

	

    }
}

