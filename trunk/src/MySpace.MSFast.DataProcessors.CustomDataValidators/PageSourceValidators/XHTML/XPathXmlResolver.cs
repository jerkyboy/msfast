//=======================================================================
/* Project: MSFast (MySpace.MSFast.DataProcessors.CustomDataValidators)
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
using System.Xml;
using System.IO;
using System.Net;
using Assembly = System.Reflection.Assembly;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.PageSourceValidators.XHTML
{
	class XHTMLXmlResolver : XmlResolver
	{
		static readonly Dictionary<string, string> FileNameToType;
		static readonly Assembly MyAss;
		static readonly Type MyType;

		static XHTMLXmlResolver()
		{
			MyType = typeof(XHTMLXmlResolver);
			MyAss = MyType.Assembly;

			Dictionary<string, string> fileNameToManifestResource = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			foreach (string ManifestResourceName in MyAss.GetManifestResourceNames())
			{
				string FileName = ManifestResourceName.Substring(MyType.Namespace.Length + 1);
				fileNameToManifestResource.Add(FileName, ManifestResourceName);
			}

			FileNameToType = fileNameToManifestResource;
		}

		static Stream GetStream(string FileName)
		{
			string StreamName = null;

			if (!FileNameToType.TryGetValue(FileName, out StreamName))
				return null;

			return MyAss.GetManifestResourceStream(StreamName);
		}

		public override ICredentials Credentials
		{
			set { }
		}

		const string DECLARE_TRANSITIONAL = "W3C/DTD XHTML 1.0 Transitional";
		const string FOLDER_TRANSITIONAL = "transitional.";
		const string DTD_TRANSITIONAL = FOLDER_TRANSITIONAL + "xhtml1-transitional.dtd";


		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			Stream iostr = null;

			string URL = absoluteUri.ToString();

			int index = URL.IndexOf(DECLARE_TRANSITIONAL);

			if (index == -1)
				return null;

			index += DECLARE_TRANSITIONAL.Length + 1;

			string FileName = URL.Substring(index);
			string ResourceName = null;

			if (string.Equals("EN", FileName, StringComparison.OrdinalIgnoreCase))
			{
				ResourceName = DTD_TRANSITIONAL;
			}
			else
			{
				ResourceName = FOLDER_TRANSITIONAL + FileName;
			}

			iostr = GetStream(ResourceName);

			return iostr;
		}
	}
}
