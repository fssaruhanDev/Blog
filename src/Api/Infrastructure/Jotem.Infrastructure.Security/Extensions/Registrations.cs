
using Jotem.Api.Application.Interfaces.infractucture.Security;
using Jotem.Infrastructure.Security.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jotem.Infrastructure.Security.Extensions
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
