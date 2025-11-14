using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Core.Interfaces
{
    public interface IFolderPath 
    {
        public string LogPath { get; init; } 
        public string ProcessPath { get; init; } 
        public string ProcessedPath { get; init; } 
        public string ErrorPath { get; init; } 
    }
}
