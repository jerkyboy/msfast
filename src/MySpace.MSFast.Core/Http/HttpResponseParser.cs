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
	public class HttpResponseParser : HttpObjectParser
	{
		
		#region Response Header Properties
		public enum Encoding
		{
			Chunked,
			Unknown
		}
		public Encoding TransferEncoding = Encoding.Unknown;
		public int ContentLength = -1;
		public int ResponseCode = 0;
        public String Connection = null;

        //Hack to determine if http 1.0 closed (Socket won't close??) 
        private int countZeroAvailable = 0;
        private int available = 0;

		#endregion

		private static readonly Dictionary<Regex, HeaderDelegator> parsers = new Dictionary<Regex, HeaderDelegator>();

		static HttpResponseParser()
		{
			parsers.Add(new Regex("(?<header>HTTP/.*?) (?<value>[0-9]{3}) ", RegexOptions.Compiled), delegate(HttpObjectParser parser, String header, String value, String rawheader)
			{
				((HttpResponseParser)parser).ResponseCode = Int32.Parse(value);
			});

			parsers.Add(new Regex("(?<header>Transfer-Encoding): (?<value>.*)", RegexOptions.Compiled), delegate(HttpObjectParser parser, String header, String value, String rawheader)
			{
				if (value.Trim() == "chunked") ((HttpResponseParser)parser).TransferEncoding = Encoding.Chunked;
			});
            parsers.Add(new Regex("(?<header>Connection): (?<value>.*)", RegexOptions.Compiled), delegate(HttpObjectParser parser, String header, String value, String rawheader)
            {
                ((HttpResponseParser)parser).Connection = value.Trim();
            });

			parsers.Add(new Regex("(?<header>Content-Length): (?<value>.*)", RegexOptions.Compiled), delegate(HttpObjectParser parser, String header, String value, String rawheader)
			{
				((HttpResponseParser)parser).ContentLength = Int32.Parse(value);
			});
		}

		public override Dictionary<Regex, HeaderDelegator> Parsers{get{return parsers;}}

		public override void ParseHeader(string rawHeader)
		{
			base.ParseHeader(rawHeader);

			if (this.ContentLength != -1)
				this.expectedLength += rawHeader.Length + this.ContentLength;
		}

		private int expectedLength = -1;
		private int totalData = 0;
		
		public override int GetExpectedLength()
		{
			if (this.ResponseCode == 100)
			{
			    return -2;
            }
            else if ((Connection == "close" || Connection == null) && this.ContentLength == -1)
            {
                if (available == 0 && countZeroAvailable > 10)
                {
                    return 0;
                }else{
                    return -1;
                    }
            }
			return expectedLength;
		}

		public override void OnData(byte[] b_buffer, int index, int length,int available)
		{
            this.available = available;
            if (this.available > 0)
            {
                this.countZeroAvailable = 0;
            }
            else
            {
                countZeroAvailable++;
            }

			totalData += length;

			if (!this.IsInitiated() || (this.TransferEncoding != Encoding.Chunked && this.ContentLength != -1))
			{
				return;
			}
			else if (this.TransferEncoding != Encoding.Chunked)
            {
				expectedLength = this.totalData;
			}
			else if (length != 0 && this.HeaderLength < totalData) //We have more than the header
			{
				UpdateExpectedChunk(b_buffer, index, length);
			}
		}

		#region Chunked Parser

		private void UpdateExpectedChunk(byte[] b_buffer, int index, int length)
		{
			if (this.expectedLength > this.totalData)
			{
				return;
			}
			else if (this.expectedLength == this.totalData)
			{
				this.expectedLength = -1;
				return;
			}

			if (this.expectedLength != -1)
			{
				index += length - (this.totalData - this.expectedLength);
				length = this.totalData - this.expectedLength;
			}
			if (totalData - length < this.HeaderLength)
			{
				int skip = this.HeaderLength - (totalData - length);
				index += skip;
				length -= skip;
			}
			String lengthStr = String.Empty;

			for (int i = index; i < index + length; i++)
			{
				//First char must be a hex
				if (lengthStr.Length == 0 && !IsHexChar(b_buffer[i]))
					throw new Exception("Invalid Response!");

				if (IsHexChar(b_buffer[i]))
				{
					lengthStr += (char)b_buffer[i];
				}
				else // Finish reading chunked/size
				{
					int next = int.Parse(lengthStr, System.Globalization.NumberStyles.HexNumber, null);

					//Scroll the chunk header till we reach the end of it (\r\n)
					for (; i < index + length - 1; i++)
					{
						if (b_buffer[i] == '\r' && b_buffer[i + 1] == '\n')
						{
							if (next != 0 && next + 4 == length - (i - index))
							{ //our buffer has the exact latest chunk, but next != 0 so we need to continue reading 
								this.expectedLength = -1;
								return;
							}
							else if (next != 0 && next + 4 < length - (i - index))
							{ //our buffer has more than the latest chunk size, skip chunk and continue to next chunk
								i += next + 3;
								lengthStr = String.Empty;
								break;
							}
							else
							{ // our buffer don't have the entire chunk
								this.expectedLength = (this.totalData - (length - (i - index))) + next + 4;
								return;
							}
						}
					}
				}
			}
			this.expectedLength = this.totalData;
		}

		private bool IsHexChar(byte c)
		{
			return ((c >= 48 && c <= 57) || (c >= 65 && c <= 70) || (c >= 97 && c <= 102));
		}
		#endregion
	}

}
