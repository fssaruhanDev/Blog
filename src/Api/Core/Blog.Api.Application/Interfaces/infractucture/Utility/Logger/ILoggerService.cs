using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Application.Interfaces.Infrastructure.Utility.Logger
{
    public interface ILoggerService
    {
        void LogDebug(string message, Dictionary<string, object>? properties = null);
        void LogInformation(string message, Dictionary<string, object>? properties = null);
        void LogWarning(string message, Dictionary<string, object>? properties = null);
        void LogError(string message, Exception? exception = null, Dictionary<string, object>? properties = null);
        void LogCritical(string message, Exception? exception = null, Dictionary<string, object>? properties = null);
    }
}

