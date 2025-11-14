using FileReader.Core.Base;
using FileReader.Core.Common;
using FileReader.Core.Interfaces;
using System.IO.Abstractions;

namespace FileReader.Core.IO
{
    public class Folders : FolderPaths
    {
        private IFileReaderLogger<Folders> _logger;
        private IFileSystem _fileSystem;
        private List<(string NombrePath, string Path)> _folders;

        public Folders(IFileReaderLogger<Folders> logger, IFileSystem fileSystem) {

            _logger = logger;
            _fileSystem = fileSystem;

        }

        public bool CreateAllFolderIfNoExists(string path) {

            SetDirectoryPaths(path);
            
            // validar si la ruta principal existe
            _logger.LogInformation("Inicio Creacion Folder");
            
            
            if (! _fileSystem.Directory.Exists(path))
            {
                _logger.LogError( null, "directorio no existe {Path}", path);
                return false;
                
            }


            foreach (var folder in _folders)
            {
                tryCreateDirectory(folder.Path);
            };

            return true;        
        }

        private void SetDirectoryPaths(string path)
        {
            _logger.LogInformation("SetDirectoryPaths");
            

            _folders = new List<(string NombrePath, string Path)> {

                (nameof(LogPath), path + "\\" + LogPath),
                (nameof(ProcessPath), path + "\\" + ProcessPath),
                (nameof(ProcessedPath), path + "\\" + ProcessedPath),
                (nameof(ErrorPath), path + "\\" + ErrorPath)
            };


        }

        private void tryCreateDirectory(string path){

            try
            {
                if (!_fileSystem.Directory.Exists(path)) {
                    _fileSystem.Directory.CreateDirectory(path);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la sub carpeta {path}", path);
                throw new CreateFolderException($"Error al crear la sub carpeta {path}", ex);
            }

        }
    }
}
