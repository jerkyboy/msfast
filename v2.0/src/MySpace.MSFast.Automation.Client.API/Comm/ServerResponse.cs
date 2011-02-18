//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.API)
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
using Newtonsoft.Json;

namespace MySpace.MSFast.Automation.Client.API.Comm
{
    public enum ErrorCodes : uint { 
        UnexpectedError =    9000001,
        InvalidCredentials = 9000002,
        UnexpectedResponse = 9000003,
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ServerResponse
    {
        [JsonProperty]
        public bool IsSucceeded;

        [JsonProperty]
        public uint ErrorCode;

        [JsonProperty]
        public Dictionary<String, String> ResponseDate;

        public ServerResponse() { }

        public virtual void Deserialize(){}

        public virtual String Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
