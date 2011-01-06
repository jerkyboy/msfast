//=======================================================================
/* Project: MSFast (MySpace.MSFast.Engine)
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
using System.IO;
using MySpace.MSFast.Engine.DataCollector;

namespace MySpace.MSFast.Engine.CollectorStartInfo
{
	public class BufferedPageDataCollectorStartInfo : PageDataCollectorStartInfo
	{
		public String Buffer;

		private String generatedTempFilename = null;

		public override bool IsValid()
		{
			return (base.IsValid() && String.IsNullOrEmpty(this.Buffer) == false);
		}

		public override void Dispose()
		{
			base.Dispose();
			this.Buffer = null;
		}

		public override PageDataCollectorErrors PrepareStartInfo()
		{

			generatedTempFilename = base.TempFolder;			
			generatedTempFilename = generatedTempFilename.Replace("\\", "/");

			if (generatedTempFilename.EndsWith("/") == false)
				generatedTempFilename += "/";

			generatedTempFilename += Guid.NewGuid().ToString().Replace("-", "") + ".html";


			if (SaveBuffer(this.Buffer, generatedTempFilename) == false)
				return PageDataCollectorErrors.CantSaveTempFile;



            String lw = String.Concat(this.URL, ((URL.IndexOf("?") == -1) ? "?" : "&"), "__SUPROXY_LOCAL_RESPONSE=", generatedTempFilename);
            lw = AppendCollectionArgs(lw);
            base.TestURL = lw;

			return base.PrepareStartInfo();
		}

		public override PageDataCollectorErrors CleanUp()
		{
			if (String.IsNullOrEmpty(this.generatedTempFilename) == false && File.Exists(this.generatedTempFilename))
			{
				File.Delete(this.generatedTempFilename);
			}
			return base.CleanUp();
		}

		private bool SaveBuffer(string buffer, string filename)
		{
			// Save temp html
			try
			{
                File.WriteAllText(filename, buffer);
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}
