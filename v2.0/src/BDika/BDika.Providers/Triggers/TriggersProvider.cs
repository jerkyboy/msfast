using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDika.Entities.Triggers;
using EYF.Entities.StandardObjects;
using EYF.Providers.Gateway;
using BDika.Providers.Tests.Browse;
using BDika.Providers.Triggers.Browse;
using BDika.Entities.Results;
using BDika.Providers.Collectors.Browse;
using BDika.Entities.Collectors;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace BDika.Providers.Triggers
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
                EntitiesGateway.UpdateEntity(t);
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

                List<Entities.Results.Results> results = new List<BDika.Entities.Results.Results>();

                foreach (ExtCollectorsConfigEntity prr in bpr.Data)
                {
                    ExtCollectorsConfig ec = new ExtCollectorsConfig();
                    ec.AppendConfig(new ExtCollectorsConfigLoader(prr));

                    results.Add(new BDika.Entities.Results.Results()
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
    }
}
