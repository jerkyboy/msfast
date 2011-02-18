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
using MySpace.MSFast.Automation.Entities.Collectors;
using EYF.Entities;
using Spring.Data.Common;
using MySpace.MSFast.Automation.Entities.Triggers;
using MySpace.MSFast.Automation.Providers.Collectors.Browse;
using System.Data;
using MySpace.MSFast.Automation.Entities.Results;

namespace MySpace.MSFast.Automation.Dao.DB.Collectors
{
    public class GetPendingResultsCollectorsConfigurationDBDAO : AbsGetCollectorsConfigurationDBDAO<BrowseExtCollectorsConfigEntities>
    {
        public override IDAOTransaction ExecuteExtCall(EntitiesDAOTransaction<BrowseExtCollectorsConfigEntities> t)
        {
            foreach (IEntityIdentifier id in t.EntitiesIdentities)
            {
                BrowseExtCollectorsConfigEntities bpe = (BrowseExtCollectorsConfigEntities)id;

                EntitiesCollection<ExtCollectorsConfigEntity> cl = null;

                IDbParametersBuilder builder = CreateDbParametersBuilder();

                builder.Create().Name("len").Type(DbType.UInt32).Value(bpe.ResultsPerPage);
                builder.Create().Name("ind").Type(DbType.UInt32).Value(bpe.Index);

                String sql = SELECT_TRIGGERID_TESTID_TESTERTYPEID;
                String from = SELECT_TRIGGERID_TESTID_TESTERTYPEID_FROM;
                String where = SELECT_TRIGGERID_TESTID_TESTERTYPEID_WHERE + " AND NOT EXISTS (SELECT state FROM results WHERE results.testid = triggertotestandtestertype.testid AND results.testertypeid = triggertotestandtestertype.testertypeid AND results.state = " + ((uint)ResultsState.Pending) + ") AND triggers.enabled = 1 ";

                if (TriggerID.IsValidTriggerID(bpe.TriggerID))
                {
                    where += " AND triggers.triggerid = ?triggerid ";
                    builder.Create().Name("triggerid").Type(DbType.UInt32).Value(bpe.TriggerID.ColumnValue);
                }
                else if (bpe.TriggerAllTimedbased)
                {
                    where += " AND triggers.triggertype = " + ((uint)TriggerType.Time) + " AND TIMESTAMPDIFF(MINUTE, triggers.lasttriggered, NOW()) >= triggers.timeout ";
                }

                String s = sql + from + where + " LIMIT ?ind,?len ";

                cl = AdoTemplate.QueryWithResultSetExtractor(CommandType.Text, s, entityMapper, builder.GetParameters());

                if(cl !=null)
                {
                    bpe.Data = new List<ExtCollectorsConfigEntity>(cl.Values);
                    
                    if (t.Entities == null){
                        t.Entities = new EntitiesCollection<BrowseExtCollectorsConfigEntities>();
                    } 

                    t.Entities.Add(id, bpe);
                }
                
                t.Succeeded = (t.Entities != null);

                return t;
            }
            return null;
        }
    }
}