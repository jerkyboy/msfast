using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Dao.DB.Common;
using EYF.Entities;
using EYF.Entities.Dao;
using BDika.Entities.Tests;
using BDika.Providers.Tests.Browse;
using Spring.Data.Common;
using System.Data;
using BDika.Entities.Triggers;

namespace BDika.Dao.DB.Tests.Browse
{
    public abstract class BrowseTesterTypesDBDAO<T> : GetObjectsDBDAO where T : BrowseTesterTypesEntities
    {
    }

    public abstract class BrowseTesterTypes_NoCacheDBDAO<T> : DBEntityAccessObject<T> where T : BrowseTesterTypesEntities_NoCache
    {
        private static EntitiesCollectionMapper<T> entityMapper = new EntitiesCollectionMapper<T>();

        public static String SELECT = " SELECT DISTINCT " + Entity.GetFieldName(typeof(TesterType), "testertypeid") + " as testertypes_testertypeid, 'BPI' as testertypes_bpi ";

        public static String COUNT = " SELECT COUNT(" + Entity.GetFieldName(typeof(TesterType), "testertypeid") + ") ";
        public static String FROM = " FROM " + Entity.GetTableNameAndNick(typeof(TesterType));
        public static String WHERE = "";
        public static String ORDERBY = " ORDER BY " + Entity.GetFieldName(typeof(TesterType), "testertypeid") + " DESC ";
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


    public class BrowseTesterTypes_ForTestAndTriggerDBDAO : BrowseTesterTypesDBDAO<BrowseTesterTypes_ForTestAndTrigger>
    {

        public static String SELECT = "SELECT CONVERT(CONCAT(" + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "triggerid") + ",'_'," + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testid") + ") USING utf8) as triggertotestandtestertype_id, " +
                                      Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "triggerid") + " as triggertotestandtestertype_triggerid, " +
                                      Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testertypeid") + " as triggertotestandtestertype_testertypeid, " +
                                      Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testid") + " as triggertotestandtestertype_testid " +
                                      " FROM " + Entity.GetTableNameAndNick(typeof(TriggerToTestAndTesterType));

        public static String WHERE = " WHERE " + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testid") + " in ({0}) AND " + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "triggerid") + " IN ({1}) " +
                                      " ORDER BY " + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testertypeid") + " DESC ";
        
        public override string GetIDs<T>(EntitiesDAOTransaction<T> t)
        {
            return null;
        }
        public override string GetWhere<T>(EntitiesDAOTransaction<T> t)
        {
            EntitiesDAOTransaction<BrowseTesterTypes_ForTestAndTrigger> et = t as EntitiesDAOTransaction<BrowseTesterTypes_ForTestAndTrigger>;
            
            if (et == null)
                return null;

            StringBuilder triggers = new StringBuilder();
            StringBuilder tests = new StringBuilder();
            
            foreach (IEntityIdentifier id in et.EntitiesIdentities)
            {
                if((id is TriggerToTestID) == false) continue;

                   TriggerToTestID ttti = id as TriggerToTestID;

                   if (TriggerID.IsValidTriggerID(ttti.TriggerID) && TestID.IsValidTestID(ttti.TestID))
                   {
                       triggers.Append(ttti.TriggerID.ColumnValue.ToString());
                       tests.Append(ttti.TestID.ColumnValue.ToString());
                   }
            }

            return String.Format(WHERE , tests.ToString() , triggers.ToString());
        }
        public override String GetSelect<T>(EntitiesDAOTransaction<T> t, String where)
        {
            return SELECT + where;
        }
    }
    
    public class BrowseTesterTypes_ForTestDBDAO : BrowseTesterTypesDBDAO<BrowseTesterTypes_ForTest>
    {
        public static String SELECT = "SELECT " +
                                      Entity.GetFieldName(typeof(TesterTypeToTest), "testid") + " as testertypes_testid, " +
                                      Entity.GetFieldName(typeof(TesterTypeToTest), "testertypeid") + " as testertypes_testertypeid " +
                                      
                                      " FROM " + Entity.GetTableNameAndNick(typeof(TesterTypeToTest)) + 
                                      " WHERE " + Entity.GetFieldName(typeof(TesterTypeToTest), "testid") + " in ({0}) " +
                                      " ORDER BY " + Entity.GetFieldName(typeof(TesterTypeToTest), "testid") + " DESC ";

        public override String GetSelect<T>(EntitiesDAOTransaction<T> t, String where)
        {
            return SELECT;
        }
    }

    public class BrowseTesterTypes_ForTriggerDBDAO : BrowseTesterTypesDBDAO<BrowseTesterTypes_ForTrigger>
    {
        public static String SELECT = "SELECT " +
                                      Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "triggerid") + " as testertypes_triggerid, " +
                                      Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testertypeid") + " as testertypes_testertypeid, " +
                                      Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "testid") + " as testertypes_testid " +
                                      " FROM " + Entity.GetTableNameAndNick(typeof(TriggerToTestAndTesterType)) +
                                      " WHERE " + Entity.GetFieldName(typeof(TriggerToTestAndTesterType), "triggerid") + " in ({0}) " +
                                      " ORDER BY testertypes_testertypeid DESC ";

        public override String GetSelect<T>(EntitiesDAOTransaction<T> t, String where)
        {
            return SELECT;
        }
    }
    

    public class BrowseTesterTypes_AllDBDAO : BrowseTesterTypes_NoCacheDBDAO<BrowseTesterTypes_All>
    {

    }
}