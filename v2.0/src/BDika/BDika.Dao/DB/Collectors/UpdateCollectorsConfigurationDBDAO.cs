using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.Dao;
using BDika.Providers.Collectors;
using EYF.Entities;
using Spring.Data.Common;
using System.Data;
using BDika.Entities.Collectors;

namespace BDika.Dao.DB.Collectors
{
    public class UpdateCollectorsConfigurationDBDAO<T> : DBEntityAccessObject<T> where T : UpdateConfigEntityCommand
    {
        private const String UPDATE_TRIGGERID =            "UPDATE triggers SET configuration = ?configuration WHERE triggerid = ?triggerid;";
        private const String UPDATE_TESTID =              "UPDATE tests SET configuration = ?configuration WHERE testid = ?testid;";
        private const String UPDATE_TESTERTYPEID =        "UPDATE testertypes SET configuration = ?configuration WHERE testertypeid = ?testertypeid;";
        private const String UPDATE_TESTID_TESTERTYPEID = "UPDATE testertypestotest SET configuration = ?configuration WHERE testid = ?testid AND testertypeid = ?testertypeid;";
        private const String UPDATE_TRIGGERID_TESTID_TESTERTYPEID = "UPDATE triggertotestandtestertype SET configuration = ?configuration WHERE testid = ?testid AND testertypeid = ?testertypeid AND triggerid = ?triggerid;";

        public override IDAOTransaction ExecuteCall(EntitiesDAOTransaction<T> t)
        {
            foreach (T ent in t.Entities.Values)
            {
                IDbParametersBuilder builder = CreateDbParametersBuilder();
                String update = String.Empty;

                if(ent is UpdateConfig_Test_EntityCommand)
                {
                    UpdateConfig_Test_EntityCommand d = ent as UpdateConfig_Test_EntityCommand;
                    builder.Create().Name("testid").Type(DbType.UInt32).Value(d.TestID.ColumnValue);
                    update = UPDATE_TESTID;
                }
                else if(ent is UpdateConfig_TesterType_EntityCommand)
                {
                    UpdateConfig_TesterType_EntityCommand d = ent as UpdateConfig_TesterType_EntityCommand;
                    builder.Create().Name("testertypeid").Type(DbType.UInt32).Value(d.TesterTypeID.ColumnValue);
                    update = UPDATE_TESTERTYPEID;
                }
                else if(ent is UpdateConfig_TesterTypeToTest_EntityCommand)
                {
                    UpdateConfig_TesterTypeToTest_EntityCommand d = ent as UpdateConfig_TesterTypeToTest_EntityCommand;
                    builder.Create().Name("testertypeid").Type(DbType.UInt32).Value(d.TesterTypeID.ColumnValue);
                    builder.Create().Name("testid").Type(DbType.UInt32).Value(d.TestID.ColumnValue);
                    update = UPDATE_TESTID_TESTERTYPEID;
                }
                else if(ent is UpdateConfig_Trigger_EntityCommand)
                {
                    UpdateConfig_Trigger_EntityCommand d = ent as UpdateConfig_Trigger_EntityCommand;
                    builder.Create().Name("triggerid").Type(DbType.UInt32).Value(d.TriggerID.ColumnValue);
                    update = UPDATE_TRIGGERID;
                }
                else if(ent is UpdateConfig_TriggerToTestAndTesterType_EntityCommand)
                {
                    UpdateConfig_TriggerToTestAndTesterType_EntityCommand d = ent as UpdateConfig_TriggerToTestAndTesterType_EntityCommand;
                    builder.Create().Name("testertypeid").Type(DbType.UInt32).Value(d.TesterTypeID.ColumnValue);
                    builder.Create().Name("testid").Type(DbType.UInt32).Value(d.TestID.ColumnValue);
                    builder.Create().Name("triggerid").Type(DbType.UInt32).Value(d.TriggerID.ColumnValue);
                    update = UPDATE_TRIGGERID_TESTID_TESTERTYPEID;
                }

                builder.Create().Name("configuration").Type(DbType.String).Value(ent.RawConfiguration);

                AdoTemplate.ExecuteNonQuery(CommandType.Text, update, builder.GetParameters());
                t.Succeeded = true;

                return t;
            }
            return null;
        }

    }

    public class UpdateConfig_Test_EntityCommandDBDAO : UpdateCollectorsConfigurationDBDAO<UpdateConfig_Test_EntityCommand> { }
    public class UpdateConfig_TesterType_EntityCommandDBDAO : UpdateCollectorsConfigurationDBDAO<UpdateConfig_TesterType_EntityCommand> { }
    public class UpdateConfig_TesterTypeToTest_EntityCommandDBDAO : UpdateCollectorsConfigurationDBDAO<UpdateConfig_TesterTypeToTest_EntityCommand> { }
    public class UpdateConfig_Trigger_EntityCommandDBDAO : UpdateCollectorsConfigurationDBDAO<UpdateConfig_Trigger_EntityCommand> { }
    public class UpdateConfig_TriggerToTestAndTesterType_EntityCommandDBDAO : UpdateCollectorsConfigurationDBDAO<UpdateConfig_TriggerToTestAndTesterType_EntityCommand> { }
}

