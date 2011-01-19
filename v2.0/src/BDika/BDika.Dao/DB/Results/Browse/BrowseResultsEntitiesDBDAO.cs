using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.Dao;
using BDika.Providers.Results.Browse;
using Spring.Data.Common;
using System.Data;
using EYF.Entities;
using BDika.Entities.Results;
using BDika.Entities.Tests;
using BDika.Entities.Triggers;

namespace BDika.Dao.DB.Results.Browse
{
    public class BrowseResultsEntities_NoCacheDBDAO<T> : DBEntityAccessObject<T> where T : BrowseResultsEntities
    {
        private static EntitiesCollectionMapper<T> entityMapper = new EntitiesCollectionMapper<T>();

        public static String COUNT = "SELECT COUNT(DISTINCT " + Entity.GetFieldName(typeof(Entities.Results.Results), "resultsid") + ") ";
        public static String SELECT = "SELECT DISTINCT " + Entity.GetFieldName(typeof(Entities.Results.Results), "resultsid") + " as results_resultsid, 'BPI' as results_bpi ";

        public static String FROM = " FROM " + Entity.GetTableNameAndNick(typeof(Entities.Results.Results));
        public static String FROM_TRIGGER = " FROM " + Entity.GetTableNameAndNick(typeof(Entities.Results.Results)) + ", " +
                                            Entity.GetTableNameAndNick(typeof(TriggerToTestAndTesterType));

        public static String WHERE_SUCCEEDED = " (" + Entity.GetFieldName(typeof(Entities.Results.Results), "state") + " = " + ((uint)ResultsState.Succeeded) + ") ";
        public static String WHERE = " WHERE " + WHERE_SUCCEEDED;
        public static String ORDERBY = " ORDER BY " + Entity.GetFieldName(typeof(Entities.Results.Results), "resultsid") + " DESC ";
        public static String LIMIT = " LIMIT ?ind,?len ";

        public override IDAOTransaction ExecuteCall(EntitiesDAOTransaction<T> t)
        {
            foreach (IEntityIdentifier id in t.EntitiesIdentities)
            {
                IDbParametersBuilder builder = CreateDbParametersBuilder();

                T bpe = (T)id;

                PrepareBuilder(builder, bpe);

                builder.Create().Name("len").Type(DbType.UInt32).Value(bpe.ResultsPerPage);
                builder.Create().Name("ind").Type(DbType.UInt32).Value(bpe.Index);

                String sql = String.Concat(GetSelect(builder, bpe), GetFrom(builder, bpe), GetWhere(builder, bpe), GetOrder(builder, bpe), GetLimit(builder, bpe));
                t.Entities = AdoTemplate.QueryWithResultSetExtractor(CommandType.Text, sql, entityMapper, builder.GetParameters());
                t.Succeeded = (t.Entities != null);

                if (t.Succeeded && bpe.PopTotal && t.Entities != null && t.Entities.ContainsKey(id))
                {
                    if (bpe.PopTotal)
                    {
                        String s = String.Concat(GetCount(builder, bpe), GetFrom(builder, bpe), GetWhere(builder, bpe));
                        t.Entities[id].Total = int.Parse(AdoTemplate.ExecuteScalar(CommandType.Text, s, builder.GetParameters()).ToString());
                    }
                }

                return t;
            }
            return null;
        }

        public virtual void PrepareBuilder(IDbParametersBuilder builder, T bpe){}
        public virtual String GetCount(IDbParametersBuilder builder, T bpe){return COUNT;}
        public virtual String GetLimit(IDbParametersBuilder builder, T bpe) { return LIMIT; }
        public virtual String GetOrder(IDbParametersBuilder builder, T bpe) { return ORDERBY; }
        public virtual String GetWhere(IDbParametersBuilder builder, T bpe) { return WHERE; }
        public virtual String GetFrom(IDbParametersBuilder builder, T bpe) { return FROM; }
        public virtual String GetSelect(IDbParametersBuilder builder, T bpe) { return SELECT; }
    }

    public class BrowseResultsEntities_FreeBrowseDBDAO : BrowseResultsEntities_NoCacheDBDAO<BrowseResultsEntities_FreeBrowse>
    {
        private static String WHERE_TEST_ID = " ( " + Entity.GetFieldName(typeof(Entities.Results.Results), "testid") + " = ?tid ) ";
        private static String WHERE_TESTER_TYPE_ID = " ( " + Entity.GetFieldName(typeof(Entities.Results.Results), "testertypeid") + " = ?ttid ) ";
        private static String WHERE_RESULTS_STATE = " ( " + Entity.GetFieldName(typeof(Entities.Results.Results), "state") + " = ?ress ) ";
        private static String WHERE_TRIGGER_ID = " ( " + 
            Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "triggerid") + " = ?trid AND " +
            Entity.GetFieldName(typeof(Entities.Results.Results), "testid") + " = " + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testid") + " AND " +
            Entity.GetFieldName(typeof(Entities.Results.Results), "testertypeid") + " = " + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testertypeid") + " ) ";
        
        public override void PrepareBuilder(IDbParametersBuilder builder, BrowseResultsEntities_FreeBrowse bpe)
        {
            base.PrepareBuilder(builder, bpe);

            if (bpe.ResultsState != ResultsState.Unknown)
                builder.Create().Name("ress").Type(DbType.UInt32).Value((uint)bpe.ResultsState);

            if(TriggerID.IsValidTriggerID(bpe.TriggerID)) 
                builder.Create().Name("trid").Type(DbType.UInt32).Value((uint)bpe.TriggerID);

            if (TestID.IsValidTestID(bpe.TestID))
                builder.Create().Name("tid").Type(DbType.UInt32).Value((uint)bpe.TestID);

            if (TesterTypeID.IsValidTesterTypeID(bpe.TesterTypeID))
                builder.Create().Name("ttid").Type(DbType.UInt32).Value((uint)bpe.TesterTypeID);
        }

        public override string GetFrom(IDbParametersBuilder builder, BrowseResultsEntities_FreeBrowse bpe)
        {
            if (TriggerID.IsValidTriggerID(bpe.TriggerID))
                return FROM_TRIGGER;

            return base.GetFrom(builder, bpe);
        }

        public override String GetWhere(IDbParametersBuilder builder, BrowseResultsEntities_FreeBrowse bpe)
        {
            String where = String.Empty;

            if (bpe.ResultsState != ResultsState.Unknown) {
                if (where.Length > 0) where += " AND ";
                where += WHERE_RESULTS_STATE;
            }

            if (TestID.IsValidTestID(bpe.TestID)) {
                if (where.Length > 0) where += " AND "; 
                where += WHERE_TEST_ID;                
            }

            if (TesterTypeID.IsValidTesterTypeID(bpe.TesterTypeID)) {
                if (where.Length > 0) where += " AND ";
                where += WHERE_TESTER_TYPE_ID;                
            }

            if (TriggerID.IsValidTriggerID(bpe.TriggerID))
            {
                if (where.Length > 0) where += " AND ";
                where += WHERE_TRIGGER_ID;                
            }

            if (String.IsNullOrEmpty(where)) 
                return WHERE;
            
            return " WHERE " + where; 
        }
    }
}
