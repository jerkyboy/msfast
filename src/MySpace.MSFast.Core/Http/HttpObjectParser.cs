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
    public abstract class HttpObjectParser
    {
        public delegate void HeaderDelegator(HttpObjectParser parser, String header, String value, String rawheader);

        public int HeaderLength = 0;

        private bool isInitiated = false;

        public bool IsInitiated()
        {
            return isInitiated;
        }

        public void Initiate(string header)
        {
            ParseHeader(header);
            this.isInitiated = true;
        }

        public virtual void ParseHeader(string rawHeader)
        {
            if (rawHeader == null) return;

            HeaderLength = rawHeader.Length;

            String[] headers = Regex.Split(rawHeader, "\r\n");

            if (headers.Length <= 0) return;

            HeaderDelegator parser = null;
            Match mc = null;
            
            foreach (String headerRaw in headers)
            {
                if (headerRaw.IndexOf(' ') != -1)
                {
                    foreach (Regex m in Parsers.Keys)
                    {
                        mc = m.Match(headerRaw);
                        if (mc != null && mc.Success)
                        {
                            try
                            {
                                
                                parser = Parsers[m];
                                parser(this, mc.Groups["header"].Value, mc.Groups["value"].Value, headerRaw);
                            }
                            catch
                            {
                            }
                            break;
                        }
                    }
                }
            }
        }


        public abstract Dictionary<Regex, HeaderDelegator> Parsers { get; }
        public virtual void OnData(byte[] b_buffer, int index, int length,int available) { }
        public virtual int GetExpectedLength() { return -1; }
    }
}
