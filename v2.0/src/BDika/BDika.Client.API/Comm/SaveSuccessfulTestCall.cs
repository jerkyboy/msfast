using System;
using System.Collections.Generic;
using System.Text;
using BDika.Client.API.Tests;
using System.Xml;
using MySpace.MSFast.ImportExportsMgrs;
using System.IO;

namespace BDika.Client.API.Comm
{
    public class SaveSuccessfulTestCall : TestingClientCall<ServerResponse>
    {
        public TestIteration TestIteration;

        public override ServerResponse ExecuteCall(string clientID, string clientKey, int timeout)
        {
            if (this.TestIteration == null || this.TestIteration.ResultsID == 0 || this.TestIteration.ProcessedDataPackage == null)
                throw new Exception("Invalid Test Iteration");

            return base.ExecuteCall(clientID, clientKey, timeout);
        }

        public override void PrepareArguments()
        {
            base.PrepareArguments();

            MSFImportExportsManager mie = new MSFImportExportsManager();
            MemoryStream ms = null;
            
            try{
                ms = new MemoryStream();
                mie.SaveProcessedDataPackage(ms, TestIteration.ProcessedDataPackage);
                
                byte[] data = ms.ToArray();

                AppendParam("d", Convert.ToBase64String(data));
            }
            catch
            {
            }
            finally
            {
                if(ms != null)
                    ms.Close();
            }

            AppendParam("r", this.TestIteration.ResultsID);
        }

        public override Uri GetURL() {
            return new Uri("http://bdika/Handlers/ClientServices/SaveSuccessfulTestServiceHandler.axd"); 
        }
    }
}
