using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using BDika.Client.API.Tests;

namespace BDika.Tasks.TestsExecuter.TestExecuters
{
    public interface ITestRunner
    {
        bool RunTest(TestIteration testIteration);
    }
}
