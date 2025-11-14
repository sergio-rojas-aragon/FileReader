using FileReader.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Core.DTO
{
    public class DirectoryPathsDTO
    {
        public string LogPath { get; set; } = string.Empty;
        public string ProcessPath { get; set; } = string.Empty;
        public string ProcessedPath { get; set; } = string.Empty;
        public string ErrorPath { get; set; } = string.Empty;
    }
}
