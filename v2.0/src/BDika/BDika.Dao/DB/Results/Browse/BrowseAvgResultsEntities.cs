using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.Dao;
using EYF.Entities;
using Spring.Data.Common;
using System.Data;
using BDika.Entities.Results;
using BDika.Providers.Results.Browse;

namespace BDika.Dao.DB.Results.Browse
{
    public abstract class AbsBrowseAvgResultsEntities<T> : DBEntityAccessObject<T> where T : EYF.Entities.IEntity
    {
        public static EntitiesCollectionMapper<AvgResults> entityMapper = new EntitiesCollectionMapper<AvgResults>();

        public static String SELECT = " SELECT 'BPI' as results_bpi, max(resultsid) as results_resultsid, testid as results_testid, testertypeid as results_testertypeid, " + 
                                     " avg(totaldownloadscount) as results_totaldownloadscount, avg(totaljsdownloadscount) as results_totaljsdownloadscount, " + 
                                     " avg(totalcssdownloadscount) as results_totalcssdownloadscount, avg(totalimagesdownloadscount) as results_totalimagesdownloadscount, " + 
                                     " avg(totaldownloadsize) as results_totaldownloadsize, avg(totaljsdownloadsize) as results_totaljsdownloadsize, avg(totalcssdownloadsize) " + 
                                     " as results_totalcssdownloadsize, avg(totalimagesdownloadsize) as results_totalimagesdownloadsize, avg(processortimeavg) as results_processortimeavg, " + 
                                     " avg(usertimeavg) as results_usertimeavg, avg(privateworkingsetdelta) as results_privateworkingsetdelta, " + 
                                     " avg(workingsetdelta) as results_workingsetdelta, avg(firstrequesttime) as results_firstrequesttime, avg(totalrendertime) as results_totalrendertime ";

        public static String FROM = " FROM results r ";
        public static String WHERE = " WHERE state = " + (uint)ResultsState.Succeeded;
        public static String WHERE_PAST_HOURS = " AND TIMESTAMPDIFF(HOUR, created, NOW()) <= ?hours ";
        public static String GROUP = " GROUP BY testid, testertypeid ";
        public static String ORDER_BY_CLIENT_TIME = " ORDER BY results_totalrendertime DESC ";
        public static String ORDER_BY_SERVER_TIME = " ORDER BY results_firstrequesttime DESC ";
        
        public static String LIMIT = " LIMIT ?ind,?len ";


        public override IDAOTransaction ExecuteCall(EntitiesDAOTransaction<T> w)
        {
            return ExecuteExtCall(w);
        }
        public abstract IDAOTransaction ExecuteExtCall(EntitiesDAOTransaction<T> w);
    }




    public class GetLatestAvgResultsDBDAO : AbsBrowseAvgResultsEntities<BrowseAvgResultsEntities>
    {
        public override IDAOTransaction ExecuteExtCall(EntitiesDAOTransaction<BrowseAvgResultsEntities> t)
        {
            foreach (IEntityIdentifier id in t.EntitiesIdentities)
            {
                BrowseAvgResultsEntities bpe = (BrowseAvgResultsEntities)id;

                EntitiesCollection<AvgResults> cl = null;

                IDbParametersBuilder builder = CreateDbParametersBuilder();

                String s = SELECT + FROM + WHERE + WHERE_PAST_HOURS + GROUP;
                
                if (bpe.Order == BrowseAvgResultsEntities.OrderBy.ClientTime)
                    s += ORDER_BY_CLIENT_TIME;
                else
                    s += ORDER_BY_SERVER_TIME;
                
                s += LIMIT;

                builder.Create().Name("len").Type(DbType.UInt32).Value(bpe.ResultsPerPage);
                builder.Create().Name("ind").Type(DbType.UInt32).Value(bpe.Index);
                builder.Create().Name("hours").Type(DbType.UInt32).Value((bpe.LastHours > 0) ? bpe.LastHours : 24);

                cl = AdoTemplate.QueryWithResultSetExtractor(CommandType.Text, s, entityMapper, builder.GetParameters());

                if (cl != null)
                {
                    bpe.Data = new List<AvgResults>(cl.Values);

                    if (t.Entities == null)
                    {
                        t.Entities = new EntitiesCollection<BrowseAvgResultsEntities>();
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


