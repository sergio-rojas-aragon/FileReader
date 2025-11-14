using FileReader.Core.DTO;
using FileReader.Core.Interfaces;
using FileReader.Core.Models;


namespace FileReader.Core.Services
{
    public class FileProccesor
    {
        private IFileReaderLogger<FileProccesor> _logger;
        private FolderResolver _folderServ;
        private ProccesorService _readServ;
        private DirectoryPathsDTO _dirDTO;
        private string _path;

        public FileInfoDTO processFileInfo { get; private set; }
        public string contentFile { get; private set; }

        public FileProccesor(
                IFileReaderLogger<FileProccesor> logger, 
                FolderResolver folderService, 
                ProccesorService readServ,
                DirectoryPathsDTO dirDTO
            )
        {
            _logger = logger;
            _folderServ = folderService;
            _readServ = readServ;
            _dirDTO = dirDTO;
        }


        public List<string> GetFiles(string path, FileTypes fileTypes)
        {

            _path = path;
            var listaArchivos = new List<string>();
            try
            {

                // lo optimizo
                string[] pathFiles = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
                _logger.LogInformation("Se encontraron " + pathFiles.Length + " archivos");

                

                foreach (string archivo in pathFiles)
                {
                    //ProcessFile(archivo);
                    listaArchivos.Add(archivo);

                }
            }
            catch (Exception ex)
            {

                //_logger.LogError(ex, "error en leer carpeta", path);
                //return false;
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

        public FileInfoDTO GetFileInfo(string archivo)
        {
            FileInfoDTO file = new FileInfoDTO();
            file.fileName = Path.GetFileNameWithoutExtension(archivo);
            file.FileExtension = Path.GetExtension(archivo);
            file.fileDirectory = Path.GetDirectoryName(archivo);
            file.fileFullPath = archivo;

            return file;
        }

        private FileInfoDTO MoveFile(FileInfoDTO fileInfo, string PathDestination)
        {

           
            File.Move(fileInfo.fileFullPath, PathDestination + "\\" + fileInfo.fileName + fileInfo.FileExtension);
            fileInfo = GetFileInfo(PathDestination + "\\" + fileInfo.fileName + fileInfo.FileExtension);
            
      
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