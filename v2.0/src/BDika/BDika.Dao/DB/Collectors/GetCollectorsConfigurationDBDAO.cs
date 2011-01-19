﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.Dao;
using BDika.Entities.Collectors;
using EYF.Entities;
using Spring.Data.Common;
using BDika.Entities.Triggers;
using BDika.Entities.Tests;
using System.Data;

namespace BDika.Dao.DB.Collectors
{
    public abstract class AbsGetCollectorsConfigurationDBDAO<T> : DBEntityAccessObject<T> where T : EYF.Entities.IEntity
    {
        public static EntitiesCollectionMapper<ExtCollectorsConfigEntity> entityMapper = new EntitiesCollectionMapper<ExtCollectorsConfigEntity>();

        public const String SELECT_TRIGGERID = "SELECT CONVERT(CONCAT(triggers.triggerid,'_0_0') USING utf8) as config_id, triggers.triggerid as config_triggerid, 0 as config_testid, 0 as config_testertypeid, triggers.configuration as 'config_triggers' FROM triggers WHERE triggers.triggerid = ?triggerid;";
        public const String SELECT_TESTID = "SELECT CONVERT(CONCAT('0_0_',tests.testid) USING utf8) as config_id, 0 as config_triggerid, tests.testid as config_testid, 0 as config_testertypeid, tests.configuration as 'config_tests' FROM tests WHERE tests.testid = ?testid;";
        public const String SELECT_TESTERTYPEID = "SELECT CONVERT(CONCAT('0_',testertypes.testertypeid,'_0') USING utf8) as config_id, 0 as config_triggerid, 0 as config_testid, testertypes.testertypeid as config_testertypeid, testertypes.configuration as 'config_testertypes' FROM testertypes WHERE testertypes.testertypeid = ?testertypeid;";

        public const String SELECT_TESTID_TESTERTYPEID = "SELECT CONVERT(CONCAT('0_',testertypes.testertypeid,'_',tests.testid) USING utf8) as config_id, 0 as config_triggerid, tests.testid as config_testid, testertypes.testertypeid as config_testertypeid, tests.configuration as 'config_tests', testertypes.configuration as 'config_testertypes', testertypestotest.configuration as 'config_testertypestotest' " +
                                                          " FROM tests, testertypes, testertypestotest " +
                                                          " WHERE tests.testid = ?testid AND testertypes.testertypeid = ?testertypeid AND testertypestotest.testid = tests.testid AND testertypes.testertypeid = " +
                                                          " testertypestotest.testertypeid ";

        public const String SELECT_TRIGGERID_TESTID_TESTERTYPEID = "SELECT CONVERT(CONCAT(triggers.triggerid,'_',testertypes.testertypeid,'_',tests.testid) USING utf8) as config_id, triggers.triggerid as config_triggerid, tests.testid as config_testid, testertypes.testertypeid as config_testertypeid, tests.configuration as 'config_tests', testertypes.configuration as 'config_testertypes', testertypestotest.configuration as 'config_testertypestotest', " +
                                                                    " triggers.configuration as 'config_triggers', triggertotestandtestertype.configuration as 'config_triggertotestandtestertype' ";
        public const String SELECT_TRIGGERID_TESTID_TESTERTYPEID_FROM = " FROM triggers,tests, testertypes, testertypestotest,triggertotestandtestertype ";
        public const String SELECT_TRIGGERID_TESTID_TESTERTYPEID_WHERE = " WHERE testertypestotest.testid = tests.testid AND testertypes.testertypeid = testertypestotest.testertypeid AND triggertotestandtestertype.triggerid = triggers.triggerid " +
                                                                    " AND triggertotestandtestertype.testid = tests.testid AND triggertotestandtestertype.testertypeid = testertypestotest.testertypeid ";

        public override IDAOTransaction ExecuteCall(EntitiesDAOTransaction<T> w)
        {
            return ExecuteExtCall(w);
        }
        public abstract IDAOTransaction ExecuteExtCall(EntitiesDAOTransaction<T> w);
    }

    public class GetCollectorsConfigurationDBDAO : AbsGetCollectorsConfigurationDBDAO<ExtCollectorsConfigEntity>
    {
        public override IDAOTransaction ExecuteExtCall(EntitiesDAOTransaction<ExtCollectorsConfigEntity> t)
        {
            foreach (IEntityIdentifier id in t.EntitiesIdentities)
            {
                IDbParametersBuilder builder = CreateDbParametersBuilder();

                TriggerToTestAndTesterTypeID bpe = (TriggerToTestAndTesterTypeID)id;

                String select = String.Empty;

                if (TesterTypeID.IsValidTesterTypeID(bpe.TesterTypeID) && TestID.IsValidTestID(bpe.TestID) && TriggerID.IsValidTriggerID(bpe.TriggerID))
                {
                    select = SELECT_TRIGGERID_TESTID_TESTERTYPEID + 
                             SELECT_TRIGGERID_TESTID_TESTERTYPEID_FROM + 
                             SELECT_TRIGGERID_TESTID_TESTERTYPEID_WHERE + 
                             " AND tests.testid = ?testid AND testertypes.testertypeid = ?testertypeid AND triggers.triggerid = ?triggerid ";
                }
                else if (TesterTypeID.IsValidTesterTypeID(bpe.TesterTypeID) && TestID.IsValidTestID(bpe.TestID))
                {
                    select = SELECT_TESTID_TESTERTYPEID;
                }
                else if (TesterTypeID.IsValidTesterTypeID(bpe.TesterTypeID))
                {
                    select = SELECT_TESTERTYPEID;
                }
                else if (TestID.IsValidTestID(bpe.TestID))
                {
                    select = SELECT_TESTID;
                }
                else if (TriggerID.IsValidTriggerID(bpe.TriggerID))
                {
                    select = SELECT_TRIGGERID;
                }


                if (TesterTypeID.IsValidTesterTypeID(bpe.TesterTypeID))
                    builder.Create().Name("testertypeid").Type(DbType.UInt32).Value(bpe.TesterTypeID.ColumnValue);

                if (TestID.IsValidTestID(bpe.TestID))
                    builder.Create().Name("testid").Type(DbType.UInt32).Value(bpe.TestID.ColumnValue);

                if (TriggerID.IsValidTriggerID(bpe.TriggerID))
                    builder.Create().Name("triggerid").Type(DbType.UInt32).Value(bpe.TriggerID.ColumnValue);

                t.Entities = AdoTemplate.QueryWithResultSetExtractor(CommandType.Text, select, entityMapper, builder.GetParameters());
                t.Succeeded = (t.Entities != null);

                return t;
            }
            return null;
        }
    }
}
