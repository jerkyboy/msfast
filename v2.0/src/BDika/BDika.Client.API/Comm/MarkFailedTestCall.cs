﻿using System;
using System.Collections.Generic;
using System.Text;
using BDika.Client.API.Tests;

namespace BDika.Client.API.Comm
{
    public class MarkFailedTestCall : TestingClientCall<ServerResponse>
    {
        public TestIteration TestIteration;

        public override ServerResponse ExecuteCall(string clientID, string clientKey, int timeout)
        {
            if (this.TestIteration == null || this.TestIteration.ResultsID == 0)
                throw new Exception("Invalid Test Iteration");

            return base.ExecuteCall(clientID, clientKey, timeout);
        }

        public override void PrepareArguments()
        {
            base.PrepareArguments();

            AppendParam("r", this.TestIteration.ResultsID);
        }

        public override Uri GetURL() {
            return new Uri("http://bdika/Handlers/ClientServices/MarkFailedTestServiceHandler.axd"); 
        }
    }
}