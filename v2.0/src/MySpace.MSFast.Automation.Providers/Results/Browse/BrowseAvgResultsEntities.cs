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
using MySpace.MSFast.Automation.Entities.Results;
using EYF.Providers.Common;
using EYF.Entities;
using EYF.Providers.Gateway;
using EYF.Entities.StandardObjects;

namespace MySpace.MSFast.Automation.Providers.Results.Browse
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
