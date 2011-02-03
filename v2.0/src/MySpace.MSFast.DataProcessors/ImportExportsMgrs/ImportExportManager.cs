using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.DataProcessors;
using System.IO;

namespace MySpace.MSFast.ImportExportsMgrs
{
    public interface ImportExportManager
    {
        String DefaultExtension{get;}
        
        ProcessedDataPackage LoadProcessedDataPackage(Stream fileStream);
        bool SaveProcessedDataPackage(Stream filestream, ProcessedDataPackage pacakge);        
    }
}
