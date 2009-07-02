//=======================================================================
/* Project: MSFast (MySpace.MSFast.GUI.Engine)
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
using System.Reflection;
using MySpace.MSFast.GUI.Engine.Panels;
using MySpace.MSFast.SuProxy.Proxy;
using MySpace.MSFast.Engine.DataCollector;
using MySpace.MSFast.Engine;

namespace MySpace.MSFast.GUI.Engine.Helpers
{
    public static class VersionManagement
    {
        public static String GetVersionsString()
        {
            return String.Format("Toolbar ({0})\r\nEngine ({1})\r\nSuProxy ({2})",
                    GetVersionString(typeof(VersionManagement)),
                    GetVersionString(typeof(SuProxyServer)),
                    GetVersionString(typeof(PageDataCollector)));
        }
        
        public static String GetVersionString(Type of)
        {
            Assembly assembly = Assembly.GetAssembly(of);
            if (assembly != null)
            {
                AssemblyName assemblyName = assembly.GetName();
                return String.Format("Version {0}", assemblyName.Version);
            }
            return null;
        }
    }
}
