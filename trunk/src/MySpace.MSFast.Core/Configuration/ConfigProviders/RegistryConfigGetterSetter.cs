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
using Microsoft.Win32;

namespace MySpace.MSFast.Core.Configuration.ConfigProviders
{
	public abstract class RegistryConfigGetterSetter:IConfigGetter,IConfigSetter
	{
		String baseKey = null;
		public RegistryConfigGetterSetter(String baseKey) 
		{
			this.baseKey = baseKey;
		}

		#region IConfigSetter Members

		public void SetString(string key, string value)
		{
			if(baseKey != null){
				try
				{
                    RegistryKey impl = Registry.LocalMachine.CreateSubKey(baseKey);
					impl.SetValue(key, value, RegistryValueKind.String);
				}
				catch 
				{
				}
			}
		}

		public void SetInt(string key, int value)
		{
			if (baseKey != null)
			{
				try
				{
                    RegistryKey impl = Registry.LocalMachine.CreateSubKey(baseKey);
					impl.SetValue(key, value, RegistryValueKind.DWord);
				}
				catch
				{
				}
			}
		}

		public void SetBoolean(string key, bool value)
		{
			if (baseKey != null)
			{
				try
				{
                    RegistryKey impl = Registry.LocalMachine.CreateSubKey(baseKey);
					impl.SetValue(key, (value ? 1 : 0) , RegistryValueKind.DWord);
				}
				catch
				{
				}
			}
		}

		public void SetObject(string key, object value)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IConfigGetter Members

		public string GetString(string key)
		{
			if (baseKey != null)
			{
				try
				{
                    RegistryKey impl = Registry.LocalMachine.OpenSubKey(baseKey, false);
                    String s = (String)impl.GetValue(key);
                    impl.Close();
                    return s;
				}
				catch
				{
				}
			}
			return null;
		}

		public int GetInt(string key)
		{
			if (baseKey != null)
			{
				try
				{
                    RegistryKey impl = Registry.LocalMachine.OpenSubKey(baseKey, false);
					int s = (int)impl.GetValue(key);
                    impl.Close();
                    return s;
                }
				catch
				{
				}
			}
			return -1;
		}

		public bool GetBoolean(string key)
		{
			if (baseKey != null)
			{
				try
				{
                    RegistryKey impl = Registry.LocalMachine.CreateSubKey(baseKey);
					bool s = (1 == (int)impl.GetValue(key));
                    impl.Close();
                    return s;
                }
				catch
				{
				}
			}
			return false;
		}

		public object GetObject(string key)
		{
            return null;
		}

		#endregion
	}
}
