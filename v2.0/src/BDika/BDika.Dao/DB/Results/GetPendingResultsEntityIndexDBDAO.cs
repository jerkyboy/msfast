using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Dao.DB.Common;
using EYF.Entities;
using BDika.Entities.Results;
using EYF.Entities.Dao;
using BDika.Entities.Tests;

namespace BDika.Dao.DB.Results
{
    public class GetPendingResultsEntityIndexDBDAO : GetObjectsDBDAO
    {
        public static String SELECT = "SELECT " + Entity.GetSelectFieldsList(typeof(PendingResultsEntityIndex)) +
                                        " FROM " + Entity.GetTableNameAndNick(typeof(PendingResultsEntityIndex)) + " WHERE " +
                                        Entity.GetFieldName(typeof(PendingResultsEntityIndex), "testertypeid") + " = {0} " +
                                        " AND " + Entity.GetFieldName(typeof(PendingResultsEntityIndex), "state") + " = " + ((uint)ResultsState.Pending) + " ORDER BY " + Entity.GetFieldName(typeof(PendingResultsEntityIndex), "created") + " ASC;";

        public override string GetIDs<T>(EntitiesDAOTransaction<T> t)
        {
            EntitiesDAOTransaction<PendingResultsEntityIndex> et = t as EntitiesDAOTransaction<PendingResultsEntityIndex>;

            if (et == null)
                return null;

            foreach (IEntityIdentifier id in et.EntitiesIdentities)
            {
                if ((id is TesterTypeID) == false) continue;

                TesterTypeID ttti = id as TesterTypeID;
                
                return id.ToString();
            }

            return null;
        }

        public override string GetWhere<T>(EntitiesDAOTransaction<T> t) {return null;}

        public override String GetSelect<T>(EntitiesDAOTransaction<T> t, String where)
        {
            return SELECT;
        }
        
    }
}
