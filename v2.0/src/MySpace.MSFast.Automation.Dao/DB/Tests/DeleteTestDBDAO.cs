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
using MySpace.MSFast.Automation.Entities.Tests;
using System.Data;

namespace MySpace.MSFast.Automation.Dao.DB.Tests
{
    public class DeleteTestDBDAO : DBEntityAccessObject<Test>
    {
        private static String DELETE = " DELETE FROM tests WHERE testid IN ({0}); ";

        public override IDAOTransaction ExecuteCall(EntitiesDAOTransaction<Test> w)
        {
            StringBuilder testids = new StringBuilder();

            int cnt = 0;

            foreach (Test t in w.Entities.Values)
            {
                if (t != null && TestID.IsValidTestID(t.TestID))
                {
                    if (testids.Length > 0) testids.Append(',');
                    testids.Append(t.TestID.ColumnValue.ToString());

                    cnt++;
                }
            }

            if (testids.Length == 0)
            {
                w.Succeeded = true;
                return w;
            }

            w.Succeeded = AdoTemplate.ExecuteNonQuery(CommandType.Text, String.Format(DELETE, testids.ToString())) == cnt;

            return w;
        }
    }
}
