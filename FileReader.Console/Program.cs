// See https://aka.ms/new-console-template for more information
using FileReader.Core.Services;

Console.WriteLine("Hello, World!");

string path = "H:\\source\\net\\FileReader\\ExampleProcess";
Console.WriteLine("Ruta a procesar", path);

// crea la aplicacion, evita una instanciacion para poder inyectar dependencias del logger
var reader = FileReaderOrchestrator.Init();

var a = reader.start(path);

Console.WriteLine(a.ToString());
