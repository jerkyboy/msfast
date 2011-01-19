using System;
using System.Collections.Generic;
using System.Text;
using EYF.Dao.DB.Common;
using EYF.Entities;
using EYF.Entities.Dao;
using BDika.Providers.Tests.Browse;
using BDika.Entities.Tests;
using BDika.Entities.Triggers;
using Spring.Data.Common;
using System.Data;

namespace BDika.Dao.DB.Tests.Browse
{
    public abstract class BrowseTestsDBDAO<T> : GetObjectsDBDAO where T : BrowseTestsEntities
    {
    }

    public abstract class BrowseTests_NoCacheDBDAO<T> : DBEntityAccessObject<T> where T : BrowseTestsEntities_NoCache
    {
        private static EntitiesCollectionMapper<T> entityMapper = new EntitiesCollectionMapper<T>();

        public static String SELECT = " SELECT DISTINCT " + Entity.GetFieldName(typeof(Test), "testid") + " as tests_testid, 'BPI' as tests_bpi ";
        public static String COUNT = " SELECT COUNT(" + Entity.GetFieldName(typeof(Test), "testid") + ") ";
        public static String FROM = " FROM " + Entity.GetTableNameAndNick(typeof(Test));
        public static String WHERE = "";
        public static String ORDERBY = " ORDER BY " + Entity.GetFieldName(typeof(Test), "testid") + " DESC ";
        public static String LIMIT = " LIMIT ?ind, ?len ";

        public override IDAOTransaction ExecuteCall(EntitiesDAOTransaction<T> t)
        {
            foreach (IEntityIdentifier id in t.EntitiesIdentities)
            {
                IDbParametersBuilder builder = CreateDbParametersBuilder();

                T bpe = (T)id;

                PrepareBuilder(builder, bpe);

                String sql = String.Concat(GetSelect(bpe), GetFrom(bpe), GetWhere(bpe), GetOrder(bpe), GetLimit(bpe));

                t.Entities = AdoTemplate.QueryWithResultSetExtractor(CommandType.Text, sql, entityMapper, builder.GetParameters());

                t.Succeeded = (t.Entities != null);

                if (t.Succeeded && bpe.PopTotal && t.Entities != null && t.Entities.ContainsKey(id))
                {
                    if (bpe.PopTotal)
                    {
                        t.Entities[id].Total = int.Parse(AdoTemplate.ExecuteScalar(CommandType.Text, String.Concat(GetCount(bpe), GetFrom(bpe), GetWhere(bpe)), builder.GetParameters()).ToString());
                    }
                }

                return t;
            }
            return null;
        }

        public virtual void PrepareBuilder(IDbParametersBuilder builder, T bpe)
        {
            builder.Create().Name("len").Type(DbType.UInt32).Value(bpe.ResultsPerPage);
            builder.Create().Name("ind").Type(DbType.UInt32).Value(bpe.Index);
        }
        public virtual String GetCount(T bpe) { return COUNT; }
        public virtual String GetLimit(T bpe) { return LIMIT; }
        public virtual String GetOrder(T bpe) { return ORDERBY; }
        public virtual String GetWhere(T bpe) { return WHERE; }
        public virtual String GetFrom(T bpe) { return FROM; }
        public virtual String GetSelect(T bpe) { return SELECT; }
    }



    public class BrowseTests_AllDBDAO : BrowseTests_NoCacheDBDAO<BrowseTests_All>
    {

    }

    public class BrowseTests_ForTriggerDBDAO : BrowseTestsDBDAO<BrowseTests_ForTrigger>
    {
        public static String SELECT = "SELECT DISTINCT " +
                                      Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "triggerid") + " as tests_triggerid, " +
                                      Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testid") + " as tests_testid " +
                                      " FROM " + Entity.GetTableNameAndNick(typeof(TriggerToTestAndTesterType));

        public static String WHERE = " WHERE " + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "triggerid") + " IN ({0}) " +
                                     " ORDER BY " + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testid") + " DESC LIMIT 0, 512";

        public override String GetSelect<T>(EntitiesDAOTransaction<T> t, String where)
        {
            return SELECT + WHERE;
        }

        public override String GetWhere<T>(EntitiesDAOTransaction<T> t)
        {
            return null;
        }
    }

}