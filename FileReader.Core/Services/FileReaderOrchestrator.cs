using FileReader.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileReader.Core.Services;

public class FileReaderOrchestrator
{
    private IFileReaderLogger<FileReaderOrchestrator> _logger;
    private FolderServices _folderService;
    private ProcessFileServices _processFileServ;

    public FileReaderOrchestrator(IFileReaderLogger<FileReaderOrchestrator> logger, FolderServices folderService, ProcessFileServices processFileServ ) {

        _logger = logger;
        _folderService = folderService;
        _processFileServ = processFileServ;

    }


    public string start(string path) {

        //return "hola";
        _logger.LogInformation("Iniciando procesamiento en {path}", path);
        var result = _folderService.CreateAllFolderIfNoExists(path);
        if (result.estado == false)
        {
            return "error al leer archivo por lo siguiente: " + result.mensaje;
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
        builder.Services.AddSingleton<FolderServices>();
        builder.Services.AddSingleton<ProcessFileServices>();
        builder.Services.AddSingleton<ReadService>();
        
        builder.Services.AddSingleton(typeof(IFileReaderLogger<>), typeof(LoggerAdapter<>));

        // crea la aplicacion a traves del builder, ensambla todo lo configurado
        var app = builder.Build();
        
        // devuelve la instancia
        return app.Services.GetRequiredService<FileReaderOrchestrator>();

    }
    
}
