using Blog.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Infrastructure.Utilities.Logger.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Utilities.Logger.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddLoggerRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            // Serilog Logger'ı oluştur
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration) // Config dosyasından oku
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Mevcut logging provider'ları temizle ve Serilog'u ekle
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog();
            });

            // ILoggerService için SerilogLoggerService kullan
            services.AddSingleton<ILoggerService>(provider =>
                new SerilogLoggerService(Log.Logger));

            return services;
        }
    }
}
