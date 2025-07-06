using System;
using AutoMapper;
using Jotem.Api.Domain.Models;
using Jotem.Common.Models.Queries;

namespace ECommerce.Api.Application.Mapping;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<User, LoginUserViewModel>()
			.ReverseMap();

	

    }
}

