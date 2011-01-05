using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MySpace.MSFast.BuildTasks.NSIS
{
    public class MakeNSIS:ToolTask
    {
        protected override string ToolName
        {
            get { return "MakeNSIS"; }
        }
        public string NSISPath { get; set; }

        [Required]
        public ITaskItem Script { get; set; }
        public ITaskItem OutputPath { get; set; }
        [Output]
        public ITaskItem CompiledInstaller { get; set; }
        
        private bool ExtractEvents(string input, out string Key, out string Message)
        {
            try
            {
                int index=input.IndexOf(":");
                if (index == -1)
                {
                    Message = Key = string.Empty;
                    return false;
                }
                Key = input.Substring(0, index);
                Message = input.Substring(index + 1);
                Message = Regex.Replace(Message, "\"", string.Empty);

                return true;
            }
            catch(Exception ex)
            {
                Log.LogWarning(input);
                Log.LogWarningFromException(ex);
                Message=Key = string.Empty;
                return false;
            }
        }

        protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
        {
            string key, message=null;

            if(!ExtractEvents(singleLine, out key, out message))
            {
                return;
            }
            if (string.Equals("Output", key, StringComparison.OrdinalIgnoreCase))
            {
                Log.LogMessage("Output file = {0}", message);
                this.CompiledInstaller = new TaskItem(message.Trim());
            }
            else if (string.Equals("File", key, StringComparison.OrdinalIgnoreCase))
            {
                Log.LogMessage("Adding File {0}..", message);
            }
            else if (key.StartsWith("WriteReg", StringComparison.OrdinalIgnoreCase))
            {
                Log.LogMessage("Adding Registry Key {0}..", message);
            }
        }

        protected override string GenerateFullPathToTool()
        {
            if (string.IsNullOrEmpty(NSISPath))
            {
                Log.LogMessage(MessageImportance.Low, "NSISPath was not set so using default install path in program files.");
                return Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\NSIS\makensis.exe");
            }

            string toolPath = null;

            if (!NSISPath.EndsWith("makensis.exe", StringComparison.OrdinalIgnoreCase))
            {
                toolPath = System.IO.Path.Combine(NSISPath, "makensis.exe");
            }
            else
            {
                toolPath = NSISPath;
            }

            return System.IO.Path.GetFullPath(toolPath);
        }
        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilder builder = new CommandLineBuilder();
            if (null != this.Script)
            {
                string TextToAppend = string.Format("\"/xOutFile {0}\"", this.OutputPath);
                builder.AppendSwitch(TextToAppend);
            }
            builder.AppendFileNameIfNotNull(this.Script);
            LogToolCommand(builder.ToString());
            return builder.ToString();
        }

    }
}
