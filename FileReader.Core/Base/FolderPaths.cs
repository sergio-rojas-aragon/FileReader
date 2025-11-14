
using FileReader.Core.Interfaces;

namespace FileReader.Core.Base
{
    public class FolderPaths : IFolderPath
    {
        public string LogPath { get; init; } = "Logs";
        public string ProcessPath { get; init; } = "Process";
        public string ProcessedPath { get; init; } = "Processed";
        public string ErrorPath { get; init; } = "Errors";

    }
}
