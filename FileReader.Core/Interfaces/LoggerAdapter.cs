using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Core.Interfaces
{
    internal class LoggerAdapter<T> : IFileReaderLogger<T>
    {
        private ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }
        public void LogError(Exception? ex, string message, params object[] args)
        {
            
            _logger.LogError(ex, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }
    }
}
