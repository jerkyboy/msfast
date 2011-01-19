using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDika.Entities.Tests;
using EYF.Entities.Dao;
using System.Data;

namespace BDika.Dao.DB.Tests
{
    public class DeleteTesterTypeToTestDBDAO : DBEntityAccessObject<TesterTypeToTest>
    {
        private static String DELETE = " DELETE FROM testertypestotest WHERE testid IN ({0}) AND testertypeid IN ({1}); ";

        public override IDAOTransaction ExecuteCall(EntitiesDAOTransaction<TesterTypeToTest> w)
        {
            StringBuilder testids = new StringBuilder();
            StringBuilder testertypeids = new StringBuilder();
            
            int cnt = 0;

            foreach (TesterTypeToTest t in w.Entities.Values)
            {
                if(t != null && TestID.IsValidTestID(t.TestID) && TesterTypeID.IsValidTesterTypeID(t.TesterTypeID))
                {
                    if (testids.Length > 0) testids.Append(',');
                    testids.Append(t.TestID.ColumnValue.ToString());

                    if (testertypeids.Length > 0) testertypeids.Append(',');
                    testertypeids.Append(t.TesterTypeID.ColumnValue.ToString());

                    cnt++;
                }
            }

            if (testertypeids.Length == 0)
            {
                w.Succeeded = true;
                return w;
            }

            w.Succeeded = AdoTemplate.ExecuteNonQuery(CommandType.Text, String.Format(DELETE, testids.ToString(), testertypeids.ToString())) == cnt;
            
            return w;
        }
    }
}
