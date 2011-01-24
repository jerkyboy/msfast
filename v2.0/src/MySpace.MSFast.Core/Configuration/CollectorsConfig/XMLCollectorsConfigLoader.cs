using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;

namespace MySpace.MSFast.Core.Configuration.CollectorsConfig
{
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
                if (String.IsNullOrEmpty(configFile) == false && File.Exists(configFile))
                {
                    configStream = File.Open(configFile, FileMode.Open);
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

                            if (attr == null || String.IsNullOrEmpty(attr.Value))
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
                                else if (n.Equals("onreadystate")) { cs.OnReadyState = xmlNd.InnerText; }
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

                if (n.Equals("emptyhtml")) { collectorsConfig.SetArgument("EmptyHTML",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector")) { collectorsConfig.SetArgument("PageDataCollector",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_constructor")) { collectorsConfig.SetArgument("Constructor",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_oninit")) { collectorsConfig.SetArgument("Event_OnInit",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onloadingfirstcollectionpage")) { collectorsConfig.SetArgument("Event_OnLoadingFirstCollectionPage",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onstartingtest")) { collectorsConfig.SetArgument("Event_OnStartingTest",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onstartdocument")) { collectorsConfig.SetArgument("Event_OnStartDocument",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onstarthtml")) { collectorsConfig.SetArgument("Event_OnStartHtml",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onstarthead")) { collectorsConfig.SetArgument("Event_OnStartHead",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onendhead")) { collectorsConfig.SetArgument("Event_OnEndHead",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onstartbody")) { collectorsConfig.SetArgument("Event_OnStartBody",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onsegment")) { collectorsConfig.SetArgument("Event_OnSegment",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onendbody")) { collectorsConfig.SetArgument("Event_OnEndBody",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onendhtml")) { collectorsConfig.SetArgument("Event_OnEndHtml",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_event_onenddocument")) { collectorsConfig.SetArgument("Event_OnEndDocument",xmlNd.InnerText);}

                else if (n.Equals("pagedatacollector_script_isfinished")) { collectorsConfig.SetArgument("Script_IsFinished",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onstartingtest")) { collectorsConfig.SetArgument("Script_OnStartingTest",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_ontestended")) { collectorsConfig.SetArgument("Script_OnTestEnded",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onloadingfirstcollectionpage")) { collectorsConfig.SetArgument("Script_OnLoadingFirstCollectionPage",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_oninit")) { collectorsConfig.SetArgument("Script_OnInit",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onstartdocument")) { collectorsConfig.SetArgument("Script_OnStartDocument",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onstarthtml")) { collectorsConfig.SetArgument("Script_OnStartHtml",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onstarthead")) { collectorsConfig.SetArgument("Script_OnStartHead",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onendhead")) { collectorsConfig.SetArgument("Script_OnEndHead",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onstartbody")) { collectorsConfig.SetArgument("Script_OnStartBody",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onsegment")) { collectorsConfig.SetArgument("Script_OnSegment",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onendbody")) { collectorsConfig.SetArgument("Script_OnEndBody",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onendhtml")) { collectorsConfig.SetArgument("Script_OnEndHtml",xmlNd.InnerText);}
                else if (n.Equals("pagedatacollector_script_onenddocument")) { collectorsConfig.SetArgument("Script_OnEndDocument", xmlNd.InnerText); }
                else if (n.Equals("pagedatacollector_script_onreadystate")) { collectorsConfig.SetArgument("Script_OnReadyState", xmlNd.InnerText); }
                
            }
        }
    }

}
