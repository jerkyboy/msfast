using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySpace.MSFast.Core.Configuration.CollectorsConfig
{
    public class JSONObjectCollectorsConfig
    {
        public CollectorsArgument[] Arguments = null;
        public JSONObjectCollectorsScriptGroups[] ScriptGroups = null;
    }
    public class JSONObjectCollectorsScriptGroups
    {
        public CollectorsScript[] Scripts;
    }

    public class JSONCollectorsConfigLoader : ICollectorsConfigLoader
    {
        private String jsonString = String.Empty;
        
        public JSONCollectorsConfigLoader(String jsonString)
        {
            this.jsonString = jsonString;
        }

        public virtual void LoadConfig(CollectorsConfig cc)
        {
            if (String.IsNullOrEmpty(jsonString))
                return;

            JSONObjectCollectorsConfig ccjo = JsonConvert.DeserializeObject<JSONObjectCollectorsConfig>(jsonString);

            if (ccjo == null)
                return;

            if (ccjo.Arguments != null)
            {
                foreach (CollectorsArgument arg in ccjo.Arguments)
                {
                    if (arg == null || String.IsNullOrEmpty(arg.Key) == false)
                        cc.SetArgument(arg.Key, arg.Value);
                }
            }

            if (ccjo.ScriptGroups != null)
            {
                foreach (JSONObjectCollectorsScriptGroups grp in ccjo.ScriptGroups)
                {
                    if (grp == null || grp.Scripts == null || grp.Scripts.Length == 0)
                        continue;

                    List<CollectorsScript> group = new List<CollectorsScript>();

                    foreach (CollectorsScript cs in grp.Scripts)
                        group.Add(cs);

                    cc.AddScriptsGroup(group.ToArray());
                }
            }
        }
    }
    public class JSONCollectorsConfigWriter : ICollectorsConfigWriter
    {
        public virtual String SerializeConfig(CollectorsConfig cc)
        {
            if (cc == null)
                return null;

            JSONObjectCollectorsConfig jcc = new JSONObjectCollectorsConfig();

            jcc.Arguments = cc.GetAllArguments();

            if (cc.ScriptsGroupCount > 0)
            {
                List<JSONObjectCollectorsScriptGroups> scgrp = new List<JSONObjectCollectorsScriptGroups>(cc.ScriptsGroupCount);

                for (uint i = 0; i < cc.ScriptsGroupCount; i++)
                {
                    CollectorsScript[] cs = cc.GetScriptsGroup(i);

                    if (cs != null && cs.Length > 0)
                    {
                        scgrp.Add(new JSONObjectCollectorsScriptGroups() { Scripts = cs });
                    }
                }

                jcc.ScriptGroups = scgrp.ToArray();
            }

            return JsonConvert.SerializeObject(jcc);
        }
    }
}
