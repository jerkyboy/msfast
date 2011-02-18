//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.Providers)
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
using EYF.Providers.Common;
using MySpace.MSFast.Automation.Entities.Results;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Entities.Triggers;

namespace MySpace.MSFast.Automation.Providers.Results.Browse
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
            base.ResultsPerPage = 50;
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
