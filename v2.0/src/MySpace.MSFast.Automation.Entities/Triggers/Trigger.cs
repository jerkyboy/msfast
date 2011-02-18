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
using EYF.Core.Exceptions;
using MySpace.MSFast.Automation.Entities.Collectors;

namespace MySpace.MSFast.Automation.Entities.Triggers
{
    #region TriggerID
    public class InvalidTriggerIDException : EYFException { }

    [Serializable]
    public class TriggerID : EntityIdentifier<TriggerID>
    {
        public static bool ValidateTriggerID(TriggerID TriggerID)
        {
            if (IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();
            return true;
        }
        public static bool IsValidTriggerID(TriggerID TriggerID)
        {
            return TriggerID != 0;
        }

        private uint _id;

        public TriggerID(uint _id)
        {
            this._id = _id;
        }

        public static implicit operator TriggerID(uint i) { return new TriggerID(i); }
        public static implicit operator uint(TriggerID i) { return (i != null ? i._id : 0); }

        public override string ToString() { return UniqueIdetifier; }

        public override String UniqueIdetifier
        {
            get
            {
                return this._id.ToString();
            }
        }
        public override object ColumnValue
        {
            get
            {
                return this._id;
            }
        }
        public override int GetHashCode() { return (int)_id; }
    }
    #endregion


    public enum TriggerType : ushort
    {
        Unknown = 0,
        Time = 1,
        Manual = 2
    }

    [Serializable]
    [CachableEntity("Trigger")]
    [DBEntity("triggers")]
    public class Trigger : ConfigurableEntity
    {
        [EntityIdentity("triggerid")]
        public TriggerID TriggerID;
        
        [EntityField("name",Size = 45)]
        public String TriggerName;

        [EntityField("lasttriggered")]
        public DateTime LastTriggered = new DateTime(1971,1,1);

        [EntityField("triggertype")]
        public ushort TriggerTypeID;
        public TriggerType TriggerType
        {
            get
            {
                return (TriggerType)this.TriggerTypeID;
            }
            set
            {
                this.TriggerTypeID = (ushort)value;
            }
        }

        [EntityField("enabled")]
        public bool Enabled = true;

        [EntityField("timeout")]
        public uint Timeout = 0;

    }

    [Serializable]
    [EntityCommand(EntityCommandType.Update, typeof(Trigger))]
    [DBEntity("triggers")]
    public class UpdateTriggerDetailsEntityCommand : Entity
    {
        [EntityIdentity("triggerid")]
        public TriggerID TriggerID;

        [EntityField("name", Size = 45)]
        public String TriggerName;

        [EntityField("triggertype")]
        public ushort TriggerTypeID;
        public TriggerType TriggerType
        {
            get
            {
                return (TriggerType)this.TriggerTypeID;
            }
            set
            {
                this.TriggerTypeID = (ushort)value;
            }
        }

        [EntityField("enabled")]
        public bool Enabled = true;

        [EntityField("timeout")]
        public uint Timeout = 0;
    }

    [EntityCommand(EntityCommandType.Update, typeof(Trigger))]
    [DBEntity("triggers")]
    public class LastTriggerEntityCommand : Entity
    {
        [EntityIdentity("triggerid")]
        public TriggerID TriggerID;
        
        [EntityField("lasttriggered")]
        public DateTime LastTriggered;
    }
}
