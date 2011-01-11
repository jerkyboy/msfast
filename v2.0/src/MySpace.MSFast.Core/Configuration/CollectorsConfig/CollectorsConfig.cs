using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;

namespace MySpace.MSFast.Core.Configuration.CollectorsConfig
{
    public class CollectorsConfig
    {
        private static object obj = new object();
        
        public void AppendConfig(ICollectorsConfigLoader loader){
            lock(obj){
                if(loader != null){
                    loader.LoadConfig(this);
                }
            }
        }
        
        public CollectorsConfig(){}

        private static CollectorsConfig _instance;
        public static CollectorsConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                            _instance = new CollectorsConfig();
                    }
                }

                return _instance;
            }
        }        

        private Dictionary<String, CollectorsArgument> arguments = null;
        private List<CollectorsScript[]> scriptGroups = null;
        
        public virtual String GetArgumentValue(String k)
        {
            CollectorsArgument ca = GetArgument(k);
            if (ca == null) return null;
            return ca.Value;
        }        
        public virtual CollectorsArgument GetArgument(String k)
        {
            if (String.IsNullOrEmpty(k) || arguments == null)
                return null;

            CollectorsArgument ea = null;
            
            arguments.TryGetValue(k, out ea);
            
            return ea;
        }
        public virtual void RemoveArgument(String k)
        {
            if (String.IsNullOrEmpty(k) || arguments == null || arguments.ContainsKey(k) == false)
                return;

            arguments.Remove(k);
        }
        public virtual void SetArgument(String k, String v)
        {
            if(String.IsNullOrEmpty(k))
                return;

            if (arguments == null) 
                arguments = new Dictionary<string, CollectorsArgument>();

            CollectorsArgument cv = null;

            arguments.TryGetValue(k, out cv);

            if (cv == null)
            {
                cv = new CollectorsArgument();
                cv.Key = k;

                arguments.Add(k, cv);
            }

            cv.Value = v;
        }
        public virtual CollectorsArgument[] GetAllArguments()
        {
            if (this.arguments == null || this.arguments.Count == 0)
                return null;

            return new List<CollectorsArgument>(arguments.Values).ToArray();
        }

        public virtual void AddScriptsGroup(CollectorsScript[] scripts)
        {
            if(scripts == null || scripts.Length == 0)
                return;
            
            if(scriptGroups == null) 
                scriptGroups = new List<CollectorsScript[]>();

            scriptGroups.Add(scripts);
        }
        public virtual CollectorsScript[] GetScriptsGroup(uint i)
        {
            if(scriptGroups == null || scriptGroups.Count == 0 || scriptGroups.Count <= i)
                return null;

            return scriptGroups[(int)i];
        }
        public virtual int ScriptsGroupCount
        {
            get{
                return this.scriptGroups == null ? 0 : this.scriptGroups.Count;
            }
        }
        public virtual CollectorsScript[] GetAllScripts()
        {
            if(this.scriptGroups == null || this.scriptGroups.Count == 0)
                return null;

            List<CollectorsScript> lst = new List<CollectorsScript>();
            
            foreach (CollectorsScript[] ss in this.scriptGroups)
            {
                foreach (CollectorsScript s in ss)
                {
                    if (s != null && String.IsNullOrEmpty(s.ScriptName) == false)
                    {
                        if (lst.Contains(s) == false)
                            lst.Add(s);
                    }
                }
            }

            return lst.ToArray();
        }

        public virtual String FormatCollectorsScript(CollectorsScript cs)
        {
            StringBuilder sb = new StringBuilder();

            if (String.IsNullOrEmpty(cs.IsFinished) == false) sb.AppendFormat(GetArgumentValue("Script_IsFinished"), cs.IsFinished);
            if (String.IsNullOrEmpty(cs.OnStartingTest) == false) sb.AppendFormat(GetArgumentValue("Script_OnStartingTest"), cs.OnStartingTest);
            if (String.IsNullOrEmpty(cs.OnTestEnded) == false) sb.AppendFormat(GetArgumentValue("Script_OnTestEnded"), cs.OnTestEnded);
            if (String.IsNullOrEmpty(cs.OnLoadingFirstCollectionPage) == false) sb.AppendFormat(GetArgumentValue("Script_OnLoadingFirstCollectionPage"), cs.OnLoadingFirstCollectionPage);
            if (String.IsNullOrEmpty(cs.OnInit) == false) sb.AppendFormat(GetArgumentValue("Script_OnInit"), cs.OnInit);
            if (String.IsNullOrEmpty(cs.OnStartDocument) == false) sb.AppendFormat(GetArgumentValue("Script_OnStartDocument"), cs.OnStartDocument);
            if (String.IsNullOrEmpty(cs.OnStartHtml) == false) sb.AppendFormat(GetArgumentValue("Script_OnStartHtml"), cs.OnStartHtml);
            if (String.IsNullOrEmpty(cs.OnStartHead) == false) sb.AppendFormat(GetArgumentValue("Script_OnStartHead"), cs.OnStartHead);
            if (String.IsNullOrEmpty(cs.OnEndHead) == false) sb.AppendFormat(GetArgumentValue("Script_OnEndHead"), cs.OnEndHead);
            if (String.IsNullOrEmpty(cs.OnStartBody) == false) sb.AppendFormat(GetArgumentValue("Script_OnStartBody"), cs.OnStartBody);
            if (String.IsNullOrEmpty(cs.OnSegment) == false) sb.AppendFormat(GetArgumentValue("Script_OnSegment"), cs.OnSegment);
            if (String.IsNullOrEmpty(cs.OnEndBody) == false) sb.AppendFormat(GetArgumentValue("Script_OnEndBody"), cs.OnEndBody);
            if (String.IsNullOrEmpty(cs.OnEndHtml) == false) sb.AppendFormat(GetArgumentValue("Script_OnEndHtml"), cs.OnEndHtml);
            if (String.IsNullOrEmpty(cs.OnEndDocument) == false) sb.AppendFormat(GetArgumentValue("Script_OnEndDocument"), cs.OnEndDocument);

            return sb.ToString();
        }
    }

    public class CollectorsArgument
    {
        public String Key = String.Empty;
        public String Value = String.Empty;
    }

    public class CollectorsScript
    {
        public String ScriptName;

        public String IsFinished;
        public String OnStartingTest;
        public String OnTestEnded;
        public String OnLoadingFirstCollectionPage;
        public String OnInit;
        public String OnStartDocument;
        public String OnStartHtml;
        public String OnStartHead;
        public String OnEndHead;
        public String OnStartBody;
        public String OnSegment;
        public String OnEndBody;
        public String OnEndHtml;
        public String OnEndDocument;
    }
}
