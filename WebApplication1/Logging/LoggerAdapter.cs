using Microsoft.Extensions.Logging;
using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;
        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();

        }

        public void LogWarning(string message, params object[] args)
        {
            // overriding default Logger.LogWarning() 
            _logger.LogWarning(message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            // overriding default Logger.LogInformation() 
            _logger.LogInformation(message, args);
        }
    }
}
