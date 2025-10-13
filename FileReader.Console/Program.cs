
using FileReader.Core.Services;
using Microsoft.Extensions.Configuration;

// leer la configuracion

var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


string path = config["Processing:Path"];

Console.WriteLine("Ruta a procesar", path);

// crea la aplicacion, evita una instanciacion para poder inyectar dependencias del logger
var reader = FileReaderOrchestrator.Init();

var a = reader.start(path);

Console.WriteLine(a.ToString());
