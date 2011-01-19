using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.Dao;
using BDika.Entities.Triggers;
using BDika.Entities.Tests;
using System.Data;

namespace BDika.Dao.DB.Triggers
{
    public class DeleteTriggerToTestAndTesterTypeDBDAO : DBEntityAccessObject<TriggerToTestAndTesterType>
    {
        private static String DELETE = " DELETE FROM triggertotestandtestertype WHERE triggerid IN ({0}) AND testid IN ({1}) AND testertypeid IN ({2}); ";

        public override IDAOTransaction ExecuteCall(EntitiesDAOTransaction<TriggerToTestAndTesterType> w)
        {
            StringBuilder testids = new StringBuilder();
            StringBuilder testertypeids = new StringBuilder();
            StringBuilder triggerids = new StringBuilder();

            int cnt = 0;

            foreach (TriggerToTestAndTesterType t in w.Entities.Values)
            {
                if (t != null && TestID.IsValidTestID(t.TestID) && TesterTypeID.IsValidTesterTypeID(t.TesterTypeID) && TriggerID.IsValidTriggerID(t.TriggerID))
                {
                    if (testids.Length > 0) testids.Append(',');
                    testids.Append(t.TestID.ColumnValue.ToString());

                    if (testertypeids.Length > 0) testertypeids.Append(',');
                    testertypeids.Append(t.TesterTypeID.ColumnValue.ToString());

                    if (triggerids.Length > 0) triggerids.Append(',');
                    triggerids.Append(t.TriggerID.ColumnValue.ToString());

                    cnt++;
                }
            }

            if (testertypeids.Length == 0)
            {
                w.Succeeded = true;
                return w;
            }

            w.Succeeded = AdoTemplate.ExecuteNonQuery(CommandType.Text, String.Format(DELETE, triggerids.ToString(), testids.ToString(), testertypeids.ToString())) == cnt;

            return w;
        }
    }
}
