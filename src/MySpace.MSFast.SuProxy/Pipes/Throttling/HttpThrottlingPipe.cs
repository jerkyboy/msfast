//=======================================================================
/* Project: MSFast (MySpace.MSFast.SuProxy)
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
using System.Threading;

namespace MySpace.MSFast.SuProxy.Pipes.Throttling
{
	public class HttpThrottlingPipe : HttpPipe
	{
		private int kbps = -1;
        private int sentWithoutSleep = 0;

		public override void Init(Dictionary<object, object> dictionary)
		{
			base.Init(dictionary);

			int i_kbps = int.Parse((String)dictionary["kbps"]);
			kbps = 102 * i_kbps;
            sentWithoutSleep = kbps;
		}

		public override void SendData(byte[] buffer, int offset, int length)
		{
			if (length == 0)
				return;

			if (kbps == -1)
			{
				base.SendData(buffer, offset, length);
				return;
			}

            do{
                if(sentWithoutSleep <= 0){
                    Thread.Sleep(100);
                    sentWithoutSleep = kbps;
                }
                int min = Math.Min(length,sentWithoutSleep);
                
                base.SendData(buffer,offset,min);
                sentWithoutSleep -= min;
                offset += min;
                length -= min;

            }while( length > 0 && offset + length <= buffer.Length);
		}
	}
}
