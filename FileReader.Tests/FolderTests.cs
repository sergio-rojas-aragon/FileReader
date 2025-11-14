using FileReader.Core.Interfaces;
using FileReader.Core.IO;
using Moq;
using System.IO.Abstractions.TestingHelpers;


namespace FileReader.Tests
{
    public class FolderTests
    {

        //private readonly ITestOutputHelper _output;

        //public FolderTests(ITestOutputHelper output)
        //{
        //    _output = output;
        //}
        /// <summary>
        /// Se envia ruta falsa para probocar error
        /// </summary>
        [Fact]
        public void CreateAllFolderIfNoExists_ReturnsError_WhenPathDoesNotExist() {


            var loggerMock = new Mock<IFileReaderLogger<Folders>>();
            var fsMock = new MockFileSystem(); // Sistema de archivos en memoria
            var sut = new Folders(loggerMock.Object, fsMock);

            string rootPath = @"C:\TestRoot"; // NO lo agregamos al mock, así que "no existe"

            var result = sut.CreateAllFolderIfNoExists(rootPath);
            // Assert
            Assert.False(result);

        }

        [Fact]

        public void CreateAllFolderIfNoExists_CreateAllSubFolders_WhenPathExist() {

            var loggerMock = new Mock<IFileReaderLogger<Folders>>();
            var fsMock = new MockFileSystem();

            string root = @"C:\Base";
            fsMock.AddDirectory(root);

            var sut = new Folders(loggerMock.Object, fsMock);

            //Act
            var result = sut.CreateAllFolderIfNoExists(root);

            //Asserts
            Assert.True(result);
            Assert.True(fsMock.Directory.Exists(root + "\\" + sut.LogPath));
            Assert.True(fsMock.Directory.Exists(root + "\\" + sut.ProcessPath));
            Assert.True(fsMock.Directory.Exists(root + "\\" + sut.ProcessedPath));
            Assert.True(fsMock.Directory.Exists(root + "\\" + sut.ErrorPath));

        }
    }
}
