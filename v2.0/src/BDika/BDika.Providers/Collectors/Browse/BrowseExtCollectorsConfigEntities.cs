using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Providers.Common;
using BDika.Entities.Results;
using EYF.Entities;
using BDika.Entities.Triggers;
using BDika.Entities.Collectors;

namespace BDika.Providers.Collectors.Browse
{
    [Serializable]
    [DBEntity("config")]
    public class BrowseExtCollectorsConfigEntities : BrowseEntities<ExtCollectorsConfigEntity>, IEntityIdentifier, IComparable, IEquatable<IEntityIdentifier>, IComparable<IEntityIdentifier>
    {
        [EntityIdentity("bpi")]
        public BrowseExtCollectorsConfigEntities BrowseProperties;

        public TriggerID TriggerID;
        public bool TriggerAllTimedbased = false;

        public new IEntityIdentifier Identity
        {
            get
            {
                return this;
            }
        }

        public BrowseExtCollectorsConfigEntities() : base() { this.BrowseProperties = this; this.ResultsPerPage = 20; }
        public BrowseExtCollectorsConfigEntities(String v) : this() { }

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
}
