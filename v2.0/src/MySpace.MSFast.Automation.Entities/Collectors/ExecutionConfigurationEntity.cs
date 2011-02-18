//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Entities)
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
using EYF.Entities;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Entities.Tests;

using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace MySpace.MSFast.Automation.Entities.Collectors
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

        [EntityField("triggertotestandtestertype")]
        public String TesterTypeAndTestAndTriggerConfiguration;
    }

    public class UpdateConfigEntityCommand : ConfigurableEntity
    {
    }

    [Serializable]
    [EntityCommand(EntityCommandType.Update, typeof(Test))]
    [DBEntity("tests")]
    public class UpdateConfig_Test_EntityCommand : UpdateConfigEntityCommand
    {
        [EntityIdentity("testid")]
        public TestID TestID;
    }

    [Serializable]
    [EntityCommand(EntityCommandType.Update, typeof(TesterType))]
    [DBEntity("testertypes")]
    public class UpdateConfig_TesterType_EntityCommand : UpdateConfigEntityCommand
    {
        [EntityIdentity("testertypeid")]
        public TesterTypeID TesterTypeID;
    }

    [Serializable]
    [EntityCommand(EntityCommandType.Update, typeof(Trigger))]
    [DBEntity("triggers")]
    public class UpdateConfig_Trigger_EntityCommand : UpdateConfigEntityCommand
    {
        [EntityIdentity("triggerid")]
        public TriggerID TriggerID;
    }


    [Serializable]
    [EntityCommand(EntityCommandType.Update, typeof(TriggerToTestAndTesterType))]
    [DBEntity("triggertotestandtestertype")]
    public class UpdateConfig_TriggerToTestAndTesterType_EntityCommand : UpdateConfigEntityCommand
    {
        [EntityAggregateFieldValue(AggregateType.Concat, "triggerid", "testertypeid", "testid")]
        [EntityIdentity("id")]
        public TriggerToTestAndTesterTypeID ID = new TriggerToTestAndTesterTypeID(0, 0, 0);

        [EntityField("triggerid")]
        public TriggerID TriggerID;

        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;

        [EntityField("testid")]
        public TestID TestID;
    }
}
