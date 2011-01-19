using System;
using System.Collections.Generic;
using System.Text;
using BDika.Client.API.Comm;

namespace BDika.Client.API
{
    public class TestingClientException : Exception
    {
        public uint ErrorCode = 0;

        public TestingClientException(uint errorcode)
        {
            this.ErrorCode = errorcode;
        }

        public TestingClientException(ErrorCodes errorcode)
        {
            this.ErrorCode =(uint)errorcode;
        }
        
    }
}
