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
using MySpace.MSFast.Automation.Entities.Tests;

namespace MySpace.MSFast.Automation.Entities.Triggers
{
    [Serializable]
    public sealed class TriggerToTestAndTesterTypeID : EntityIdentifier<TriggerToTestAndTesterTypeID>
    {
        private string _id = "BPI";

        public TriggerID TriggerID;
        public TestID TestID;
        public TesterTypeID TesterTypeID;
        
        public TriggerToTestAndTesterTypeID()
        {
        }
        
        public TriggerToTestAndTesterTypeID(TriggerID TriggerID, TesterTypeID TesterTypeID, TestID TestID)
        {
            this.TriggerID = TriggerID;
            this.TesterTypeID = TesterTypeID;
            this.TestID = TestID;

            this._id = String.Concat((uint)TriggerID, "_", (uint)TesterTypeID, "_", (uint)TestID);
        }

        public TriggerToTestAndTesterTypeID(string _id)
        {
            this._id = _id;
        }

        public static implicit operator TriggerToTestAndTesterTypeID(string i) { return new TriggerToTestAndTesterTypeID(i); }
        public static implicit operator string(TriggerToTestAndTesterTypeID i) { return (i != null ? i._id : null); }

        public override string ToString() { return UniqueIdetifier; }

        public override String UniqueIdetifier
        {
            get
            {
                return this._id;
            }
        }
        public override object ColumnValue
        {
            get
            {
                return this._id;
            }
        }

    }  

    [Serializable]
    [DBEntity("triggertotestandtestertype", SelectAfterInsert = false)]
    public class TriggerToTestAndTesterType : Entity
    {
        [EntityAggregateFieldValue(AggregateType.Concat, "triggerid", "testertypeid","testid")]
        [EntityIdentity("id")]
        public TriggerToTestAndTesterTypeID TriggerToTestAndTesterTypeID = new TriggerToTestAndTesterTypeID(0, 0, 0);

        [EntityField("triggerid")]
        public TriggerID TriggerID;

        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;
        
        [EntityField("testid")]
        public TestID TestID;
    }
}
