using System;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Persistence.Context;
using Blog.Infrastructure.Persistence.EntityConfigurations.Interceptors;
using Blog.Infrastructure.Persistence.Repository;
using Blog.Infrastructure.Persistence.Repostory;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Persistence.Extensions;

public static class Registration
{

	public static IServiceCollection AddInfastructureRegistration(this IServiceCollection services,IConfiguration configuration)
	{

		//var SeedData = new SeedData();

		//SeedData.SeedAsync(configuration).GetAwaiter().GetResult();

	services.AddScoped<IUserRepository, UserRepository>();
	services.AddScoped<IPostRepository, PostRepository>();
	services.AddScoped<ICategoryRepository, CategoryRepository>();
	services.AddScoped<ITagRepository, TagRepository>();
	services.AddScoped<INewsRepository, NewsRepository>();
	services.AddScoped<IProjectRepository, ProjectRepository>();
	services.AddScoped<IProjectTagRepository, ProjectTagRepository>();
	services.AddScoped(typeof(IGenericRepository<>), typeof(Blog.Infrastructure.Persistence.Repostory.GenericRepository<>));
		// Map DbContext -> EntityContext for repositories expecting DbContext
		services.AddScoped<DbContext>(sp => sp.GetRequiredService<EntityContext>());

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<SavingChangesInterceptor>();

		// DbContext registration: configure provider and add interceptor
		services.AddDbContext<EntityContext>((serviceProvider, optionsBuilder) =>
		{
			var connectionString = configuration["ConnectionStrings:ConnectionString"];
			optionsBuilder.UseSqlServer(connectionString, x => x.EnableRetryOnFailure());

			var interceptor = serviceProvider.GetRequiredService<SavingChangesInterceptor>();
			optionsBuilder.AddInterceptors(interceptor);
		});
        return services;
	}
}

