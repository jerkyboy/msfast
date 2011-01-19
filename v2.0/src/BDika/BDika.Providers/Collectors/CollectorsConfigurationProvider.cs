using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDika.Entities.Collectors;
using BDika.Entities.Triggers;
using BDika.Entities.Tests;
using EYF.Providers.Gateway;
using EYF.Entities;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace BDika.Providers.Collectors
{
    public static class CollectorsConfigurationProvider
    {
        #region GET
        public static ExtCollectorsConfig GetExtCollectorsConfig(TriggerID TriggerID)
        {
            if (TriggerID.IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();
            return LoadFrom(GetExtCollectorsConfigEntity(TriggerID));
        }
        public static ExtCollectorsConfig GetExtCollectorsConfig(TestID TestID)
        {
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            return LoadFrom(GetExtCollectorsConfigEntity(TestID));
        }
        public static ExtCollectorsConfig GetExtCollectorsConfig(TesterTypeID TesterTypeID)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            return LoadFrom(GetExtCollectorsConfigEntity(TesterTypeID));
        }
        public static ExtCollectorsConfig GetExtCollectorsConfig(TestID TestID, TesterTypeID TesterTypeID)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            return LoadFrom(GetExtCollectorsConfigEntity(TestID, TesterTypeID));
        }
        public static ExtCollectorsConfig GetExtCollectorsConfig(TriggerID TriggerID, TestID TestID, TesterTypeID TesterTypeID)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            if (TriggerID.IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();
            return LoadFrom(GetExtCollectorsConfigEntity(TriggerID, TestID, TesterTypeID));
        }
        private static ExtCollectorsConfigEntity GetExtCollectorsConfigEntity(TriggerID TriggerID)
        {
            if (TriggerID.IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();
            return EntitiesGateway.GetEntity<ExtCollectorsConfigEntity>(new TriggerToTestAndTesterTypeID(TriggerID,0,0));
        }
        private static ExtCollectorsConfigEntity GetExtCollectorsConfigEntity(TestID TestID)
        {
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            return EntitiesGateway.GetEntity<ExtCollectorsConfigEntity>(new TriggerToTestAndTesterTypeID(0,0,TestID));
        }
        private static ExtCollectorsConfigEntity GetExtCollectorsConfigEntity(TesterTypeID TesterTypeID)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            return EntitiesGateway.GetEntity<ExtCollectorsConfigEntity>(new TriggerToTestAndTesterTypeID(0, TesterTypeID, 0));
        }
        private static ExtCollectorsConfigEntity GetExtCollectorsConfigEntity(TestID TestID, TesterTypeID TesterTypeID)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            return EntitiesGateway.GetEntity<ExtCollectorsConfigEntity>(new TriggerToTestAndTesterTypeID(0, TesterTypeID, TestID));
        }
        private static ExtCollectorsConfigEntity GetExtCollectorsConfigEntity(TriggerID TriggerID, TestID TestID, TesterTypeID TesterTypeID)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            if (TriggerID.IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();
            return EntitiesGateway.GetEntity<ExtCollectorsConfigEntity>(new TriggerToTestAndTesterTypeID(TriggerID, TesterTypeID, TestID));
        }

        private static ExtCollectorsConfig LoadFrom(ExtCollectorsConfigEntity res)
        {
            if (res == null)
                return null;

            ExtCollectorsConfig ec = new ExtCollectorsConfig();
            ec.AppendConfig(new ExtCollectorsConfigLoader(res));

            return ec;
        }
        #endregion

        #region CollectorsArgument
        public static bool SetArgument(TriggerID TriggerID, String key, String value)
        {
            if (TriggerID.IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TriggerID);
            ExtCollectorsConfig ec = new ExtCollectorsConfig();

            if (ece != null && String.IsNullOrEmpty(ece.TriggerConfiguration) == false)
                ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TriggerConfiguration));

            ec.SetArgument(key, value);

            return EntitiesGateway.UpdateEntity(new UpdateConfig_Trigger_EntityCommand() { TriggerID = TriggerID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }
        public static bool SetArgument(TestID TestID, String key, String value)
        {
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TestID);

            ExtCollectorsConfig ec = new ExtCollectorsConfig();

            if (ece != null && String.IsNullOrEmpty(ece.TestConfiguration) == false)
                ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TestConfiguration));

            ec.SetArgument(key, value);

            return EntitiesGateway.UpdateEntity(new UpdateConfig_Test_EntityCommand() { TestID = TestID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }
        public static bool SetArgument(TesterTypeID TesterTypeID, String key, String value)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TesterTypeID);

            ExtCollectorsConfig ec = new ExtCollectorsConfig();

            if (ece != null && String.IsNullOrEmpty(ece.TesterTypeConfiguration) == false)            
                ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TesterTypeConfiguration));

            ec.SetArgument(key, value);

            return EntitiesGateway.UpdateEntity(new UpdateConfig_TesterType_EntityCommand() { TesterTypeID = TesterTypeID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }
        public static bool SetArgument(TestID TestID, TesterTypeID TesterTypeID, String key, String value)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TestID, TesterTypeID);

            ExtCollectorsConfig ec = new ExtCollectorsConfig();

            if (ece != null && String.IsNullOrEmpty(ece.TesterTypeAndTestConfiguration) == false)
                ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TesterTypeAndTestConfiguration));

            ec.SetArgument(key, value);

            return EntitiesGateway.UpdateEntity(new UpdateConfig_TesterTypeToTest_EntityCommand() { TestID = TestID, TesterTypeID = TesterTypeID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }
        public static bool SetArgument(TriggerID TriggerID, TestID TestID, TesterTypeID TesterTypeID, String key, String value)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            if (TriggerID.IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();

            ExtCollectorsConfig ec = new ExtCollectorsConfig();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TriggerID, TestID, TesterTypeID);

            if (ece != null && String.IsNullOrEmpty(ece.TesterTypeAndTestAndTriggerConfiguration) == false)
                ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TesterTypeAndTestAndTriggerConfiguration));

            ec.SetArgument(key, value);

            return EntitiesGateway.UpdateEntity(new UpdateConfig_TriggerToTestAndTesterType_EntityCommand() { TriggerID = TriggerID, TestID = TestID, TesterTypeID = TesterTypeID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }

        public static bool RemoveArgument(TriggerID TriggerID, String key)
        {
            if (TriggerID.IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TriggerID);

            if (ece == null || String.IsNullOrEmpty(ece.TriggerConfiguration))
                return true;

            ExtCollectorsConfig ec = new ExtCollectorsConfig();            
            ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TriggerConfiguration));
            ec.RemoveArgument(key);

            return EntitiesGateway.UpdateEntity(new UpdateConfig_Trigger_EntityCommand() { TriggerID = TriggerID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }
        public static bool RemoveArgument(TestID TestID, String key)
        {
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TestID);

            if (ece == null || String.IsNullOrEmpty(ece.TestConfiguration))
                return true;

            ExtCollectorsConfig ec = new ExtCollectorsConfig();
            ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TestConfiguration));
            ec.RemoveArgument(key); 

            return EntitiesGateway.UpdateEntity(new UpdateConfig_Test_EntityCommand() { TestID = TestID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }
        public static bool RemoveArgument(TesterTypeID TesterTypeID, String key)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TesterTypeID);

            if (ece == null || String.IsNullOrEmpty(ece.TesterTypeConfiguration))
                return true;

            ExtCollectorsConfig ec = new ExtCollectorsConfig();
            ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TesterTypeConfiguration));
            ec.RemoveArgument(key);

            return EntitiesGateway.UpdateEntity(new UpdateConfig_TesterType_EntityCommand() { TesterTypeID = TesterTypeID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }
        public static bool RemoveArgument(TestID TestID, TesterTypeID TesterTypeID, String key)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TestID, TesterTypeID);

            if (ece == null || String.IsNullOrEmpty(ece.TesterTypeAndTestConfiguration))
                return true;

            ExtCollectorsConfig ec = new ExtCollectorsConfig();
            ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TesterTypeAndTestConfiguration));
            ec.RemoveArgument(key);

            return EntitiesGateway.UpdateEntity(new UpdateConfig_TesterTypeToTest_EntityCommand() { TestID = TestID, TesterTypeID = TesterTypeID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }
        public static bool RemoveArgument(TriggerID TriggerID, TestID TestID, TesterTypeID TesterTypeID, String key)
        {
            if (TesterTypeID.IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            if (TestID.IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            if (TriggerID.IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();

            ExtCollectorsConfigEntity ece = GetExtCollectorsConfigEntity(TriggerID, TestID, TesterTypeID);

            if (ece == null || String.IsNullOrEmpty(ece.TesterTypeAndTestAndTriggerConfiguration))
                return true;

            ExtCollectorsConfig ec = new ExtCollectorsConfig();
            ec.AppendConfig(new JSONCollectorsConfigLoader(ece.TesterTypeAndTestAndTriggerConfiguration));
            ec.RemoveArgument(key);

            return EntitiesGateway.UpdateEntity(new UpdateConfig_TriggerToTestAndTesterType_EntityCommand() { TriggerID = TriggerID, TestID = TestID, TesterTypeID = TesterTypeID, RawConfiguration = new JSONCollectorsConfigWriter().SerializeConfig(ec) });
        }
        #endregion
    }
}
