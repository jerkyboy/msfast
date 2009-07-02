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
using MySpace.MSFast.Core.Configuration.ConfigProviders.MSFast;

namespace MySpace.MSFast.Core.Configuration.ConfigProviders
{
	public class ConfigProvider
	{
		private Dictionary<String, IConfigSetter> setters = null;
		private Dictionary<String, IConfigGetter> getters = null;

		public static readonly ConfigProvider Instance = new ConfigProvider();

		private ConfigProvider()
		{
			this.setters = new Dictionary<string, IConfigSetter>();
			this.getters = new Dictionary<string, IConfigGetter>();

			MSFastGlobalConfigGetterSetter msFastGlobalConfigGetterSetter = new MSFastGlobalConfigGetterSetter();

			this.setters.Add("MSFast.Global", msFastGlobalConfigGetterSetter);
			this.getters.Add("MSFast.Global", msFastGlobalConfigGetterSetter);

		}


		public IConfigGetter GetConfigGetter(String categoryId)
		{
			if (this.getters.ContainsKey(categoryId))
			{
				return this.getters[categoryId];
			}
			return null;
		}
		public IConfigSetter GetConfigSetter(String categoryId)
		{
			if (this.setters.ContainsKey(categoryId))
			{
				return this.setters[categoryId];
			}
			return null;
		}



	}
}
