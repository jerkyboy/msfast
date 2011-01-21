using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities;
using EYF.Providers.Common;
using BDika.Entities.Results;
using BDika.Entities.Tests;
using BDika.Entities.Triggers;

namespace BDika.Providers.Results.Browse
{
    public enum BrowseResultsEntitiesTypes : uint
    {
        BrowseResults_FreeBrowse = 1
    }

    [Serializable]
    public abstract class BrowseResultsEntities : BrowseEntitiesIDs<ResultsID, Entities.Results.Results>
    {
        [EntityField("resultsid")]
        public List<ResultsID> ResultsIDs;

        public BrowseResultsEntities()
        {
#if DEBUG
            base.ResultsPerPage = 5;
#else 
            base.ResultsPerPage = 100;
#endif

        }
        public override List<ResultsID> GetEntitiesIDs()
        {
            return this.ResultsIDs;
        }
    }

    [Serializable]
    public class BrowseResultsEntities_NoCache : BrowseResultsEntities, IEntityIdentifier, IComparable, IEquatable<IEntityIdentifier>, IComparable<IEntityIdentifier>
    {
        [EntityIdentity("bpi")]
        public BrowseResultsEntities_NoCache BrowseProperties;

        public new IEntityIdentifier Identity
        {
            get
            {
                return this;
            }
        }

        public BrowseResultsEntities_NoCache():base() { this.BrowseProperties = this; }
        public BrowseResultsEntities_NoCache(String v) : this() { }

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
    [DBEntity("results")]
    [EntitiesBrowser(ID = (uint)BrowseResultsEntitiesTypes.BrowseResults_FreeBrowse)]
    public class BrowseResultsEntities_FreeBrowse : BrowseResultsEntities_NoCache
    {
        [BrowseArgument("tid")]
        public TestID TestID;

        [BrowseArgument("ttid")]
        public TesterTypeID TesterTypeID;

        [BrowseArgument("trid")]
        public TriggerID TriggerID;        
        
        [BrowseArgument("r")]
        public uint ResultsStateID;
        public ResultsState ResultsState
        {
            get
            {
                return (ResultsState)this.ResultsStateID;
            }
            set
            {
                this.ResultsStateID = (uint)value;
            }
        }
    
    }
}
