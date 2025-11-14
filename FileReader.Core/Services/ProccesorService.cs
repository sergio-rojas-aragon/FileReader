using FileReader.Core.DTO;
using FileReader.Core.Interfaces;
using FileReader.Core.Models;
using System.Xml;

namespace FileReader.Core.Services
{
    public class ProccesorService: IProccesorService
    {
        private IFileReaderLogger<ProccesorService> _logger;

        public ProccesorService(IFileReaderLogger<ProccesorService> logger) { 
        
            _logger = logger;
        }


        public Result Read(string contentFile, FileInfoDTO processFile, FileTypes fileTypes)
        {

            //Parte del XML
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(processFile.fileFullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al leer XML {fileInfo.fileName}", processFile.fileName);
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

                    _logger.LogError(ex, "error al leer data {fileInfo.fileName}", processFile.fileName);
                    return new Result { estado = false, mensaje = "error al leer data" };
                }

                Console.WriteLine(rut + "-" + codsii + "-" + rut);

            }

            return new Result {  estado = true, mensaje = "" };

        }


    }
}
