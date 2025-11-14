using FileReader.Core.DTO;
using FileReader.Core.Interfaces;
using FileReader.Core.Models;
using System.IO.Abstractions;


namespace FileReader.Core.Services
{
    public class FileProccesor
    {
        private IFileSystem _fileSystem;
        private IFileReaderLogger<FileProccesor> _logger;
        private DirectoryPathsDTO _dirDTO;
        private string _path;

        public FileInfoDTO processFileInfo { get; private set; }
        public string contentFile { get; private set; }

        public FileProccesor(
                IFileReaderLogger<FileProccesor> logger, 
                IFileSystem fileSystem, 
                DirectoryPathsDTO dirDTO
            )
        {
            _fileSystem = fileSystem;
            _logger = logger;
            _dirDTO = dirDTO;
        }


        public List<string> GetFiles(string path, FileTypes fileTypes)
        {

            _path = path;
            var listaArchivos = new List<string>();
            try
            {

                // lo optimizo
                string[] pathFiles = _fileSystem.Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
                _logger.LogInformation("Se encontraron " + pathFiles.Length + " archivos");

                

                foreach (string archivo in pathFiles)
                {
                    //ProcessFile(archivo);
                    listaArchivos.Add(archivo);

                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException("error al leer la carpeta", ex);
            }


            return listaArchivos;
        }

        public void ProcessFile(FileInfoDTO fileInfoDTO)
        {
            // move to process
            processFileInfo = MoveFile(fileInfoDTO, _dirDTO.ProcessPath);

            // read file
            contentFile = ReadFile(processFileInfo);
            
        }

        private string ReadFile(FileInfoDTO fileInfo)
        {
            string text = string.Empty;
            using (StreamReader reader = new(fileInfo.fileFullPath))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }

        public FileInfoDTO GetFileInfo(string PathWithFile)
        {
            if (string.IsNullOrEmpty(PathWithFile))
            {
                throw new ArgumentException("La ruta no puede ser vacia o nula");
            }

            try
            {
                FileInfoDTO file = new FileInfoDTO();
                file.fileName = Path.GetFileNameWithoutExtension(PathWithFile);
                file.FileExtension = Path.GetExtension(PathWithFile);
                file.fileDirectory = Path.GetDirectoryName(PathWithFile);
                file.fileFullPath = PathWithFile;
                return file;
            }
            catch (Exception ex)
            {

                throw;

            }


            
        }

        private FileInfoDTO MoveFile(FileInfoDTO fileInfo, string PathDestination)
        {

            string fileDestination = PathDestination + "\\" + fileInfo.fileName + fileInfo.FileExtension;
            _fileSystem.File.Move(fileInfo.fileFullPath, fileDestination);
            fileInfo = GetFileInfo(fileDestination);
            
      
            return fileInfo;
        }

        public void MoveFile(FileInfoDTO fileInfo, bool status) {

            if (status)
            {
                MoveFile(fileInfo, _dirDTO.ProcessedPath);
            }
            else
            {
                fileInfo = MoveFile(fileInfo, _dirDTO.ErrorPath);
            }

        }
    }
}