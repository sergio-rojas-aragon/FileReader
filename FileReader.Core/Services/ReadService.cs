using FileReader.Core.Common;
using FileReader.Core.DTO;
using FileReader.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileReader.Core.Services
{
    public class ReadService
    {
        private IFileReaderLogger<ReadService> _logger;

        public ReadService(IFileReaderLogger<ReadService> logger) { 
        
            _logger = logger;
        }


        public Result LeeXML(FileInfoDTO fileInfo)
        {

            var xmlDoc = new XmlDocument();


            try
            {
                xmlDoc.Load(fileInfo.fileFullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al leer XML {fileInfo.fileName}", fileInfo.fileName);
                return new Result { estado = false, mensaje = "error al leer XML" };
            }

            // solo validare algunos datos para el ejercicio
            foreach (XmlNode node in xmlDoc.SelectNodes("/DTE"))
            {
                var DTE = new XmlDocument();
                DTE.LoadXml(node.OuterXml);
                string folio = "";
                string codsii = "";
                string rut = "";

                try
                {
                    folio = DTE.SelectSingleNode("/DTE/Documento/Encabezado/IdDoc/Folio").InnerText;
                    codsii = DTE.SelectSingleNode("/DTE/Documento/Encabezado/IdDoc/TipoDTE").InnerText;
                    rut = DTE.SelectSingleNode("/DTE/Documento/Encabezado/Emisor/RUTEmisor").InnerText;
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "error al leer data {fileInfo.fileName}", fileInfo.fileName);
                    return new Result { estado = false, mensaje = "error al leer data" };
                }

                Console.WriteLine(rut + "-" + codsii + "-" + rut);

            }



            return new Result {  estado = true, mensaje = "" };

        }
    }
}
