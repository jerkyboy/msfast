using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.Core.Configuration.ConfigProviders;
using MySpace.MSFast.Core.Configuration.CommonDataTypes;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{   
    [JSONObject("browser")]
    public class Browser
    {
        [JSONField("version")]
        public String Version;

        [JSONField("name")]
        public String Name;

        public Browser(ProcessedDataPackage pacakge)
        {
            IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");

            if (getter != null && String.IsNullOrEmpty(getter.GetString(MSFastGlobalConfigKeys.CURRENT_PACKAGE_VERSION)))
            {
                Name = "MSFast Test Harness";
                Version = getter.GetString(MSFastGlobalConfigKeys.CURRENT_PACKAGE_VERSION);
            }
            else
            {
                Name = "MSFast Test Harness";
                Version = "0.0";
            }
        }
    }
}
