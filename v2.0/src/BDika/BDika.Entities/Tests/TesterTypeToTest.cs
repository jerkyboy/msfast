using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities;

namespace BDika.Entities.Tests
{
    [Serializable]
    public sealed class TesterTypeToTestID : EntityIdentifier<TesterTypeToTestID>
    {
        private string _id;

        public TesterTypeToTestID(TesterTypeID TesterTypeID, TestID TestID)
        {
            this._id = String.Concat((uint)TesterTypeID, "_", (uint)TestID);
        }

        public TesterTypeToTestID(string _id)
        {
            this._id = _id;
        }

        public static implicit operator TesterTypeToTestID(string i) { return new TesterTypeToTestID(i); }
        public static implicit operator string(TesterTypeToTestID i) { return (i != null ? i._id : null); }

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
    [DBEntity("testertypestotest")]
    public class TesterTypeToTest : Entity
    {
        [EntityAggregateFieldValue(AggregateType.Concat, "testertypeid", "testid")]
        [EntityIdentity("id")]
        public TesterTypeToTestID TesterTypeToTestID = new TesterTypeToTestID(0,0);

        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;
        
        [EntityField("testid")]
        public TestID TestID;
    }
    
}
