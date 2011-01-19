using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BDika.Client.API.Comm
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
