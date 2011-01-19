using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.Dao;
using BDika.Providers.Triggers.Browse;
using EYF.Entities;
using BDika.Entities.Triggers;
using Spring.Data.Common;
using System.Data;

namespace BDika.Dao.DB.Triggers.Browse
{
    public class BrowseTriggersEntities_NoCacheDBDAO<T> : DBEntityAccessObject<T> where T : BrowseTriggersEntities
    {
        private static EntitiesCollectionMapper<T> entityMapper = new EntitiesCollectionMapper<T>();

        public static String COUNT = "SELECT COUNT(DISTINCT " + Entity.GetFieldName(typeof(Trigger), "triggerid") + ") ";
        public static String SELECT = "SELECT DISTINCT " + Entity.GetFieldName(typeof(Trigger), "triggerid") + " as triggers_triggerid, 'BPI' as triggers_bpi ";
        public static String FROM = " FROM " + Entity.GetTableNameAndNick(typeof(Trigger));
        public static String WHERE = String.Empty;//" WHERE " + WHERE_ENABLED;
        public static String ORDERBY = " ORDER BY " + Entity.GetFieldName(typeof(Trigger), "triggerid") + " DESC ";
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

    public class BrowseTriggersEntities_FreeBrowseDBDAO : BrowseTriggersEntities_NoCacheDBDAO<BrowseTriggersEntities_FreeBrowse>
    {
        private static String WHERE_ENABLED = " ( " + Entity.GetFieldName(typeof(Trigger), "enabled") + " = ?enabled ) ";
        private static String WHERE_TYPE = " ( " + Entity.GetFieldName(typeof(Trigger), "triggertype") + " = ?triggertype ) ";
        private static String WHERE_NAME = " ( " + Entity.GetFieldName(typeof(Trigger), "name") + " LIKE ?name ) ";

        public override void PrepareBuilder(IDbParametersBuilder builder, BrowseTriggersEntities_FreeBrowse bpe)
        {
            base.PrepareBuilder(builder, bpe);

            if (bpe.Enabled != -1)
                builder.Create().Name("enabled").Type(DbType.Boolean).Value(bpe.Enabled == 1);

            if (bpe.TriggerType != TriggerType.Unknown)
                builder.Create().Name("triggertype").Type(DbType.UInt16).Value(bpe.TriggerTypeID);

            if (String.IsNullOrEmpty(bpe.Name) == false)
                builder.Create().Name("name").Type(DbType.String).Value("%" + bpe.Name + "%");
        }

        public override String GetWhere(IDbParametersBuilder builder, BrowseTriggersEntities_FreeBrowse bpe)
        {
            String where = String.Empty;

            if (bpe.Enabled != -1)
            {
                if (where.Length > 0) where += " AND ";
                where += WHERE_ENABLED;
            }

            if (bpe.TriggerType != TriggerType.Unknown)
            {
                if (where.Length > 0) where += " AND ";
                where += WHERE_TYPE;                
            }

            if (String.IsNullOrEmpty(bpe.Name) == false)
            {
                if (where.Length > 0) where += " AND ";
                where += WHERE_NAME;                
            }
            
            if (String.IsNullOrEmpty(where)) return WHERE;
            
            return " WHERE " + where; 
        }
    }
}
