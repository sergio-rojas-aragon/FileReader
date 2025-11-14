using FileReader.Core.Base;
using FileReader.Core.DTO;
using FileReader.Core.Interfaces;
using FileReader.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace FileReader.Core.Services;

public class FileReaderOrchestrator
{
    private IFileReaderLogger<FileReaderOrchestrator> _logger;
    private FileProccesor _fileProcessor;
    private FolderResolver _folderResolver;
    private ProccesorService _readServ;

    public FileReaderOrchestrator(IFileReaderLogger<FileReaderOrchestrator> logger, FileProccesor fileProcessor, FolderResolver folderResolver, ProccesorService readServ) {

        _logger = logger;
        _fileProcessor = fileProcessor;
        _folderResolver = folderResolver;
        _readServ = readServ;
    }


    public string Execute(string path, FileTypes fileTypes) {

        //return "hola";
        _logger.LogInformation("Iniciando procesamiento en {path}", path);

        var result = _folderResolver.CreateAllFolderIfNoExists(path);
        if (result == false)
        {
            return $"Error al crear el directorio en {path}";
        }

        // read folder

        var listFiles = _fileProcessor.GetFiles(path, fileTypes);
        if (listFiles.Count == 0)
        {
            return $"Sin archivos para leer";
        }
        //Process Files

        foreach (var file in listFiles)
        {
            FileInfoDTO fileInfo = _fileProcessor.GetFileInfo(file);

            // i copy the original file so i dont have to move it
            string pathCopy = path + "\\" + fileInfo.fileName + " Copia" + fileInfo.FileExtension;
            File.Copy(file, pathCopy, true);
            fileInfo = _fileProcessor.GetFileInfo(pathCopy);

            // move and read file
            _fileProcessor.ProcessFile(fileInfo);

            // business proccesor

            var readResult = _readServ.Read(_fileProcessor.contentFile, _fileProcessor.processFileInfo, fileTypes);

            // move file if status proccess
            _fileProcessor.MoveFile(_fileProcessor.processFileInfo, readResult.estado);
     
        }

     

        _logger.LogInformation("Procesamiento finalizado");
        return "OK";
        
    }

    public static FileReaderOrchestrator Init() {

        // estatico para no crear una instancia


        // builder que permite configurar servicios antes de construir la aplicacion.
        var builder = Host.CreateApplicationBuilder();

       // limpia
        builder.Logging.ClearProviders();

        // agrega logger que escribe por consola.
        builder.Logging.AddConsole();

        // clases donde se hara la inyeccion de dependencias
        // AddSingleton<T>() significa que se creará una sola instancia de ese servicio durante toda la vida de la aplicación.

        builder.Services.AddSingleton<FileReaderOrchestrator>();
        builder.Services.AddSingleton<FolderResolver>();
        builder.Services.AddSingleton<FileProccesor>();
        builder.Services.AddSingleton<ProccesorService>();
        builder.Services.AddSingleton<IFolderPath, FolderPaths>();

        // variables compartidas ya que no se puede hacer instanciacion
        builder.Services.AddScoped<DirectoryPathsDTO>();

        // libreria para lectura de archivos
        builder.Services.AddSingleton<IFileSystem, FileSystem>();
        
        builder.Services.AddSingleton(typeof(IFileReaderLogger<>), typeof(LoggerAdapter<>));

        // crea la aplicacion a traves del builder, ensambla todo lo configurado
        var app = builder.Build();
        
        // devuelve la instancia
        return app.Services.GetRequiredService<FileReaderOrchestrator>();

    }
    
}
