using System;
using System.Globalization;
using Bogus;
using Blog.Api.Domain.Models;
using Blog.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.Persistence.Context;

internal class SeedData
{
	private static List<User> GetUsers()
	{
        var result = new Faker<User>(locale:"tr")
            .RuleFor(i => i.ID, i => Guid.NewGuid())
            .RuleFor(i => i.CreatedDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
            .RuleFor(i => i.Avatar, i => i.Internet.Avatar())
            .RuleFor(i => i.FirstName, i => i.Person.FirstName)
            .RuleFor(i => i.LastName, i => i.Person.LastName)
            .RuleFor(i => i.EmailAddress, i => i.Internet.Email())
            .RuleFor(i => i.UserName, i => i.Internet.UserName())
			.RuleFor(i => i.Password, i => PasswordEncryptor.Encrypt( i.Internet.Password()))
			.RuleFor(i => i.EmailConfirmed, i => i.PickRandom(true, false))
			.Generate(100);

		return result;
    }


	public async Task SeedAsync(IConfiguration configuration)
	{
        try
        {

            var dbContextBuilder = new DbContextOptionsBuilder();
            dbContextBuilder.UseSqlServer(configuration["ConnectionStrings"]);
            var context = new EntityContext(dbContextBuilder.Options);
            var users = GetUsers();
            var userIds = users.Select(i => i.ID);
            await context.Users.AddRangeAsync(users);

            await context.SaveChangesAsync();
           

        }
        catch (CultureNotFoundException ex)
        {
            Console.WriteLine($"Hata: {ex.Message}, Invalid Culture Name: {ex.InvalidCultureName}");
        }
       
    }
}

