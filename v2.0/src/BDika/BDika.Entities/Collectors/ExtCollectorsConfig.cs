﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace BDika.Entities.Collectors
{
    public class ExtCollectorsConfig : CollectorsConfig
    {
        private class CollectorsArgumentMeta
        {
            public bool IsOverride = false;
            public String OriginalValue = String.Empty;
        }

        private Dictionary<String, CollectorsArgumentMeta> metaData = null;
        private List<String> newValues = new List<string>();

        public void ResetPreviousConfig()
        {
            if (newValues != null)
                newValues.Clear();

            if (metaData != null)
            {
                foreach (CollectorsArgumentMeta cam in metaData.Values)
                {
                    cam.IsOverride = false;
                }
            }
        }

        public override void SetArgument(string k, string v)
        {
            if (metaData == null)
                metaData = new Dictionary<string, CollectorsArgumentMeta>();

            CollectorsArgumentMeta cam = null;

            metaData.TryGetValue(k, out cam);

            if (cam == null)
                metaData.Add(k, cam = new CollectorsArgumentMeta());

            CollectorsArgument ca = GetArgument(k);
            
            if (newValues.Contains(k) == false)
                newValues.Add(k);

            if (ca != null)
            {
                cam.IsOverride = true;
                cam.OriginalValue = ca.Value;
            }
            else
            {
                cam.IsOverride = false;
                cam.OriginalValue = null;
            }

            base.SetArgument(k, v);
        }

        public bool IsNewVal(CollectorsArgument cv)
        {
            return newValues.Contains(cv.Key);
        }

        public bool IsOverride(CollectorsArgument cv)
        {
            return IsNewVal(cv) && this.metaData != null && this.metaData.ContainsKey(cv.Key) && this.metaData[cv.Key].IsOverride; 
        }

        public string GetOriginalValue(CollectorsArgument cv)
        {
            return IsOverride(cv) ? this.metaData[cv.Key].OriginalValue : null;
        }
    }

    public class ExtCollectorsConfigLoader : ICollectorsConfigLoader
    {
        private ExtCollectorsConfigEntity configuration;

        public ExtCollectorsConfigLoader(ExtCollectorsConfigEntity extCollectorsConfigEntity)
        {
            this.configuration = extCollectorsConfigEntity;
        }

        public void LoadConfig(CollectorsConfig cc)
        {
            if (configuration == null)
                return;

            if (String.IsNullOrEmpty(configuration.TriggerConfiguration) == false) cc.AppendConfig(new JSONCollectorsConfigLoaderOverride(configuration.TriggerConfiguration));
            if (String.IsNullOrEmpty(configuration.TestConfiguration) == false) cc.AppendConfig(new JSONCollectorsConfigLoaderOverride(configuration.TestConfiguration));
            if (String.IsNullOrEmpty(configuration.TesterTypeConfiguration) == false) cc.AppendConfig(new JSONCollectorsConfigLoaderOverride(configuration.TesterTypeConfiguration));
            if (String.IsNullOrEmpty(configuration.TesterTypeAndTestConfiguration) == false) cc.AppendConfig(new JSONCollectorsConfigLoaderOverride(configuration.TesterTypeAndTestConfiguration));
            if (String.IsNullOrEmpty(configuration.TesterTypeAndTestAndTriggerConfiguration) == false) cc.AppendConfig(new JSONCollectorsConfigLoaderOverride(configuration.TesterTypeAndTestAndTriggerConfiguration));
        }

        private class JSONCollectorsConfigLoaderOverride : JSONCollectorsConfigLoader
        {
            public JSONCollectorsConfigLoaderOverride(String s) : base(s) { }

            public override void  LoadConfig(CollectorsConfig cc)
            {
                if (cc == null) return;

                if (cc is ExtCollectorsConfig)
                {
                    lock (cc)
                    {
                        ((ExtCollectorsConfig)cc).ResetPreviousConfig();
                        base.LoadConfig(cc);
                    }
                }
                else
                {
                    base.LoadConfig(cc);
                }
            }
        }
    }

}