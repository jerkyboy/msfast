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
using System.Text;
using EYF.Providers.Common;
using MySpace.MSFast.Automation.Entities.Tests;
using EYF.Entities;
using MySpace.MSFast.Automation.Entities.Triggers;

namespace MySpace.MSFast.Automation.Providers.Tests.Browse
{
    public enum BrowseTesterTypesEntitiesTypes : uint
    {
        BrowseTesterTypes_All = 1
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
    [DBEntity("testertypes")]
    [EntitiesBrowser(ID = (uint)BrowseTesterTypesEntitiesTypes.BrowseTesterTypes_All)]
    public class BrowseTesterTypes_All : BrowseTesterTypesEntities_NoCache { }
}