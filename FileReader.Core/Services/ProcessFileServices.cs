using FileReader.Core.DTO;
using FileReader.Core.Interfaces;
using FileReader.Core.IO;


namespace FileReader.Core.Services
{
    public class ProcessFileServices
    {
        private IFileReaderLogger<ProcessFileServices> _logger;
        private Folders _folderServ;
        private ReadService _readServ;
        private string _path;

        public ProcessFileServices(IFileReaderLogger<ProcessFileServices> logger, Folders folderService, ReadService readServ)
        {
            _logger = logger;
            _folderServ = folderService;
            _readServ = readServ;
        }


        public bool ReadAndProcessFolder(string path)
        {

            _path = path;
            try
            {
                // en vb.net es asi For Each Archivo As String In My.Computer.FileSystem.GetFiles(path, FileIO.SearchOption.SearchTopLevelOnly, "*.xml")

                // lo optimizo
                string[] pathFiles = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
                _logger.LogInformation("Se encontraron " + pathFiles.Length + " archivos");

                foreach (string archivo in pathFiles)
                {
                    ProcessFile(archivo);

                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "error en leer carpeta", path);
                return false;
            }


            return true;
        }

        private void ProcessFile(string archivo)
        {


            // hare una excepcion y copiare el archivo primero para mantener intacto los originales.
            FileInfoDTO fileInfo = GetFileInfo(archivo);

            string pathCopy = _path + "\\" + fileInfo.fileName + " Copia" + fileInfo.FileExtension;
            File.Copy(archivo, pathCopy, true);

            fileInfo = GetFileInfo(pathCopy);

            // muevo el archivo a proceso
            try
            {
                fileInfo = MoveFile(fileInfo, _folderServ.ProcessPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pudo mover archivo " + fileInfo.fileName, _folderServ.ProcessPath);

                // termino el proceso
                throw;
            }

            // leer el archivo
            var resultado = _readServ.LeeXML(fileInfo);

            if (resultado.estado)
            {
                // si esta ok, lo mueve a procesado.
                fileInfo = MoveFile(fileInfo, _folderServ.ProcessedPath);
            }
            else {
                // si esta con error lo mueve a la carpeta de error
                fileInfo = MoveFile(fileInfo, _folderServ.ErrorPath);
            }


        }

        private FileInfoDTO GetFileInfo(string archivo)
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
    }
}