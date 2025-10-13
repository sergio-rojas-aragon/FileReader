using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Core.Interfaces
{
    public interface IFileReaderLogger<T>
    {
        void LogInformation(string message, params object[] args);
        void LogError(Exception? ex, string message, params object[] args);
    }
}
