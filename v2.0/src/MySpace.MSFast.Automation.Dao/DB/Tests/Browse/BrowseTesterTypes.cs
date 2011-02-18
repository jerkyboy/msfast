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
using EYF.Entities.Dao;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Providers.Tests.Browse;
using Spring.Data.Common;
using System.Data;
using MySpace.MSFast.Automation.Entities.Triggers;

namespace MySpace.MSFast.Automation.Dao.DB.Tests.Browse
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

    public class BrowseTesterTypes_AllDBDAO : BrowseTesterTypes_NoCacheDBDAO<BrowseTesterTypes_All>
    {

    }
}