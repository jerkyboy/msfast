//=======================================================================
/* Project: MSFast (MySpace.MSFast.GUI.Engine)
*  Original author: Omar Al Zabir
*  Modified by Yadid Ramot (e.yadid@gmail.com)
*
*  This class is a part of "SafeCOMWrapper" (CPOL). By Omar Al Zabir. 
*  For more info: http://www.codeproject.com/KB/COM/safecomwrapper.aspx?msg=1418263
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

namespace MySpace.MSFast.GUI.Engine.Panels.GraphView.DelayCOM
{
	/// <summary>
	/// An attribute to specifiy the ProgID of the
	/// COM class to create. (As suggested by
	/// Kristen Wegner)
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface, Inherited=false, AllowMultiple=false)]
	public sealed class ComProgIdAttribute : Attribute
	{
		private string _value;
		
		/// <summary>
		/// Extracts the attribute from the specified type.
		/// </summary>
		/// <param name="interfaceType">
		/// The interface type.
		/// </param>
		/// <returns>
		/// The <see cref="ComProgIdAttribute"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="interfaceType"/> is <see langword="null"/>.
		/// </exception>
		public static ComProgIdAttribute GetAttribute(Type interfaceType)
		{
			if (null == interfaceType) throw new ArgumentNullException("interfaceType");
			
			Type attributeType = typeof(ComProgIdAttribute);
			object[] attributes = interfaceType.GetCustomAttributes(attributeType, false);
			
			if (null == attributes || 0 == attributes.Length)
			{
				Type[] interfaces = interfaceType.GetInterfaces();
				for(int i=0; i<interfaces.Length; i++)
				{
					interfaceType = interfaces[i];
					attributes = interfaceType.GetCustomAttributes(attributeType, false);
					if (null != attributes && 0 != attributes.Length) break;
				}
			}
			
			if (null == attributes || 0 == attributes.Length) return null;
			return (ComProgIdAttribute)attributes[0];
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">
		/// The COM ProgID.
		/// </param>
		public ComProgIdAttribute(string value)
		{
			_value = value;
		}
		
		/// <summary>
		/// Returns the COM ProgID
		/// </summary>
		public string Value
		{
			get { return _value; }
		}
	}
}
