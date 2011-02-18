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
using EYF.Providers.Common;
using MySpace.MSFast.Automation.Entities.Tests;
using EYF.Entities;
using MySpace.MSFast.Automation.Entities.Triggers;
using System.Collections;

namespace MySpace.MSFast.Automation.Providers.Tests.Browse
{
    public enum BrowseTestsEntitiesTypes : uint
    {
        BrowseTests_All = 1,
        BrowseTests_ForTriggerAndTesterType = 2
    }

    [Serializable]
    public abstract class BrowseTestsEntities : BrowseEntitiesIDs<TestID, Test>
    {
        [EntityField("testid")]
        public List<TestID> TestIDs;

        public BrowseTestsEntities()
        {
            base.ResultsPerPage = 50;
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

        [BrowseArgument("trid")]
        [EntityField("triggerid")]
        public TriggerID TriggerID;

        [BrowseArgument("ttid")]
        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;

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
    [DBEntity("tests")]
    [EntitiesBrowser(ID = (uint)BrowseTestsEntitiesTypes.BrowseTests_ForTriggerAndTesterType)]
    public class BrowseTests_ForTriggerAndTesterType : BrowseTestsEntities_NoCache
    {
        [EntityField("selected")]
        public List<TestID> SelectedTests;

        public BrowseTests_ForTriggerAndTesterType()
        {
            base.ResultsPerPage = 4*1024;
        }
        public override BrowseEntitiesIDs<TestID, Test> LoadBrowseEntity(EYF.Entities.StandardObjects.IUser requestor)
        {
            BrowseTests_ForTriggerAndTesterType bpe = base.LoadBrowseEntity(requestor) as BrowseTests_ForTriggerAndTesterType;
            if (bpe != null)
                this.SelectedTests = bpe.SelectedTests;
            
            return bpe;
        }
    }
}
