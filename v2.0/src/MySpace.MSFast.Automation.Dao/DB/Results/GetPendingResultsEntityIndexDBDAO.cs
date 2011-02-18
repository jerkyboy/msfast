//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Dao)
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
using EYF.Dao.DB.Common;
using EYF.Entities;
using MySpace.MSFast.Automation.Entities.Results;
using EYF.Entities.Dao;
using MySpace.MSFast.Automation.Entities.Tests;

namespace MySpace.MSFast.Automation.Dao.DB.Results
{
    public class GetPendingResultsEntityIndexDBDAO : GetObjectsDBDAO
    {
        public static String SELECT = "SELECT " + Entity.GetSelectFieldsList(typeof(PendingResultsEntityIndex)) +
                                        " FROM " + Entity.GetTableNameAndNick(typeof(PendingResultsEntityIndex)) + " WHERE " +
                                        Entity.GetFieldName(typeof(PendingResultsEntityIndex), "testertypeid") + " = {0} " +
                                        " AND " + Entity.GetFieldName(typeof(PendingResultsEntityIndex), "state") + " = " + ((uint)ResultsState.Pending) + " ORDER BY " + Entity.GetFieldName(typeof(PendingResultsEntityIndex), "created") + " ASC;";

        public override string GetIDs<T>(EntitiesDAOTransaction<T> t)
        {
            EntitiesDAOTransaction<PendingResultsEntityIndex> et = t as EntitiesDAOTransaction<PendingResultsEntityIndex>;

            if (et == null)
                return null;

            foreach (IEntityIdentifier id in et.EntitiesIdentities)
            {
                if ((id is TesterTypeID) == false) continue;

                TesterTypeID ttti = id as TesterTypeID;
                
                return id.ToString();
            }

            return null;
        }

        public override string GetWhere<T>(EntitiesDAOTransaction<T> t) {return null;}

        public override String GetSelect<T>(EntitiesDAOTransaction<T> t, String where)
        {
            return SELECT;
        }
        
    }
}
