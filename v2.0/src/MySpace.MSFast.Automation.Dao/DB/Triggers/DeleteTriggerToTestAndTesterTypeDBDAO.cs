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
using EYF.Entities.Dao;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Entities.Tests;
using System.Data;

namespace MySpace.MSFast.Automation.Dao.DB.Triggers
{
    public class DeleteTriggerToTestAndTesterTypeDBDAO : DBEntityAccessObject<TriggerToTestAndTesterType>
    {
        private static String DELETE = " DELETE FROM triggertotestandtestertype WHERE triggerid IN ({0}) AND testid IN ({1}) AND testertypeid IN ({2}); ";

        public override IDAOTransaction ExecuteCall(EntitiesDAOTransaction<TriggerToTestAndTesterType> w)
        {
            StringBuilder testids = new StringBuilder();
            StringBuilder testertypeids = new StringBuilder();
            StringBuilder triggerids = new StringBuilder();

            int cnt = 0;

            foreach (TriggerToTestAndTesterType t in w.Entities.Values)
            {
                if (t != null && TestID.IsValidTestID(t.TestID) && TesterTypeID.IsValidTesterTypeID(t.TesterTypeID) && TriggerID.IsValidTriggerID(t.TriggerID))
                {
                    if (testids.Length > 0) testids.Append(',');
                    testids.Append(t.TestID.ColumnValue.ToString());

                    if (testertypeids.Length > 0) testertypeids.Append(',');
                    testertypeids.Append(t.TesterTypeID.ColumnValue.ToString());

                    if (triggerids.Length > 0) triggerids.Append(',');
                    triggerids.Append(t.TriggerID.ColumnValue.ToString());

                    cnt++;
                }
            }

            if (testertypeids.Length == 0)
            {
                w.Succeeded = true;
                return w;
            }

            w.Succeeded = AdoTemplate.ExecuteNonQuery(CommandType.Text, String.Format(DELETE, triggerids.ToString(), testids.ToString(), testertypeids.ToString())) == cnt;

            return w;
        }
    }
}
