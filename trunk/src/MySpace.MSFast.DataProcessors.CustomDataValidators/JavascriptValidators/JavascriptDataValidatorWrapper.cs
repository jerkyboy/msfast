using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.DataValidators;
using MySpace.MSFast.ImportExportsMgrs;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using MySpace.MSFast.Core.Logger;
using MySpace.MSFast.DataProcessors.CustomDataValidators.JavascriptValidators.JSShellOutputReader;

namespace MySpace.MSFast.DataProcessors.CustomDataValidators.JavascriptValidators
{
    public class JavascriptDataValidatorWrapper : IDataValidator
    {
        private static readonly MSFastLogger log = MSFastLogger.GetLogger(typeof(JavascriptDataValidatorWrapper));

        private const String JSON_FORMAT = "\"{0}\":\"{1}\"";
        private const String HAR_VAR = "var processedDataPackage = {0};\r\n";


        #region IDataValidator Impl
        private String _helpURL = "";
        private String _description = "";
        private String _name = "";
        private String _groupName = "";

        public String HelpURL
        {
            get { return this._helpURL; }
            set { this._helpURL = value; }
        }
        public String GroupName
        {
            get { return this._groupName; }
            set { this._groupName = value; }
        }
        public String Description
        {
            get { return this._description; }
            set { this._description = value; }
        }
        public String Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        #endregion

        private String script = null;
        private String data = null;
        private JavascriptOutputReader JavascriptOutputReader = null;
 
        public virtual void Dispose()
        {
            this._description = null;
            this._name = null;
            this._helpURL = null;
            this.script = null;
            this.data = null;
        }

        public virtual void Init(Dictionary<string, string> config)
        {
            script = config["script"];

            if (config.Count > 1)
            {
                StringBuilder addtlData = new StringBuilder();

                foreach (String k in config.Keys)
                {
                    if (k.Trim().ToLower().Equals("script") == false)
                    {
                        if (addtlData.Length > 0) addtlData.Append(',');
                        addtlData.Append(String.Format(JSON_FORMAT, k, config[k]));
                    }
                }
                data = addtlData.ToString();
            }
        }

        public IValidationResults Validate(ProcessedDataPackage package)
        {
            String jsshellExecutable = GetJSShellExecutable();

            if (String.IsNullOrEmpty(jsshellExecutable) || File.Exists(jsshellExecutable) == false) return null;

            if (package == null) return null;
            
            if (package.ContainsKey(typeof(HARCache)) == false){
                HARCache c = GenerateHAR(package);
                if(c == null || String.IsNullOrEmpty(c.HARJSONObject)) return null;
                package.Add(typeof(HARCache), c);
            }

            HARCache har = package[typeof(HARCache)] as HARCache;

            if (har == null) return null;

            String tmpFilename = GenerateTmpFilename();

            if (String.IsNullOrEmpty(tmpFilename)) return null;

            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(File.Open(tmpFilename, FileMode.Create));

                sw.Write(har.HARJSONObject);
                sw.Write(script);
                sw.Write(String.Format("var EXTDATA = {{{0}}};\r\n",data));
                sw.Write(String.Format("var BASE_FOLDER = \"{0}\";\r\n",GetBaseFolder()));
                sw.Write("load(BASE_FOLDER + \"/JSTemplate/JavascriptValidator.js\");\r\n");

                sw.Flush();
                sw.Close();

                this.JavascriptOutputReader = new JavascriptOutputReader(package);

                ProcessStartInfo psi = new ProcessStartInfo(jsshellExecutable);
                psi.CreateNoWindow = true;
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                psi.UseShellExecute = false;
                psi.Arguments = String.Format("\"{0}\"", tmpFilename);
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                Process p = new Process();
                p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
                p.StartInfo = (psi);
                p.Start();
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();
                p.WaitForExit();

                return this.JavascriptOutputReader.GetResults();

            }
            catch(Exception e)
            {

                if (log.IsErrorEnabled)
                    log.Error("Javascript Validator Error", e);
            }
            finally
            {
                if (sw != null) sw.Close();
                
                this.JavascriptOutputReader = null;
                
                if (File.Exists(tmpFilename))
                {
                    try
                    {
                        File.Delete(tmpFilename);
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }


        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if(log.IsErrorEnabled)
                log.Error(String.Format("JSShell Error: {0}", e.Data));
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e != null && String.IsNullOrEmpty(e.Data) == false && this.JavascriptOutputReader != null)
            {
                this.JavascriptOutputReader.OnData(e.Data);
            }
        }

        private string GenerateTmpFilename()
        {
            String folder = null;

            String fn = Guid.NewGuid().ToString().Replace("-", "") + Guid.NewGuid().ToString().Replace("-", "") + ".js";

            try
            {
                folder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(JavascriptDataValidatorWrapper)).Location).Replace("\\", "/");
                if (folder.EndsWith("/") == false) folder += "/";
                folder += fn;
            }
            catch
            {
            }

            return folder;
        }
        private string GetJSShellExecutable()
        {
            String folder = null;

            try
            {
                folder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(JavascriptDataValidatorWrapper)).Location).Replace("\\","/");
                if (folder.EndsWith("/") == false) folder += "/";
                folder += "JavascriptValidators/JSShell/jsshell.exe";
            }
            catch
            {
            }
            
            return folder;
        }
        private string GetBaseFolder()
        {
            String folder = null;

            try
            {
                folder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(JavascriptDataValidatorWrapper)).Location).Replace("\\", "/");
                if (folder.EndsWith("/") == false) folder += "/";
                folder += "JavascriptValidators";
            }
            catch
            {
            }

            return folder;
        }

        private HARCache GenerateHAR(ProcessedDataPackage package)
        {
            HARImportExportsManager i = new HARImportExportsManager();
            MemoryStream ms = new MemoryStream();
            
            try
            {
                if (i.SaveProcessedDataPackage(ms, package))
                {
                    HARCache c = new HARCache();
                    c.HARJSONObject = String.Format(HAR_VAR,Encoding.UTF8.GetString(ms.ToArray()));
                    return c;
                }
            }
            catch{

            }
            finally{
                ms.Close();
                ms.Dispose();
            }

            return null;
        }

        private class HARCache : ProcessedData
        {
            public String HARJSONObject = null;
        }
    }
}
