//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.Providers)
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
using MySpace.MSFast.Automation.Entities.Triggers;
using EYF.Entities.StandardObjects;
using EYF.Providers.Gateway;
using MySpace.MSFast.Automation.Providers.Tests.Browse;
using MySpace.MSFast.Automation.Providers.Triggers.Browse;
using MySpace.MSFast.Automation.Entities.Results;
using MySpace.MSFast.Automation.Providers.Collectors.Browse;
using MySpace.MSFast.Automation.Entities.Collectors;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using MySpace.MSFast.Automation.Entities.Tests;

namespace MySpace.MSFast.Automation.Providers.Triggers
{
    public class TriggersProvider
    {
        public static Trigger GetTrigger(UserID UserID, TriggerID TriggerID)
        {
            if (TriggerID.IsValidTriggerID(TriggerID) == false) throw new InvalidTriggerIDException();
            return EntitiesGateway.GetEntity<Trigger>(TriggerID);
        }

        public static Trigger UpdateOrCreateTrigger(UserID userID, Trigger t)
        {
            if (t == null) throw new Exception("Invalid Trigger");
            if (String.IsNullOrEmpty(t.TriggerName)) throw new Exception("Invalid Trigger Name");

            if (TriggerID.IsValidTriggerID(t.TriggerID))
            {
                EntitiesGateway.UpdateEntity(new UpdateTriggerDetailsEntityCommand()
                {
                    TriggerID = t.TriggerID,
                    TriggerName = t.TriggerName,
                    TriggerTypeID = t.TriggerTypeID,
                    Enabled = t.Enabled,
                    Timeout = t.Timeout
                });
                return t;
            }
            else
            {
                return EntitiesGateway.AddEntity(t);
            }
        }

        public static bool Trigger(UserID userID, TriggerID t)
        {
            if(TriggerID.IsValidTriggerID(t) == false) throw new InvalidTriggerIDException();

            BrowseExtCollectorsConfigEntities bpr = new BrowseExtCollectorsConfigEntities();
            bpr.TriggerID = t;           
            return PopulateResults(bpr);
        }

        public static bool TriggerAllTimedBased()
        {
            BrowseExtCollectorsConfigEntities bpr = new BrowseExtCollectorsConfigEntities();
            bpr.TriggerAllTimedbased = true;           
            return PopulateResults(bpr);
        }

        private static bool PopulateResults(BrowseExtCollectorsConfigEntities bpr)
        {
            int max = 500;
            
            List<TriggerID> lastTriggered = new List<TriggerID>();

            while ((max--) > 0)
            {   
                bpr.Load(null);

                if (bpr.Data == null || bpr.Data.Count == 0)
                    break;

                List<Entities.Results.Results> results = new List<MySpace.MSFast.Automation.Entities.Results.Results>();

                foreach (ExtCollectorsConfigEntity prr in bpr.Data)
                {
                    ExtCollectorsConfig ec = new ExtCollectorsConfig();
                    ec.AppendConfig(new ExtCollectorsConfigLoader(prr));

                    results.Add(new MySpace.MSFast.Automation.Entities.Results.Results()
                    {
                        TesterTypeID = prr.TesterTypeID,
                        TestID = prr.TestID,
                        ResultsState = ResultsState.Pending,
                        CreatedOn = EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime(),
                        RawConfig = new JSONCollectorsConfigWriter().SerializeConfig(ec)
                    });

                    if (lastTriggered.Contains(prr.TriggerID) == false)
                        lastTriggered.Add(prr.TriggerID);
                }

                if (results.Count == 0)
                    break;

                EntitiesGateway.AddEntities(results);
                
                if (bpr.Data.Count < bpr.ResultsPerPage)
                    break;
            }

            
            if (lastTriggered != null && lastTriggered.Count > 0)
            {
                List<LastTriggerEntityCommand> cmd = new List<LastTriggerEntityCommand>();

                foreach (TriggerID tid in lastTriggered)
                {
                    cmd.Add(new LastTriggerEntityCommand() { TriggerID = tid, LastTriggered = EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime() });
                }
                
                if(cmd.Count > 0)
                    EntitiesGateway.UpdateEntities(cmd);
            }

            return true;
        }

        public static bool SetTestsForTriggerAndBox(TriggerID TriggerID, TesterTypeID TesterTypeID, HashSet<TestID> testids)
        {
            TriggerID.ValidateTriggerID(TriggerID);
            TesterTypeID.ValidateTesterTypeID(TesterTypeID);
            if (testids == null || testids.Count == 0) return true;

            List<TriggerToTestAndTesterType> lst = new List<TriggerToTestAndTesterType>();
                
            foreach (TestID tid in testids)
            {
                lst.Add(new TriggerToTestAndTesterType()
                {
                    TesterTypeID = TesterTypeID,
                    TriggerID = TriggerID,
                    TestID = tid,
                    TriggerToTestAndTesterTypeID = new TriggerToTestAndTesterTypeID(TriggerID, TesterTypeID, tid)
                });
            }
            if (lst.Count > 0)
            {
                return EntitiesGateway.AddEntities(lst) != null;
            }
            return true;
        }
        public static bool RemoveTestsForTriggerAndBox(TriggerID TriggerID, TesterTypeID TesterTypeID, HashSet<TestID> testids)
        {
            TriggerID.ValidateTriggerID(TriggerID);
            TesterTypeID.ValidateTesterTypeID(TesterTypeID);
            if (testids == null || testids.Count == 0) return true;

            List<TriggerToTestAndTesterType> lst = new List<TriggerToTestAndTesterType>();

            foreach (TestID tid in testids)
            {
                lst.Add(new TriggerToTestAndTesterType()
                {
                    TesterTypeID = TesterTypeID,
                    TriggerID = TriggerID,
                    TestID = tid,
                    TriggerToTestAndTesterTypeID = new TriggerToTestAndTesterTypeID(TriggerID, TesterTypeID, tid)
                });
            }
            if (lst.Count > 0)
            {
                return EntitiesGateway.DeleteEntities(lst);
            }
            return true;

        }

        public static bool DeleteTrigger(UserID userID, TriggerID triggerID)
        {
            if (TriggerID.IsValidTriggerID(triggerID) == false) throw new InvalidTriggerIDException();

            return EntitiesGateway.DeleteEntity(new Trigger() { TriggerID = triggerID });
           
        }
    }
}
