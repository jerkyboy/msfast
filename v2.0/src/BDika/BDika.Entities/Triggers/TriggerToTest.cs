using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDika.Entities.Tests;
using EYF.Entities;

namespace BDika.Entities.Triggers
{
    [Serializable]
    public sealed class TriggerToTestID : EntityIdentifier<TriggerToTestID>
    {
        private string _id;
        
        public TriggerID TriggerID;
        public TestID TestID;

        public TriggerToTestID(TriggerID TriggerID, TestID TestID)
        {
            this.TestID = TestID;
            this.TriggerID = TriggerID;
            this._id = String.Concat((uint)TriggerID, "_", (uint)TestID);
        }

        public TriggerToTestID(string _id)
        {
            this._id = _id;
        }

        public static implicit operator TriggerToTestID(string i) { return new TriggerToTestID(i); }
        public static implicit operator string(TriggerToTestID i) { return (i != null ? i._id : null); }

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

}
