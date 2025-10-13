using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Core.DTO
{
    public class FileInfoDTO
    {

        public string fileName { get; set; }
        public string fileDirectory { get; set; }
        public string FileExtension { get; set; }
        public string fileFullPath { get; set; }


    }
}
