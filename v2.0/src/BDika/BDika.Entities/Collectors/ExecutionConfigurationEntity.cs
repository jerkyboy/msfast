using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities;
using BDika.Entities.Triggers;
using BDika.Entities.Tests;

using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace BDika.Entities.Collectors
{
    [Serializable]
    [DBEntity("config")]
    public class ExtCollectorsConfigEntity : Entity
    {
        [EntityAggregateFieldValue(AggregateType.Concat, "triggerid", "testertypeid", "testid")]
        [EntityIdentity("id")]
        public TriggerToTestAndTesterTypeID ConfigID = new TriggerToTestAndTesterTypeID(0, 0, 0);

        [EntityField("triggerid")]
        public TriggerID TriggerID;

        [EntityField("testid")]
        public TestID TestID;

        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;

        [EntityField("triggers")]
        public String TriggerConfiguration;
        
        [EntityField("tests")]
        public String TestConfiguration;
        
        [EntityField("testertypes")]
        public String TesterTypeConfiguration;
        
        [EntityField("testertypestotest")]
        public String TesterTypeAndTestConfiguration;

        [EntityField("triggertotestandtestertype")]
        public String TesterTypeAndTestAndTriggerConfiguration;
    }

    public class UpdateConfigEntityCommand : Entity
    {
        [EntityIdentity("id")]
        public UInt32EntityIdentifier ID = 0;

        [EntityField("configuration")]
        public String RawConfiguration;
    }

    [Serializable]
    [DBEntity("tests")]
    public class UpdateConfig_Test_EntityCommand : UpdateConfigEntityCommand
    {
        [EntityField("testid")]
        public TestID TestID;
    }

    [Serializable]
    [DBEntity("testertypes")]
    public class UpdateConfig_TesterType_EntityCommand : UpdateConfigEntityCommand
    {
        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;
    }

    [Serializable]
    [DBEntity("testertypestotest")]
    public class UpdateConfig_TesterTypeToTest_EntityCommand : UpdateConfigEntityCommand
    {
        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;

        [EntityField("testid")]
        public TestID TestID;
    }

    [Serializable]
    [DBEntity("triggers")]
    public class UpdateConfig_Trigger_EntityCommand : UpdateConfigEntityCommand
    {
        [EntityField("triggerid")]
        public TriggerID TriggerID;
    }

    [Serializable]
    [DBEntity("triggertotestandtestertype")]
    public class UpdateConfig_TriggerToTestAndTesterType_EntityCommand : UpdateConfigEntityCommand
    {
        [EntityField("triggerid")]
        public TriggerID TriggerID;

        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;

        [EntityField("testid")]
        public TestID TestID;
    }
}
