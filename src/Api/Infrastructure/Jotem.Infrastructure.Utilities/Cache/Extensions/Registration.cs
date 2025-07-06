using Jotem.Api.Application.Interfaces.infractucture.Utility.Cache;
using Jotem.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Jotem.Infrastructure.Utilities.Cache.MemoryCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jotem.Infrastructure.Utilities.Cache.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddCacheRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache(); // Microsoft.Extensions.DependencyInjection içinde gelir
            services.AddSingleton(typeof(ICache<>), typeof(MemoryCaches<>));
            return services;
        }
    }
}
