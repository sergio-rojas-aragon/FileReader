using FileReader.Core.Interfaces;
using FileReader.Core.Services;
using Moq;

namespace FileReader.Tests;

public class UnitTest1
{
    [Fact]
    public void Start_ShouldReturnOk_WhenEverythingWorks()
    {
        // 1️⃣ Crear mocks (simulaciones)
        var mockLogger = new Mock<IFileReaderLogger<FileReaderOrchestrator>>();
        var mockFolderService = new Mock<FolderServices>();
        var mockProcessFileService = new Mock<ProcessFileServices>();

        // 2️⃣ Decirle a los mocks cómo comportarse
        mockFolderService
            .Setup(x => x.CreateAllFolderIfNoExists(It.IsAny<string>()))
            .Returns((estado: true, mensaje: "ok"));

        mockProcessFileService
            .Setup(x => x.ReadAndProcessFolder(It.IsAny<string>()))
            .Returns("Listo");

        // 3️⃣ Crear la clase con los mocks
        var orchestrator = new FileReaderOrchestrator(
            mockLogger.Object,
            mockFolderService.Object,
            mockProcessFileService.Object
        );

        // 4️⃣ Llamar al método
        var result = orchestrator.start("C:\\RutaDePrueba");

        // 5️⃣ Verificar que el resultado sea el esperado
        Assert.Equal("OK", result);
    }
}
