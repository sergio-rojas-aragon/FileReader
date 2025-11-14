using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Core.Base
{
    public class CreateFolderException : Exception
    {
        public CreateFolderException(string mensaje, Exception inner = null) : base(mensaje, inner)
        {
        
        }
    }
}
