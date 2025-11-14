using FileReader.Core.Interfaces;
using FileReader.Core.IO;
using FileReader.Core.Models;
using FileReader.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace FileReader.Core.Processing;

public class FileReaderOrchestrator
{
    private IFileReaderLogger<FileReaderOrchestrator> _logger;
    private Folders _folderService;
    private ProcessFileServices _processFileServ;

    public FileReaderOrchestrator(IFileReaderLogger<FileReaderOrchestrator> logger, Folders folderService, ProcessFileServices processFileServ ) {

        _logger = logger;
        _folderService = folderService;
        _processFileServ = processFileServ;

    }


    public string Execute(string path, FileTypes fileTypes) {

        //return "hola";
        _logger.LogInformation("Iniciando procesamiento en {path}", path);
        var result = _folderService.CreateAllFolderIfNoExists(path);
        if (result == false)
        {
            return $"Error al crear el directorio en {path}";
        }

        var a = _processFileServ.ReadAndProcessFolder(path);

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
        builder.Services.AddSingleton<Folders>();
        builder.Services.AddSingleton<ProcessFileServices>();
        builder.Services.AddSingleton<ReadService>();

        // libreria para lectura de archivos
        builder.Services.AddSingleton<IFileSystem, FileSystem>();
        
        builder.Services.AddSingleton(typeof(IFileReaderLogger<>), typeof(LoggerAdapter<>));

        // crea la aplicacion a traves del builder, ensambla todo lo configurado
        var app = builder.Build();
        
        // devuelve la instancia
        return app.Services.GetRequiredService<FileReaderOrchestrator>();

    }
    
}
