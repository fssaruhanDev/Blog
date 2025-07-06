
using Jotem.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jotem.Infrastructure.Utilities.Logger.Services
{
    public class SerilogLoggerService : ILoggerService
    {
        private readonly Serilog.ILogger _logger;

        public SerilogLoggerService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void LogDebug(string message, Dictionary<string, object>? properties = null)
        {
            WriteLog(LogEventLevel.Debug, message, null, properties);
        }

        public void LogInformation(string message, Dictionary<string, object>? properties = null)
        {
            WriteLog(LogEventLevel.Information, message, null, properties);
        }

        public void LogWarning(string message, Dictionary<string, object>? properties = null)
        {
            WriteLog(LogEventLevel.Warning, message, null, properties);
        }

        public void LogError(string message, Exception? exception = null, Dictionary<string, object>? properties = null)
        {
            WriteLog(LogEventLevel.Error, message, exception, properties);
        }

        public void LogCritical(string message, Exception? exception = null, Dictionary<string, object>? properties = null)
        {
            WriteLog(LogEventLevel.Fatal, message, exception, properties);
        }

        /// <summary>
        /// Serilog kütüphanesine uygun seviyede log yazan yardımcı metot.
        /// </summary>
        private void WriteLog(LogEventLevel level, string message, Exception? exception, Dictionary<string, object>? properties)
        {
            if (properties is { Count: > 0 })
            {
                // Log'a ek değerler ekliyoruz (structured logging)
                // "ForContext" ile ek property alanları ekleniyor
                var loggerWithProps = _logger.ForContext("CustomProperties", properties, destructureObjects: true);
                loggerWithProps.Write(level, exception, message);
            }
            else
            {
                _logger.Write(level, exception, message);
            }
        }
    }
}
