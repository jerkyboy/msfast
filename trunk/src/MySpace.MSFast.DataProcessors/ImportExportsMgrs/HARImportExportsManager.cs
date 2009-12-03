using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MySpace.MSFast.DataProcessors;
using System.IO.Compression;
using System.Text.RegularExpressions;
using MySpace.MSFast.Core.Configuration.Common;
using MySpace.MSFast.ImportExportsMgrs.HARObjects;
using MySpace.MSFast.Core.Configuration.ConfigProviders;
using MySpace.MSFast.Core.Configuration.CommonDataTypes;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors.Render;
using MySpace.MSFast.Core.Utils;

namespace MySpace.MSFast.ImportExportsMgrs
{
    public class HARImportExportsManager : ImportExportManager
    {
        public String DefaultExtension { get { return "har"; } }

        public ProcessedDataPackage LoadProcessedDataPackage(Stream fileStream)
        {
            throw new NotImplementedException();
        }
        
        public bool SaveProcessedDataPackage(Stream msfFilestream, ProcessedDataPackage pacakge)
        {
            byte[] f = Encoding.UTF8.GetBytes(SimpleJSONSerializer.Serialize(new Trace(pacakge)));
            msfFilestream.Write(f, 0, f.Length);
            return true;
        }  
    }
}
