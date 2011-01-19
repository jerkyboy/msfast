using System;
using System.Collections.Generic;
using System.Text;
using EYF.Providers.Common;
using BDika.Entities.Tests;
using EYF.Entities;
using BDika.Entities.Triggers;

namespace BDika.Providers.Tests.Browse
{
    public enum BrowseTesterTypesEntitiesTypes : uint
    {
        BrowseTesterTypes_All = 1,
        BrowseTesterTypes_ForTest = 2,
        BrowseTesterTypes_ForTrigger = 3,
        BrowseTesterTypes_ForTestAndTrigger = 4
    }

    [Serializable]
    public abstract class BrowseTesterTypesEntities : BrowseEntitiesIDs<TesterTypeID, TesterType>
    {
        [EntityField("testertypeid")]
        public List<TesterTypeID> TesterTypeIDs;

        public BrowseTesterTypesEntities()
        {
            base.ResultsPerPage = 20;
        }
        public override List<TesterTypeID> GetEntitiesIDs()
        {
            return this.TesterTypeIDs;
        }
    }

    [Serializable]
    public class BrowseTesterTypesEntities_NoCache : BrowseTesterTypesEntities, IEntityIdentifier, IComparable, IEquatable<IEntityIdentifier>, IComparable<IEntityIdentifier>
    {
        [EntityIdentity("bpi")]
        public BrowseTesterTypesEntities_NoCache BrowseProperties;

        public new IEntityIdentifier Identity
        {
            get
            {
                return this;
            }
        }

        public BrowseTesterTypesEntities_NoCache() { this.BrowseProperties = this; this.ResultsPerPage = 20; }
        public BrowseTesterTypesEntities_NoCache(String v) : this() { }           

        #region IEntityIdentifier Members

        public object ColumnValue
        {
            get { return UniqueIdetifier; }
        }

        public string UniqueIdetifier
        {
            get { return "BPI"; }
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object other)
        {
            if (other == null) return 1;

            if (other is IEntityIdentifier)
                return this.CompareTo(((IEntityIdentifier)other).UniqueIdetifier);

            return -1;
        }

        #endregion

        #region IEquatable<IEntityIdentifier> Members

        public bool Equals(IEntityIdentifier other)
        {
            if (this.UniqueIdetifier == null || other == null || other.UniqueIdetifier == null)
                return false;

            return this.UniqueIdetifier.Equals(other.UniqueIdetifier);
        }

        #endregion

        #region IComparable<IEntityIdentifier> Members

        public int CompareTo(IEntityIdentifier other)
        {
            if (other == null) return 1;
            return this.UniqueIdetifier.CompareTo(other.UniqueIdetifier);
        }

        #endregion

        public override int GetHashCode()
        {
            return 1;
        }
    }

    [Serializable]
    [CachableEntity("BrowseTesterTypes_ForTest", TTL = 2 * 60 * 60)]
    [DBEntity("testertypes")]
    [EntitiesBrowser(ID = (uint)BrowseTesterTypesEntitiesTypes.BrowseTesterTypes_ForTest)]
    public class BrowseTesterTypes_ForTest : BrowseTesterTypesEntities
    {
        [BrowseArgument("t")]
        [EntityIdentity("testid", Hide = true)]
        public TestID TestID;
    }

    [Serializable]
    /*[CachableEntity("BrowseTesterTypes_ForTestAndTrigger", TTL = 2 * 60 * 60)]*/
    [DBEntity("triggertotestandtestertype")]
    [EntitiesBrowser(ID = (uint)BrowseTesterTypesEntitiesTypes.BrowseTesterTypes_ForTestAndTrigger)]
    public class BrowseTesterTypes_ForTestAndTrigger : BrowseTesterTypesEntities
    {
        [EntityAggregateFieldValue(AggregateType.Concat, "triggerid", "testid")]
        [BrowseArgument("id")]
        [EntityIdentity("id", Hide = true)]
        public TriggerToTestID TriggerToTestID;
    }

    [Serializable]
    [CachableEntity("BrowseTesterTypes_ForTrigger", TTL = 2 * 60 * 60)]
    [DBEntity("testertypes")]
    [EntitiesBrowser(ID = (uint)BrowseTesterTypesEntitiesTypes.BrowseTesterTypes_ForTrigger)]
    public class BrowseTesterTypes_ForTrigger : BrowseTesterTypesEntities
    {
        [BrowseArgument("t")]
        [EntityIdentity("triggerid", Hide = true)]
        public TriggerID TriggerID;
    }
    
    [Serializable]
    [CachableEntity("BrowseTesterTypes_All", TTL = 2 * 60 * 60)]
    [DBEntity("testertypes")]
    [EntitiesBrowser(ID = (uint)BrowseTesterTypesEntitiesTypes.BrowseTesterTypes_All)]
    public class BrowseTesterTypes_All : BrowseTesterTypesEntities_NoCache { }
}