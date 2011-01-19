﻿using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using Newtonsoft.Json;

namespace BDika.Client.API.Comm
{
    public class GetNextTestQueCall : TestingClientCall<GetNextTestQueServerResponse>
    {
        public override Uri GetURL(){ return new Uri("http://bdika/Handlers/ClientServices/GetNextTestInQueServiceHandler.axd"); }
    }
    
    public class GetNextTestQueServerResponse : ServerResponse
    {
        [JsonProperty]
        public String RawConfig;
        
        [JsonProperty]
        public uint ResultsID = 0;
    }
}
