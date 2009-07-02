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
using System.Text.RegularExpressions;

namespace MySpace.MSFast.Core.Http
{
	public class HttpRequestParser : HttpObjectParser
	{
		public String URL;
		public String Host;
		public int ContentLength = 0;

		private static readonly Dictionary<Regex, HeaderDelegator> parsers = new Dictionary<Regex, HeaderDelegator>();

		static HttpRequestParser()
		{
			parsers.Add(new Regex("(?<header>GET|POST|CONNECT) (?<value>.*)", RegexOptions.Compiled), delegate(HttpObjectParser parser, String header, String value, String rawheader)
			{
				int io = value.IndexOf(' ');
				String url = value.Substring(0, io);
				((HttpRequestParser)parser).URL = url;
			});

			parsers.Add(new Regex("(?<header>Content-Length): (?<value>.*)", RegexOptions.Compiled), delegate(HttpObjectParser parser, String header, String value, String rawheader)
			{
				((HttpRequestParser)parser).ContentLength = Int32.Parse(value);
			});
			parsers.Add(new Regex("(?<header>Host): (?<value>.*)", RegexOptions.Compiled), delegate(HttpObjectParser parser, String header, String value, String rawheader)
			{
				((HttpRequestParser)parser).Host = value;
			});
		}

		public override Dictionary<Regex, HeaderDelegator> Parsers { get { return parsers; } }

		public override int GetExpectedLength()
		{
			if (this.IsInitiated() == false)
				return -1;

			if (this.ContentLength == 0)
				return this.HeaderLength;

			return this.ContentLength + this.HeaderLength;
		}
	}
}
