using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using Jayrock.Json.Conversion;

namespace MySpace.MSFast.Core.Configuration.CollectorsConfig
{
    public class CollectorsConfig
    {
        private static object obj = new object();
        
        public static void AppendConfig(ICollectorsConfigLoader loader){
            lock(obj){
                if(loader != null){
                    if(_instance == null)
                        _instance = new CollectorsConfig();
                    
                    loader.LoadConfig(_instance);
                }
            }
        }
        
        private CollectorsConfig(){}

        private static CollectorsConfig _instance;
        public static CollectorsConfig Instance
        {
            get
            {
                return _instance;
            }
        }

        public String EmptyHTML = String.Empty;

        public String PageDataCollector = String.Empty;
        
        public String Constructor = String.Empty;
        
        public String Event_OnInit = String.Empty;
        public String Event_OnStartingTest = String.Empty;
        public String Event_OnLoadingFirstCollectionPage = String.Empty;
        public String Event_OnStartDocument = String.Empty;
        public String Event_OnStartHtml = String.Empty;
        public String Event_OnStartHead = String.Empty;
        public String Event_OnEndHead = String.Empty;
        public String Event_OnStartBody = String.Empty;
        public String Event_OnSegment = String.Empty;
        public String Event_OnEndBody = String.Empty;
        public String Event_OnEndHtml = String.Empty;
        public String Event_OnEndDocument = String.Empty;

        public String Script_IsFinished = String.Empty;
        public String Script_OnStartingTest = String.Empty;
        public String Script_OnTestEnded = String.Empty;
        public String Script_OnLoadingFirstCollectionPage = String.Empty;
        public String Script_OnInit = String.Empty;
        public String Script_OnStartDocument = String.Empty;
        public String Script_OnStartHtml = String.Empty;
        public String Script_OnStartHead = String.Empty;
        public String Script_OnEndHead = String.Empty;
        public String Script_OnStartBody = String.Empty;
        public String Script_OnSegment = String.Empty;
        public String Script_OnEndBody = String.Empty;
        public String Script_OnEndHtml = String.Empty;
        public String Script_OnEndDocument = String.Empty;

        private Dictionary<String, CollectorsArgument> arguments = null;
        private List<CollectorsScript[]> scriptGroups = null;

        public CollectorsArgument GetArgument(String k)
        {
            if (String.IsNullOrEmpty(k) || arguments == null)
                return null;

            CollectorsArgument ea = null;
            
            arguments.TryGetValue(k, out ea);
            
            return ea;
        }
        public void RemoveArgument(String k)
        {
            if (String.IsNullOrEmpty(k) || arguments == null || arguments.ContainsKey(k) == false)
                return;

            arguments.Remove(k);
        }
        public void SetArgument(String k,String v)
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
    
        public void AddScriptsGroup(CollectorsScript[] scripts)
        {
            if(scripts == null || scripts.Length == 0)
                return;
            
            if(scriptGroups == null) 
                scriptGroups = new List<CollectorsScript[]>();

            scriptGroups.Add(scripts);
        }
        public CollectorsScript[] GetScriptsGroup(uint i)
        {
            if(scriptGroups == null || scriptGroups.Count == 0 || scriptGroups.Count <= i)
                return null;

            return scriptGroups[(int)i];
        }
        public int ScriptsGroupCount
        {
            get{
                return this.scriptGroups == null ? 0 : this.scriptGroups.Count;
            }
        }
        
        public CollectorsScript[] GetAllScripts()
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
        
        public String FormatCollectorsScript(CollectorsScript cs)
        {
            StringBuilder sb = new StringBuilder();

            if (String.IsNullOrEmpty(cs.IsFinished) == false) sb.AppendFormat(Script_IsFinished, cs.IsFinished);
            if (String.IsNullOrEmpty(cs.OnStartingTest) == false) sb.AppendFormat(Script_OnStartingTest, cs.OnStartingTest);
            if (String.IsNullOrEmpty(cs.OnTestEnded) == false) sb.AppendFormat(Script_OnTestEnded, cs.OnTestEnded);
            if (String.IsNullOrEmpty(cs.OnLoadingFirstCollectionPage) == false) sb.AppendFormat(Script_OnLoadingFirstCollectionPage, cs.OnLoadingFirstCollectionPage);
            if (String.IsNullOrEmpty(cs.OnInit) == false) sb.AppendFormat(Script_OnInit, cs.OnInit);
            if (String.IsNullOrEmpty(cs.OnStartDocument) == false) sb.AppendFormat(Script_OnStartDocument, cs.OnStartDocument);
            if (String.IsNullOrEmpty(cs.OnStartHtml) == false) sb.AppendFormat(Script_OnStartHtml, cs.OnStartHtml);
            if (String.IsNullOrEmpty(cs.OnStartHead) == false) sb.AppendFormat(Script_OnStartHead, cs.OnStartHead);
            if (String.IsNullOrEmpty(cs.OnEndHead) == false) sb.AppendFormat(Script_OnEndHead, cs.OnEndHead);
            if (String.IsNullOrEmpty(cs.OnStartBody) == false) sb.AppendFormat(Script_OnStartBody, cs.OnStartBody);
            if (String.IsNullOrEmpty(cs.OnSegment) == false) sb.AppendFormat(Script_OnSegment, cs.OnSegment);
            if (String.IsNullOrEmpty(cs.OnEndBody) == false) sb.AppendFormat(Script_OnEndBody, cs.OnEndBody);
            if (String.IsNullOrEmpty(cs.OnEndHtml) == false) sb.AppendFormat(Script_OnEndHtml, cs.OnEndHtml);
            if (String.IsNullOrEmpty(cs.OnEndDocument) == false) sb.AppendFormat(Script_OnEndDocument, cs.OnEndDocument);

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

    public interface ICollectorsConfigLoader{
        void LoadConfig(CollectorsConfig cc);
    }

    public class XMLCollectorsConfigLoader : ICollectorsConfigLoader
    {
        private String configFile = null;
        private Stream configStream = null;

        public XMLCollectorsConfigLoader(String config)
        {
            this.configFile = config;
        }
        public XMLCollectorsConfigLoader(Stream config)
        {
            this.configStream = config;
        }
        public XMLCollectorsConfigLoader()
        { 
            String config = Path.GetDirectoryName(Assembly.GetAssembly(typeof(XMLCollectorsConfigLoader)).Location) + "\\conf\\CollectorsConfig.Default.config";
            
            if (File.Exists(config))
            {
                this.configFile = config;
                return;
            }

            this.configStream = Assembly.GetAssembly(typeof(XMLCollectorsConfigLoader)).GetManifestResourceStream("MySpace.MSFast.Core.CollectorsConfig_Default.config");
        }

        public void LoadConfig(CollectorsConfig collectorsConfig)
        {
            try
            {
                if(String.IsNullOrEmpty(configFile) == false && File.Exists(configFile))
                {
                    configStream = File.Open(configFile,FileMode.Open);
                }
                
                if (configStream == null)
                    return;

                XmlDocument xml = new XmlDocument();

                try
                {
                    xml.Load(configStream);
                    ParseXML(xml, collectorsConfig);
                }
                catch
                {

                }

            }
            catch
            {
            }
            finally
            {
                try
                {
                    if (configStream != null)
                        configStream.Close();
                }
                catch
                {
                }
                configStream = null;
            }
        }

        private void ParseXML(XmlDocument configXml, CollectorsConfig collectorsConfig)
        {
            XmlNode config = configXml.ChildNodes[1];

            if (config != null)
            {
                foreach (XmlNode node in config.ChildNodes)
                {
                    if (node.Name.ToLower().Equals("global"))
                    {
                        ParseGlobalNode(node, collectorsConfig);
                    }
                    else if (node.Name.ToLower().Equals("collectorsgroups"))
                    {
                        ParseScriptsNode(node, collectorsConfig);
                    }
                }
            }
        }

        private void ParseScriptsNode(XmlNode node, CollectorsConfig collectorsConfig)
        {
            foreach (XmlNode collectorsgroup in node.ChildNodes)
            {
                if (collectorsgroup.Name.ToLower().Equals("collectorsgroup"))
                {
                    List<CollectorsScript> scripts = new List<CollectorsScript>();

                    foreach (XmlNode collector in collectorsgroup.ChildNodes)
                    {
                        if (collector.Name.ToLower().Equals("collector"))
                        {
                            XmlAttribute attr = collector.Attributes["name"];
                    
                            if(attr == null || String.IsNullOrEmpty(attr.Value))
                                continue;
                    
                            CollectorsScript cs = new CollectorsScript();
                            cs.ScriptName = attr.Value;

                            foreach (XmlNode xmlNd in collector.ChildNodes)
                            {
                                String n = xmlNd.Name.ToLower();

                                if (n.Equals("isfinished")) { cs.IsFinished = xmlNd.InnerText; }
                                else if (n.Equals("onstartingtest")) { cs.OnStartingTest = xmlNd.InnerText; }
                                else if (n.Equals("ontestended")) { cs.OnTestEnded = xmlNd.InnerText; }
                                else if (n.Equals("onloadingfirstcollectionpage")) { cs.OnLoadingFirstCollectionPage = xmlNd.InnerText; }
                                else if (n.Equals("oninit")) { cs.OnInit = xmlNd.InnerText; }
                                else if (n.Equals("onstartdocument")) { cs.OnStartDocument = xmlNd.InnerText; }
                                else if (n.Equals("onstarthtml")) { cs.OnStartHtml = xmlNd.InnerText; }
                                else if (n.Equals("onstarthead")) { cs.OnStartHead = xmlNd.InnerText; }
                                else if (n.Equals("onendhead")) { cs.OnEndHead = xmlNd.InnerText; }
                                else if (n.Equals("onstartbody")) { cs.OnStartBody = xmlNd.InnerText; }
                                else if (n.Equals("onsegment")) { cs.OnSegment = xmlNd.InnerText; }
                                else if (n.Equals("onendbody")) { cs.OnEndBody = xmlNd.InnerText; }
                                else if (n.Equals("onendhtml")) { cs.OnEndHtml = xmlNd.InnerText; }
                                else if (n.Equals("onenddocument")) { cs.OnEndDocument = xmlNd.InnerText; }
                            }

                            scripts.Add(cs);
                        }
                    }

                    collectorsConfig.AddScriptsGroup(scripts.ToArray());
                }
            }
        }

        private void ParseGlobalNode(XmlNode x, CollectorsConfig collectorsConfig)
        {
            foreach (XmlNode xmlNd in x)
            {
                String n = xmlNd.Name.ToLower();

                if (n.Equals("emptyhtml")) { collectorsConfig.EmptyHTML = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector")) { collectorsConfig.PageDataCollector = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_constructor")) { collectorsConfig.Constructor = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_oninit")) { collectorsConfig.Event_OnInit = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onloadingfirstcollectionpage")) { collectorsConfig.Event_OnLoadingFirstCollectionPage = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstartingtest")) { collectorsConfig.Event_OnStartingTest = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstartdocument")) { collectorsConfig.Event_OnStartDocument = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstarthtml")) { collectorsConfig.Event_OnStartHtml = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstarthead")) { collectorsConfig.Event_OnStartHead = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onendhead")) { collectorsConfig.Event_OnEndHead = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onstartbody")) { collectorsConfig.Event_OnStartBody = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onsegment")) { collectorsConfig.Event_OnSegment = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onendbody")) { collectorsConfig.Event_OnEndBody = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onendhtml")) { collectorsConfig.Event_OnEndHtml = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_event_onenddocument")) { collectorsConfig.Event_OnEndDocument = xmlNd.InnerText; }

                else if (n.Equals("pagedatacollector_script_isfinished")) { collectorsConfig.Script_IsFinished = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onstartingtest")) { collectorsConfig.Script_OnStartingTest = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_ontestended")) { collectorsConfig.Script_OnTestEnded = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onloadingfirstcollectionpage")) { collectorsConfig.Script_OnLoadingFirstCollectionPage = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_oninit")) { collectorsConfig.Script_OnInit = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onstartdocument")) { collectorsConfig.Script_OnStartDocument = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onstarthtml")) { collectorsConfig.Script_OnStartHtml = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onstarthead")) { collectorsConfig.Script_OnStartHead = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onendhead")) { collectorsConfig.Script_OnEndHead = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onstartbody")) { collectorsConfig.Script_OnStartBody = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onsegment")) { collectorsConfig.Script_OnSegment = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onendbody")) { collectorsConfig.Script_OnEndBody = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onendhtml")) { collectorsConfig.Script_OnEndHtml = xmlNd.InnerText; }
                else if (n.Equals("pagedatacollector_script_onenddocument")) { collectorsConfig.Script_OnEndDocument = xmlNd.InnerText; }
            }
        }
    }

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

        public void LoadConfig(CollectorsConfig cc)
        {

            if (String.IsNullOrEmpty(jsonString))
                return;

            JSONObjectCollectorsConfig ccjo = (JSONObjectCollectorsConfig)JsonConvert.Import(typeof(JSONObjectCollectorsConfig), jsonString);

            if (ccjo == null)
                return;

            if (ccjo.Arguments != null)
            {
                foreach(CollectorsArgument arg in ccjo.Arguments)
                {
                    if(arg == null || String.IsNullOrEmpty(arg.Key) == false)
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
}
