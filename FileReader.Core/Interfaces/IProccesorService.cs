using FileReader.Core.DTO;
using FileReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Core.Interfaces
{
    public interface IProccesorService
    {
        public Result Read(string contentFile, FileInfoDTO processFile, FileTypes fileTypes);
    }
}
