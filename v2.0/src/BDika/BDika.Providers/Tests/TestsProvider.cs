using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDika.Entities.Tests;
using EYF.Providers.Gateway;
using EYF.Entities.StandardObjects;
using BDika.Entities.Triggers;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using BDika.Entities.Collectors;

namespace BDika.Providers.Tests
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
                EntitiesGateway.UpdateEntity(testertype);
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

        public static bool UpdateTesterTypeState(UserID userID, TestID TestID, TesterTypeID TesterTypeID, TriggerID TriggerID, bool Selected)
        {
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();

            if (TriggerID.IsValidTriggerID(TriggerID))
            {
                if (Selected)
                {
                    EntitiesGateway.AddEntity(new TriggerToTestAndTesterType()
                    {
                        TesterTypeID = TesterTypeID,
                        TestID = TestID,
                        TriggerID = TriggerID,
                        TriggerToTestAndTesterTypeID = new TriggerToTestAndTesterTypeID(TriggerID,TesterTypeID,TestID)
                    });

                    return true;
                }
                else
                {
                    return EntitiesGateway.DeleteEntity(new TriggerToTestAndTesterType()
                    {
                        TesterTypeID = TesterTypeID,
                        TestID = TestID,
                        TriggerID = TriggerID,
                        TriggerToTestAndTesterTypeID = new TriggerToTestAndTesterTypeID(TriggerID, TesterTypeID, TestID)
                    });
                }
            }
            else
            {
                if (Selected)
                {
                    EntitiesGateway.AddEntity(new TesterTypeToTest()
                    {
                        TesterTypeID = TesterTypeID,
                        TestID = TestID,
                        TesterTypeToTestID = new TesterTypeToTestID(TesterTypeID, TestID)
                    });

                    return true;
                }
                else
                {
                    return EntitiesGateway.DeleteEntity(new TesterTypeToTest()
                    {
                        TesterTypeID = TesterTypeID,
                        TestID = TestID,
                        TesterTypeToTestID = new TesterTypeToTestID(TesterTypeID, TestID)
                    });
                }
            }
        }
       
    }
}
