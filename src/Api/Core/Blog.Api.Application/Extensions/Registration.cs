using System;
using System.Reflection;
using FluentValidation;
using Blog.Api.Application.Interfaces.infractucture.Utility.Cache;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Api.Application.Extensions;

public static class Registration
{


    public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
    {
        var assm = Assembly.GetExecutingAssembly();

        services.AddMediatR(assm);
        services.AddAutoMapper(assm);
        services.AddValidatorsFromAssembly(assm);



        return services;
    }


}

