
using FileReader.Core.Processing;
using Microsoft.Extensions.Configuration;

// leer la configuracion

var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


string path = config["Processing:Path"];

Console.WriteLine("Ruta a procesar", path);

// crea la aplicacion, evita una instanciacion para poder inyectar dependencias del logger
var reader = FileReaderOrchestrator.Init();

var a = reader.Execute(path, FileReader.Core.Models.FileTypes.xml);

Console.WriteLine(a.ToString());
