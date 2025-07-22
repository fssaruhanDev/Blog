using System;
using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Infrastructure.Persistence.Context;
using Blog.Infrastructure.Persistence.EntityConfigurations.Interceptors;
using Blog.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Persistence.Extensions;

public static class Registration
{

    public static IServiceCollection AddInfastructureRegistration(this IServiceCollection services,IConfiguration configuration)
	{

		services.AddDbContext<EntityContext>(conf =>
		{

			var connectionString = configuration["ConnectionStrings:ConnectionString"];

			conf.UseSqlServer(connectionString, x =>
			{
				x.EnableRetryOnFailure();
			});

		});

		//var SeedData = new SeedData();

		//SeedData.SeedAsync(configuration).GetAwaiter().GetResult();

		services.AddScoped<IUserRepository, UserRepository>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<SavingChangesInterceptor>();


        // DbContext için interceptor ekleniyor
        services.AddDbContext<EntityContext>((serviceProvider, optionsBuilder) =>
        {
            var interceptor = serviceProvider.GetRequiredService<SavingChangesInterceptor>();
            optionsBuilder.AddInterceptors(interceptor);
        });
        return services;
	}
}

