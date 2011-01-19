using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.Dao;
using BDika.Entities.Collectors;
using EYF.Entities;
using Spring.Data.Common;
using BDika.Entities.Triggers;
using BDika.Providers.Collectors.Browse;
using System.Data;
using BDika.Entities.Results;

namespace BDika.Dao.DB.Collectors
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