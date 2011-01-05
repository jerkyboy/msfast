using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;

namespace MySpace.MSFast.BuildTasks.NSIS
{
    public class WriteInstallFileInclude:BaseTask
    {
        [Required]
        public string ParentFolder { get; set; }
        [Required]
        public ITaskItem OutputFile { get; set; }
        [Required]
        public ITaskItem[] Files { get; set; }
        public bool Overwrite { get; set; }

        public override bool Execute()
        {
            try
            {
                Log.LogMessage("Writing {0}...", this.OutputFile);
                using (StreamWriter writer = new StreamWriter(OutputFile.ItemSpec, false, Encoding.ASCII))
                {
                    writer.WriteLine("#Generated {0}", DateTime.Now);

                    writer.WriteLine("CreateDirectory \"{0}\";", ParentFolder);
                    writer.WriteLine("SetOutPath \"{0}\";", ParentFolder);
                    if (Overwrite)
                        writer.WriteLine("SetOverwrite on");
                    writer.WriteLine();

                    foreach (ITaskItem file in Files)
                    {
                        string RecursiveDir = file.GetMetadata("RecursiveDir");
                        //Log.LogMessage("RecursiveDir = {0}", RecursiveDir);
                        Log.LogMessage(MessageImportance.Low, "Writing Install File {0}", file);
                        writer.WriteLine("File \"{0}\";", file);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex);
                return false;
            }

            return true;
        }
    }
}
