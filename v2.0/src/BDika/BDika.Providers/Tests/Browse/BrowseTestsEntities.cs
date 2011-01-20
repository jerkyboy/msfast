using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Providers.Common;
using BDika.Entities.Tests;
using EYF.Entities;
using BDika.Entities.Triggers;

namespace BDika.Providers.Tests.Browse
{
    public enum BrowseTestsEntitiesTypes : uint
    {
        BrowseTests_All = 1,
        BrowseTests_ForTrigger = 2
    }

    [Serializable]
    public abstract class BrowseTestsEntities : BrowseEntitiesIDs<TestID, Test>
    {
        [EntityField("testid")]
        public List<TestID> TestIDs;

        public BrowseTestsEntities()
        {
            base.ResultsPerPage = 20;
        }
        public override List<TestID> GetEntitiesIDs()
        {
            return this.TestIDs;
        }
    }

    [Serializable]
    public class BrowseTestsEntities_NoCache : BrowseTestsEntities, IEntityIdentifier, IComparable, IEquatable<IEntityIdentifier>, IComparable<IEntityIdentifier>
    {
        [EntityIdentity("bpi")]
        public BrowseTestsEntities_NoCache BrowseProperties;

        public new IEntityIdentifier Identity
        {
            get
            {
                return this;
            }
        }

        public BrowseTestsEntities_NoCache() :base() { this.BrowseProperties = this; }
        public BrowseTestsEntities_NoCache(String v) : this() { }           

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
    [DBEntity("tests")]
    [EntitiesBrowser(ID = (uint)BrowseTestsEntitiesTypes.BrowseTests_All)]
    public class BrowseTests_All : BrowseTestsEntities_NoCache{}

    [Serializable]
    [CachableEntity("BrowseTests_ForTrigger", TTL = 20 * 60)]
    [DBEntity("tests")]
    [EntitiesBrowser(ID = (uint)BrowseTestsEntitiesTypes.BrowseTests_ForTrigger)]
    public class BrowseTests_ForTrigger : BrowseTestsEntities
    {
        [BrowseArgument("t")]
        [EntityIdentity("triggerid")]
        public TriggerID TriggerID;
    }
}
