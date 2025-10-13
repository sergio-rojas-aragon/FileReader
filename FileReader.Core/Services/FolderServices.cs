using FileReader.Core.Common;
using FileReader.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Core.Services
{
    public class FolderServices
    {
        private IFileReaderLogger<FolderServices> _logger;

        public string LogPath { get; internal set; } = "Logs";
        public string ProcessPath { get; internal set; } = "Process";
        public string ProcessedPath { get; internal set; } = "Processed";

        public string ErrorPath { get; internal set; } = "Errors";
        public string Path { get; internal set; } = "";

        public FolderServices(IFileReaderLogger<FolderServices> logger) {

            _logger = logger;

        }



        internal Result CreateAllFolderIfNoExists(string path) {

            SetDirectoryPaths(path);
            
            // validar si la ruta principal existe
            _logger.LogInformation("Inicio Creacion Folder");
            
            
            if (! Directory.Exists(Path))
            {
                _logger.LogError( null, "directorio no existe {Path}", Path);
                return new Result { estado = false, mensaje = "directorio no existe" };
                
            }


            // LOGPATH
            try
            {
                
                Directory.CreateDirectory(LogPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "directorio no existe {Path}", Path);
                return new Result { estado = false, mensaje = "directorio no existe" };
            }

            //PROCESSPATH
            try
            {
               
                Directory.CreateDirectory(ProcessPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "directorio no existe {ProcessPath}", ProcessPath);
                return new Result { estado = false, mensaje = "directorio no existe" };
            }


            //PROCESSEDPATH
            try
            {
                
                Directory.CreateDirectory(ProcessedPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "directorio no existe {ProcessedPath}", ProcessedPath);
                return new Result { estado = false, mensaje = "directorio no existe" };
            }

            //ERRORPATH
            try
            {

                Directory.CreateDirectory(ErrorPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "directorio no existe {ErrorPath}", ErrorPath);
                return new Result { estado = false, mensaje = "directorio no existe" };
            }

            return new Result { estado = true, mensaje = "" };
        
        }

        private void SetDirectoryPaths(string path)
        {
            _logger.LogInformation("SetDirectoryPaths");
            this.Path = path;
            LogPath = Path + "\\" + LogPath;
            ProcessPath = Path + "\\" + ProcessPath;
            ProcessedPath = Path + "\\" + ProcessedPath;
            ErrorPath = Path + "\\" + ErrorPath;


        }
    }
}
