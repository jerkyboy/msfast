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
using MySpace.MSFast.Automation.Providers.Results.Browse;
using Spring.Data.Common;
using System.Data;
using EYF.Entities;
using MySpace.MSFast.Automation.Entities.Results;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Entities.Triggers;

namespace MySpace.MSFast.Automation.Dao.DB.Results.Browse
{
    public class BrowseResultsEntities_NoCacheDBDAO<T> : DBEntityAccessObject<T> where T : BrowseResultsEntities
    {
        private static EntitiesCollectionMapper<T> entityMapper = new EntitiesCollectionMapper<T>();

        public static String COUNT = "SELECT COUNT(DISTINCT " + Entity.GetFieldName(typeof(Entities.Results.Results), "resultsid") + ") ";
        public static String SELECT = "SELECT DISTINCT " + Entity.GetFieldName(typeof(Entities.Results.Results), "resultsid") + " as results_resultsid, " +
                                      " ( " + Entity.GetFieldName(typeof(Entities.Results.Results), "endtime") + " - " + Entity.GetFieldName(typeof(Entities.Results.Results), "starttime") + ") as results_totaltime, " +
                                      Entity.GetFieldName(typeof(Test), "testname") + " as tests_testname, " +
                                      Entity.GetFieldName(typeof(TesterType), "name") + " as testertypes_name, " +
                                      " 'BPI' as results_bpi ";

        public static String FROM = " FROM " + Entity.GetTableNameAndNick(typeof(Entities.Results.Results)) + 
                                    " LEFT JOIN " + Entity.GetTableNameAndNick(typeof(Test)) + " ON " + Entity.GetFieldName(typeof(Entities.Results.Results), "testid") + " = " + Entity.GetFieldName(typeof(Test), "testid") +
                                    " LEFT JOIN " + Entity.GetTableNameAndNick(typeof(TesterType)) + " ON " + Entity.GetFieldName(typeof(Entities.Results.Results), "testertypeid") + " = " + Entity.GetFieldName(typeof(TesterType), "testertypeid");

        public static String FROM_TRIGGER = " FROM " + Entity.GetTableNameAndNick(typeof(TriggerToTestAndTesterType)) + ", " + Entity.GetTableNameAndNick(typeof(Entities.Results.Results)) +  
                                            " LEFT JOIN " + Entity.GetTableNameAndNick(typeof(Test)) + " ON " + Entity.GetFieldName(typeof(Entities.Results.Results), "testid") + " = " + Entity.GetFieldName(typeof(Test), "testid") +
                                            " LEFT JOIN " + Entity.GetTableNameAndNick(typeof(TesterType)) + " ON " + Entity.GetFieldName(typeof(Entities.Results.Results), "testertypeid") + " = " + Entity.GetFieldName(typeof(TesterType), "testertypeid");

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

                String sql = String.Concat(GetSelect(builder, bpe), GetFrom(builder, bpe), GetWhere(builder, bpe), GetGroupBy(builder,bpe), GetOrder(builder, bpe), GetLimit(builder, bpe));
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
        public virtual String GetGroupBy(IDbParametersBuilder builder,T bpe){return String.Empty;}
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

        private static String WHERE_ALL = " WHERE " + Entity.GetFieldName(typeof(Entities.Results.Results), "resultsid") + " IN ( " +
                                          " SELECT MAX(" + Entity.GetFieldName(typeof(Entities.Results.Results), "resultsid") + ") " +
                                          " FROM " + Entity.GetTableNameAndNick(typeof(Entities.Results.Results)) +
                                          " WHERE " + Entity.GetFieldName(typeof(Entities.Results.Results), "state") + " = " + ((uint)ResultsState.Succeeded) +
                                          " GROUP BY " + Entity.GetFieldName(typeof(Entities.Results.Results), "testid") + ", " + Entity.GetFieldName(typeof(Entities.Results.Results), "testertypeid") +
                                          " ) ";

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

            if (bpe.ResultsState == ResultsState.Unknown &&
                TriggerID.IsValidTriggerID(bpe.TriggerID) == false &&
                TestID.IsValidTestID(bpe.TestID) == false &&
                TesterTypeID.IsValidTesterTypeID(bpe.TesterTypeID) == false)
            {
                return WHERE_ALL;
            }

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
        
        public override string GetOrder(IDbParametersBuilder builder, BrowseResultsEntities_FreeBrowse bpe)
        {
            String order = Entity.GetFieldName(typeof(Entities.Results.Results), "resultsid");

            if(String.IsNullOrEmpty(bpe.Sort) == false){
                if("testname".Equals(bpe.Sort)){order = "tests_testname";}else
                if("testbox".Equals(bpe.Sort)){order = "testertypes_name";}else 
                if("createdon".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "created");}else
                if("totaltime".Equals(bpe.Sort)){order = "results_totaltime";}else
                if("rendertime".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "totalrendertime");}else
                if("servertime".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "firstrequesttime");}else
                if("totaldownloadscount".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "totaldownloadscount");}else
                if("totaljsdownloadscount".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "totaljsdownloadscount");}else
                if("totalcssdownloadscount".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "totalcssdownloadscount");}else
                if("totalimagesdownloadscount".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "totalimagesdownloadscount");}else
                if("totaldownloadsize".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "totaldownloadsize");}else
                if("totaljsdownloadsize".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "totaljsdownloadsize");}else
                if("totalcssdownloadsize".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "");}else
                if("totalimagesdownloadsize".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "totalimagesdownloadsize");}else
                if("processortimeavg".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "processortimeavg");}else
                if("usertimeavg".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "usertimeavg");}else
                if("privateworkingsetdelta".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "privateworkingsetdelta");}else
                if("workingsetdelta".Equals(bpe.Sort)){order = Entity.GetFieldName(typeof(Entities.Results.Results), "workingsetdelta");}
            }
            return " ORDER BY " + order + (bpe.DESC ? " DESC " : " ASC ");
        }
    }
}
