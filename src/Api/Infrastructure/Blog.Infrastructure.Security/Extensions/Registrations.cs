
using Blog.Api.Application.Interfaces.infractucture.Security;
using Blog.Infrastructure.Security.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Security.Extensions
{
    public static class Registrations
    {
        public static IServiceCollection AddJWTRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtProvider, TokenService>();

            return services;
        }
    }
}
