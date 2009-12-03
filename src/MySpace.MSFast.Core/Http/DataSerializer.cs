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
using System.IO;
using MySpace.MSFast.Core.Http;
using MySpace.MSFast.Core.Utils;

namespace MySpace.MSFast.Core.Http
{
	public class DataSerializer
	{
        public static bool SaveHttpTransactions(Stream output, IEnumerable<HttpTransaction> data)
		{
			try
			{
                StreamWriter sw = new StreamWriter(output);

                foreach (HttpTransaction transaction in data)
				{
					if (transaction.IsTrackable == false)
						continue;

                    sw.Write("GUID");
                    sw.Write(transaction.FileGUID);

                    sw.Write("CNST");
                    sw.Write(Converter.DateTimeToEpoch(transaction.ConnectionStartTime));

					sw.Write("SRST");
                    sw.Write(Converter.DateTimeToEpoch(transaction.SendingRequestStartTime));

					sw.Write("SRET");
                    sw.Write(Converter.DateTimeToEpoch(transaction.SendingRequestEndTime));

					sw.Write("RRST");
                    sw.Write(Converter.DateTimeToEpoch(transaction.ReceivingResponseStartTime));


					if (transaction.Mode == HttpMode.Completed)
					{
						sw.Write("RRET");
                        sw.Write(Converter.DateTimeToEpoch(transaction.ReceivingResponseEndTime));
						sw.Write("CNET");
                        sw.Write(Converter.DateTimeToEpoch(transaction.ConnectionEndTime));
					}
					else
					{
						sw.Write("RRET");
						sw.Write("-1");
						sw.Write("CNET");
						sw.Write("-1");
					}

					sw.Write("TTSN");
					sw.Write(transaction.TotalSent);

					sw.Write("TTRC");
					sw.Write(transaction.TotalReceived);

					sw.Write("~");

					sw.Write(transaction.URL);

					sw.Write("\r\n");
				}
				sw.Flush();
				sw.Close();
			}
			catch
			{
				return false;
			}
			return true;
		}

		
	}
}
