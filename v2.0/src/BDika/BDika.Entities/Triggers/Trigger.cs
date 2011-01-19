using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities;
using EYF.Core.Exceptions;
using BDika.Entities.Collectors;

namespace BDika.Entities.Triggers
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

        [EntityField("lasttriggered", Hide = true)]
        public DateTime LastTriggered;

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
        public bool Enabled;

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
