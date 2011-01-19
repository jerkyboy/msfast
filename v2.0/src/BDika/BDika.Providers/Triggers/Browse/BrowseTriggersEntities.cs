using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities;
using EYF.Providers.Common;
using BDika.Entities.Triggers;

namespace BDika.Providers.Triggers.Browse
{
    public enum BrowseTriggersEntitiesTypes : uint
    {
        BrowseTriggers_FreeBrowse = 1
    }

    [Serializable]
    public abstract class BrowseTriggersEntities : BrowseEntitiesIDs<TriggerID, Trigger>
    {
        [EntityField("triggerid")]
        public List<TriggerID> TriggerIDs;

        public BrowseTriggersEntities()
        {
            base.ResultsPerPage = 20;
        }
        public override List<TriggerID> GetEntitiesIDs()
        {
            return this.TriggerIDs;
        }
    }

    [Serializable]
    public class BrowseTriggersEntities_NoCache : BrowseTriggersEntities, IEntityIdentifier, IComparable, IEquatable<IEntityIdentifier>, IComparable<IEntityIdentifier>
    {
        [EntityIdentity("bpi")]
        public BrowseTriggersEntities_NoCache BrowseProperties;

        public new IEntityIdentifier Identity
        {
            get
            {
                return this;
            }
        }

        public BrowseTriggersEntities_NoCache() : base() { this.BrowseProperties = this; }
        public BrowseTriggersEntities_NoCache(String v) : this() { }

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
    [DBEntity("triggers")]
    [EntitiesBrowser(ID = (uint)BrowseTriggersEntitiesTypes.BrowseTriggers_FreeBrowse)]
    public class BrowseTriggersEntities_FreeBrowse : BrowseTriggersEntities_NoCache
    {
        [BrowseArgument("e")]
        public int Enabled = -1;

        [BrowseArgument("n")]
        public String Name;

        [BrowseArgument("t")]
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
    }
}
