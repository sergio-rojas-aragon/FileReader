using FileReader.Core.DTO;
using FileReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Core.Interfaces
{
    public interface IFileProcessor
    {
        List<string>? GetFilesFromFolder(string pathFolder, FileTypes fileTypes);
        void MoveFile(FileInfoDTO fileInfoDTO, string PathDestination);

    }
}
