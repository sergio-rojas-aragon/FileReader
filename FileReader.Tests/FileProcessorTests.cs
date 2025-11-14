using FileReader.Core.Base;
using FileReader.Core.DTO;
using FileReader.Core.Interfaces;
using FileReader.Core.Services;
using Moq;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileReader.Tests;

public class FileProcessorTests
{
    private Mock<IFileReaderLogger<FileProccesor>> loggerMock;
    private MockFileSystem fsMock;
    private string rootPath;
    private string ProcessedPath;
    private string ErrorsPath;

    public FileProcessorTests()
    {
        loggerMock = new Mock<IFileReaderLogger<FileProccesor>>();

        // Sistema de archivos en memoria
        fsMock = new MockFileSystem();
        rootPath = @"C:\TestRoot";

        //Pre cargo carpetas
        ProcessedPath = @"C:\TestRoot\Processed";
        ErrorsPath = @"C:\TestRoot\Errors";

        fsMock.AddDirectory(ProcessedPath);
        fsMock.AddDirectory(ErrorsPath);
        

    }

    [Fact]
    public void GetFiles_shouldReturnListXMLFiles()
    {

        // Arrange
        var dto = new DirectoryPathsDTO();
 

        fsMock.AddFile(@"c:\TestRoot\archivo.xml", new MockFileData("contenido del archivo"));
        fsMock.AddFile(@"c:\TestRoot\archivo2.xml", new MockFileData("contenido del archivo"));

        var sut = new FileProccesor(loggerMock.Object, fsMock, dto);

        var files = sut.GetFiles(rootPath, Core.Models.FileTypes.xml);

        //Asserts
        Assert.NotEmpty(files);
        Assert.Equal(2, files.Count());
        Assert.Contains("archivo.xml", files.First());

    }

    [Fact]
    public void GetFileInfo_ShouldReturnDetailsFromFile() {

        string rootPath = @"C:\TestRoot";
        fsMock.AddFile(@"c:\TestRoot\archivo.xml", new MockFileData("contenido del archivo"));
        var dto = new DirectoryPathsDTO();

        var sut = new FileProccesor(loggerMock.Object, fsMock, dto);

        var fileInfo = sut.GetFileInfo(@"c:\TestRoot\archivo.xml");

        //Asserts

        Assert.Equal("c:\\TestRoot\\archivo.xml", fileInfo.fileFullPath);
        Assert.Equal(".xml", fileInfo.FileExtension);


    }

    [Fact]
    public void GetFileInfo_ShouldReturnException() {

        string rootPath = @"C:\TestRoot";
        var dto = new DirectoryPathsDTO();

        var sut = new FileProccesor(loggerMock.Object, fsMock, dto);

        
        //var fileInfo = sut.GetFileInfo("");

        //Asserts
        var exception = Assert.Throws<ArgumentException>(() => sut.GetFileInfo(string.Empty));
        Assert.Contains("La ruta no puede ser vacia o nula", exception.Message);
    }

    [Fact]
    public void MoveFile_ShouldMoveFileToProcessedIfStatusTrue()
    {

        var dto = new DirectoryPathsDTO();
        dto.ProcessedPath = ProcessedPath;

        var sut = new FileProccesor(loggerMock.Object, fsMock, dto);

        fsMock.AddFile(@"c:\TestRoot\archivo.xml", new MockFileData("contenido del archivo"));
        var fileInfo = sut.GetFileInfo(@"c:\TestRoot\archivo.xml");
        bool status = true;

        sut.MoveFile(fileInfo, status);

        //Asserts
        Assert.True(fsMock.FileExists(ProcessedPath + @"\archivo.xml"));
        Assert.False(fsMock.FileExists(@"c:\TestRoot\archivo.xml"));
        Assert.False(fsMock.FileExists(ErrorsPath + @"\archivo.xml"));
    }

    [Fact]
    public void MoveFile_ShouldMoveFileToErrorsIfStatusFalse()
    {

        var dto = new DirectoryPathsDTO();
        dto.ProcessedPath = ProcessedPath;
        dto.ErrorPath = ErrorsPath;

        var sut = new FileProccesor(loggerMock.Object, fsMock, dto);

        fsMock.AddFile(@"c:\TestRoot\archivo.xml", new MockFileData("contenido del archivo"));
        var fileInfo = sut.GetFileInfo(@"c:\TestRoot\archivo.xml");
        bool status = false;

        sut.MoveFile(fileInfo, status);

        //Asserts
        Assert.False(fsMock.FileExists(ProcessedPath + @"\archivo.xml"));
        Assert.False(fsMock.FileExists(@"c:\TestRoot\archivo.xml"));
        Assert.True(fsMock.FileExists(ErrorsPath + @"\archivo.xml"));
    }
}

