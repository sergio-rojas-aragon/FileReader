using FileReader.Core.Base;
using FileReader.Core.DTO;
using FileReader.Core.Interfaces;
using FileReader.Core.Services;
using Moq;
using System.IO.Abstractions.TestingHelpers;


namespace FileReader.Tests
{
    public class FolderResolverTests
    {
        private FolderPaths folderPath;
        private DirectoryPathsDTO dto;

        public FolderResolverTests()
        {
            folderPath = new FolderPaths();
            dto = new DirectoryPathsDTO();
        }
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


            var loggerMock = new Mock<IFileReaderLogger<FolderResolver>>();
            var fsMock = new MockFileSystem(); // Sistema de archivos en memoria


            var sut = new FolderResolver(loggerMock.Object, fsMock, folderPath, dto);

            string rootPath = @"C:\TestRoot"; // NO lo agregamos al mock, así que "no existe"

            var result = sut.CreateAllFolderIfNoExists(rootPath);
            // Assert
            Assert.False(result);

        }

        [Fact]

        public void CreateAllFolderIfNoExists_CreateAllSubFolders_WhenPathExist() {

            var loggerMock = new Mock<IFileReaderLogger<FolderResolver>>();
            var fsMock = new MockFileSystem();

            string root = @"C:\Base";
            fsMock.AddDirectory(root);

            var sut = new FolderResolver(loggerMock.Object, fsMock, folderPath, dto);

            //Act
            var result = sut.CreateAllFolderIfNoExists(root);

            //Asserts
            Assert.True(result);
            Assert.True(fsMock.Directory.Exists(root + "\\" + sut.folderPath.LogPath));
            Assert.True(fsMock.Directory.Exists(root + "\\" + sut.folderPath.ProcessPath));
            Assert.True(fsMock.Directory.Exists(root + "\\" + sut.folderPath.ProcessedPath));
            Assert.True(fsMock.Directory.Exists(root + "\\" + sut.folderPath.ErrorPath));

        }
    }
}
