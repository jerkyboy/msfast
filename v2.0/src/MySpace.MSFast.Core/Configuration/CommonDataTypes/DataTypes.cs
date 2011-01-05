//=======================================================================
/* Project: MSFast (MySpace.MSFast.Core)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
using System;
using System.Collections.Generic;
using System.Text;

namespace MySpace.MSFast.Core.Configuration.CommonDataTypes
{


	public static class MSFastGlobalConfigKeys
	{
        public const String PAGE_VALIDATION = "PageValidation";
        public const String PAGE_GRAPH = "PageGraph";

        public const String CLEAR_CACHE_BEFORE_TEST = "ClearCacheBeforeTest";

		public const String LAST_COLLECTION_ID = "LatestCollection";

		public const String TEMP_FOLDER = "TemporaryFolder";
		public const String DUMP_FOLDER = "DumpFolder";
        public const String DUMP_MAX_SIZE = "DumpSize";

		public const String DEFAULT_PROXY_PORT = "DefaultProxyPort";

		public const String DEVICE_ID = "DeviceId";
		public const String PORTS = "Ports";

        public const String VERSION_UPDATE_URL = "VersionUpdateURL";
        public const String CURRENT_PACKAGE_VERSION = "CurrentPackageVersion";
        public const String CURRENT_PACKAGE_VERSION_LATEST_ALERT = "LatestPackageVersion";

	}
}
