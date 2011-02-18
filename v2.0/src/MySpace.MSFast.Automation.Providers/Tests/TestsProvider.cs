//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.Providers)
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
using System.Linq;
using System.Text;
using MySpace.MSFast.Automation.Entities.Tests;
using EYF.Providers.Gateway;
using EYF.Entities.StandardObjects;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using MySpace.MSFast.Automation.Entities.Collectors;

namespace MySpace.MSFast.Automation.Providers.Tests
{
    public class TestsProvider
    {

        public static Test UpdateOrCreateTest(UserID userid, Test test)
        {
            if (test == null) throw new Exception("Invalid Test");
            if (String.IsNullOrEmpty(test.TestName)) throw new Exception("Invalid Test Name");

            if (TestID.IsValidTestID(test.TestID))
            {
                EntitiesGateway.UpdateEntity(test);
                return test;
            }
            else
            {
                return EntitiesGateway.AddEntity(test);
            }
        }

        public static Test GetTest(UserID userid, TestID testID)
        {
            if (TestID.IsValidTestID(testID) == false) throw new InvalidTestIDException();
            
            return EntitiesGateway.GetEntity<Test>(testID);
        }

        public static bool DeleteTest(UserID userid, TestID testID)
        {
            if (TestID.IsValidTestID(testID) == false) throw new InvalidTestIDException();

            return EntitiesGateway.DeleteEntity(new Test() { TestID = testID });
        }

        public static TesterType UpdateOrCreateTesterType(UserID userID, TesterType testertype)
        {
            if (testertype == null) throw new Exception("Invalid Tester Type");
            if (String.IsNullOrEmpty(testertype.Name)) throw new Exception("Invalid Tester Type Name");

            if (TesterTypeID.IsValidTesterTypeID(testertype.TesterTypeID))
            {
                EntitiesGateway.UpdateEntity(new UpdateTesterTypeEntityCommand()
                {
                    TesterTypeID = testertype.TesterTypeID,
                    Name = testertype.Name
                });

                return testertype;
            }
            else
            {
                return EntitiesGateway.AddEntity(testertype);
            }
        }

        public static TesterType GetTesterType(UserID userID, TesterTypeID testerTypeID)
        {
            if (TesterTypeID.IsValidTesterTypeID(testerTypeID) == false) throw new InvalidTesterTypeIDException();

            return EntitiesGateway.GetEntity<TesterType>(testerTypeID);
        }

        public static bool DeleteTesterType(UserID userid, TesterTypeID testerTypeID)
        {
            if (TesterTypeID.IsValidTesterTypeID(testerTypeID) == false) throw new InvalidTesterTypeIDException();

            return EntitiesGateway.DeleteEntity(new TesterType() { TesterTypeID = testerTypeID });
        }
    }
}
