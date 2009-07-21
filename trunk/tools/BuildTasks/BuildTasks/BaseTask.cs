using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;


namespace MySpace.MSFast.BuildTasks
{
    class CommentNotFoundException : Exception
    {
        public CommentNotFoundException(string comment, ITaskItem script)
        {
            this.Comment = comment;
            this.Script = script;
        }

        public string Comment { get; private set; }
        public ITaskItem Script { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format("Could not find comment \"{0}\" in script \"{1}\"", this.Comment, this.Script);
            }
        }
    }

    public abstract class BaseTask:Task
    {
        protected List<string> fileToList(ITaskItem ScriptFile)
        {
            if (null == ScriptFile) throw new ArgumentNullException("ScriptFile", "ScriptFile cannot be null.");
            Log.LogMessage("Reading {0}...", ScriptFile);
            List<string> lines = new List<string>();
            lines.AddRange(File.ReadAllLines(ScriptFile.ItemSpec));
            return lines;
        }

        protected int FindComment(List<string> lines, ITaskItem ScriptFile, string Comment)
        {
            if (null == ScriptFile) throw new ArgumentNullException("ScriptFile", "ScriptFile cannot be null.");
            if (null == lines) throw new ArgumentNullException("lines", "lines cannot be null.");
            if (string.IsNullOrEmpty(Comment)) throw new ArgumentNullException("Comment", "Comment cannot be null or empty.");

            bool foundComment = false;
            int index = 0;
            while (!foundComment && index < lines.Count)
            {
                string line = lines[index];

                if (line.IndexOf(Comment, StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    index++;
                    foundComment = true;
                    break;
                }
                else
                    index++;
            }

            if (!foundComment)
                throw new CommentNotFoundException(Comment, ScriptFile);

            return index;
        }

        protected void SaveScript(ITaskItem ScriptFile, List<string> lines)
        {
            if (null == ScriptFile) throw new ArgumentNullException("ScriptFile", "ScriptFile cannot be null.");
            if (null == lines) throw new ArgumentNullException("lines", "lines cannot be null.");

            FileInfo info = new FileInfo(ScriptFile.ItemSpec);

            if (info.IsReadOnly)
                info.IsReadOnly = false;

            Log.LogMessage("Saving {0}...", ScriptFile);
            File.WriteAllLines(ScriptFile.ItemSpec, lines.ToArray());
        }

    }
}
