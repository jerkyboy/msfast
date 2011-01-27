using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDika.Entities.Results;
using EYF.Providers.Common;
using EYF.Entities;
using EYF.Providers.Gateway;
using EYF.Entities.StandardObjects;

namespace BDika.Providers.Results.Browse
{
    [Serializable]
    [DBEntity("results")]
    public class BrowseAvgResultsEntities : BrowseEntities<AvgResults>, IEntityIdentifier, IComparable, IEquatable<IEntityIdentifier>, IComparable<IEntityIdentifier>
    {
        public enum OrderBy
        {
            ClientTime,
            ServerTime
        }

        [EntityIdentity("bpi")]
        public BrowseAvgResultsEntities BrowseProperties;

        public uint LastHours = 0;
        public OrderBy Order = OrderBy.ClientTime;

        public new IEntityIdentifier Identity
        {
            get
            {
                return this;
            }
        }

        public BrowseAvgResultsEntities() : base() { this.BrowseProperties = this; this.ResultsPerPage = 20; }
        public BrowseAvgResultsEntities(String v) : this() { }

        public override void Load(IUser requestor)
        {
            base.Load(requestor);
            
            if(this.Data != null && this.Data.Count > 0)
                DelayedEntitiesLoader.LoadDelayedEntities<AvgResults>(new EntitiesCollection<AvgResults>(this.Data));
        }

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
