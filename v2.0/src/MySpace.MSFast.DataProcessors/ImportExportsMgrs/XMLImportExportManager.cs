using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.DataProcessors;
using System.IO;
using System.Xml;

namespace MySpace.MSFast.ImportExportsMgrs
{
    public class XMLImportExportManager : ImportExportManager
    {
        public String DefaultExtension { get { return "xml"; } }

        public ProcessedDataPackage  LoadProcessedDataPackage(Stream fileStream)
        {
            throw new NotImplementedException();
        }

        public bool SaveProcessedDataPackage(Stream filestream, ProcessedDataPackage package)
        {
            XmlDocument xml = null;

            if (package == null || (xml = package.Serialize()) == null)
            {
                return false;
            }

            xml.Save(filestream);
            
            return true;
        }

    }
}
