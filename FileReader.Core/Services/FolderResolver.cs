using FileReader.Core.Base;
using FileReader.Core.Common;
using FileReader.Core.DTO;
using FileReader.Core.Interfaces;
using System.IO.Abstractions;

namespace FileReader.Core.Services
{
    public class FolderResolver : FolderResolverBase
    {
        private IFileReaderLogger<FolderResolver> _logger;
        private IFileSystem _fileSystem;
        private DirectoryPathsDTO _dirDTO;
        private List<(string NombrePath, string Path)>? _folders;

        public FolderResolver(IFileReaderLogger<FolderResolver> logger, 
            IFileSystem fileSystem, 
            IFolderPath folderPath,
            DirectoryPathsDTO dirDTO
            ) 
            : base(folderPath)
        {

            _logger = logger;
            _fileSystem = fileSystem;
            _dirDTO = dirDTO;

        }


        public override bool CreateAllFolderIfNoExists(string path) {

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

        protected override void SetDirectoryPaths(string path)
        {
            _logger.LogInformation("SetDirectoryPaths");

            _dirDTO.LogPath = path + "\\" + folderPath.LogPath;
            _dirDTO.ProcessPath = path + "\\" + folderPath.ProcessPath;
            _dirDTO.ProcessedPath = path + "\\" + folderPath.ProcessedPath;
            _dirDTO.ErrorPath = path + "\\" + folderPath.ErrorPath;


            _folders = new List<(string NombrePath, string Path)> {

                (nameof(folderPath.LogPath), _dirDTO.LogPath),
                (nameof(folderPath.ProcessPath), _dirDTO.ProcessPath),
                (nameof(folderPath.ProcessedPath), _dirDTO.ProcessedPath),
                (nameof(folderPath.ErrorPath), _dirDTO.ErrorPath)
            };

        }

        protected override void tryCreateDirectory(string path){

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
