//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Entities)
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
using EYF.Core.Exceptions;
using EYF.Entities;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Entities.Collectors;

namespace MySpace.MSFast.Automation.Entities.Results
{
    #region ResultsID
    public class InvalidResultsIDException : EYFException { }

    [Serializable]
    public class ResultsID : EntityIdentifier<ResultsID>
    {
        public static bool ValidateResultsID(ResultsID ResultsID)
        {
            if (IsValidResultsID(ResultsID) == false) throw new InvalidResultsIDException();
            return true;
        }
        public static bool IsValidResultsID(ResultsID ResultsID)
        {
            return ResultsID != 0;
        }

        private uint _id;

        public ResultsID(uint _id)
        {
            this._id = _id;
        }

        public static implicit operator ResultsID(uint i) { return new ResultsID(i); }
        public static implicit operator uint(ResultsID i) { return (i != null ? i._id : 0); }

        public override string ToString() { return UniqueIdetifier; }

        public override String UniqueIdetifier
        {
            get
            {
                return this._id.ToString();
            }
        }
        public override object ColumnValue
        {
            get
            {
                return this._id;
            }
        }
        public override int GetHashCode() { return (int)_id; }
    }
    #endregion

    public enum ResultsState : uint
    {
        Unknown = 0,
        Pending = 1,
        Testing = 2,
        Processing = 3,
        Failed = 4,
        Succeeded = 5
    }

    [Serializable]
    [CachableEntity("Results")]
    [DBEntity("results")]
    public class Results : ConfigurableEntity
    {
        [EntityIdentity("resultsid")]
        public ResultsID ResultsID;

        [EntityField("testid")]
        public TestID TestID;

        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;

        [EntityField("state")]
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

        [EntityField("created")]
        public DateTime CreatedOn = EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime();

        [EntityField("saved")]
        public DateTime ExecutedOn = new DateTime(1971,1,1);

        [NonSerialized]
        [DelayedEntityReferenceField("testid")]
        public Test Test;

        [NonSerialized]
        [DelayedEntityReferenceField("testertypeid")]
        public TesterType TesterType;

        [EntityField("starttime")]
        public ulong StartTime = 0;

        [EntityField("endtime")]
        public ulong EndTime = 0;

        [EntityField("totalrendertime")]
        public uint RenderTime = 0;        

        [EntityField("firstrequesttime")]
        public uint FirstRequestTime = 0;

        [EntityField("totaldownloadscount")]
        public uint TotalDownloadsCount = 0;

        [EntityField("totaljsdownloadscount")]
        public uint TotalJSDownloadsCount = 0;

        [EntityField("totalcssdownloadscount")]
        public uint TotalCSSDownloadsCount = 0;

        [EntityField("totalimagesdownloadscount")]
        public uint TotalImagesDownloadsCount = 0;

        [EntityField("totaldownloadsize")]
        public uint TotalDownloadSize = 0;

        [EntityField("totaljsdownloadsize")]
        public uint TotalJSDownloadSize = 0;

        [EntityField("totalcssdownloadsize")]
        public uint TotalCSSDownloadSize = 0;

        [EntityField("totalimagesdownloadsize")]
        public uint TotalImagesDownloadSize = 0;

        [EntityField("processortimeavg")]
        public uint ProcessorTimeAvg = 0;

        [EntityField("usertimeavg")]
        public uint UserTimeAvg = 0;

        [EntityField("privateworkingsetdelta")]
        public uint PrivateWorkingSetDelta = 0;

        [EntityField("workingsetdelta")]
        public uint WorkingSetDelta = 0;
    }

    [Serializable]
    [DBEntity("results")]
    public class PendingResultsEntityIndex : Entity
    {
        [EntityIdentity("testertypeid")]
        public TesterTypeID TesterTypeID;

        [EntityField("resultsid")]
        public ResultsID ResultsID;

        [NonSerialized]
        [DelayedEntityReferenceField("resultsid")]
        public Results Results;
    }

    [EntityCommand(EntityCommandType.Update, typeof(Results))]
    [DBEntity("results")]
    public class UpdateResultsStateEntityCommand : Entity
    {
        [EntityIdentity("resultsid")]
        public ResultsID ResultsID;

        [EntityField("saved")]
        public DateTime ExecutedOn = EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime();

        [EntityField("state")]
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

        [EntityField("starttime")]
        public ulong StartTime = 0;

        [EntityField("endtime")]
        public ulong EndTime = 0;

        [EntityField("totalrendertime")]
        public uint RenderTime = 0;
        
        [EntityField("firstrequesttime")]
        public uint FirstRequestTime = 0;
        
        [EntityField("totaldownloadscount")]
        public uint TotalDownloadsCount = 0;

        [EntityField("totaljsdownloadscount")]
        public uint TotalJSDownloadsCount = 0;

        [EntityField("totalcssdownloadscount")]
        public uint TotalCSSDownloadsCount = 0;

        [EntityField("totalimagesdownloadscount")]
        public uint TotalImagesDownloadsCount = 0;

        [EntityField("totaldownloadsize")]
        public uint TotalDownloadSize = 0;

        [EntityField("totaljsdownloadsize")]
        public uint TotalJSDownloadSize = 0;

        [EntityField("totalcssdownloadsize")]
        public uint TotalCSSDownloadSize = 0;

        [EntityField("totalimagesdownloadsize")]
        public uint TotalImagesDownloadSize = 0;

        [EntityField("processortimeavg")]
        public uint ProcessorTimeAvg = 0;

        [EntityField("usertimeavg")]
        public uint UserTimeAvg = 0;

        [EntityField("privateworkingsetdelta")]
        public uint PrivateWorkingSetDelta = 0;

        [EntityField("workingsetdelta")]
        public uint WorkingSetDelta = 0;
    }
}
