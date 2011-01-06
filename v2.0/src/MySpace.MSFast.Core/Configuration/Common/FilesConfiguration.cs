using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MySpace.MSFast.Core.Configuration.Common
{
    public abstract class DumpFileInfo
    {
        public readonly static String DUMP_FOLDER_PATTERN = "MSFastPerfDump_{0}";
        private String filenamePattern;
        protected CollectionMetaInfo collectionMetaInfo;

        public DumpFileInfo(CollectionMetaInfo collectionMetaInfo , String filenamePattern)
        {
            this.collectionMetaInfo = collectionMetaInfo;
            this.filenamePattern = filenamePattern;
        }

        protected bool Exist(params object[] args)
        {
            String filename = GetFullPath(args);

            if (filename == null)
                return false;
            
            return File.Exists(filename);
        }

        protected Stream Open(FileAccess fa, params object[] args)
        {
            String folder = GetFolderNameAndCheckIfValid();

            if (folder == null)
                return null;

            if ((fa & FileAccess.Write) == FileAccess.Write)
            {
                folder = GetOrCreateCollectionFolder();
            }

            String filename = GetFullPath(args);

            if (filename == null)
                return null;

            try
            {
                if ((fa & FileAccess.Write) == FileAccess.Write)
                {
                    return new FileStream(filename,FileMode.OpenOrCreate, fa);
                }else{
                    return new FileStream(filename, FileMode.Open, fa);
                }
            }
            catch 
            {
            
            }
            return null;
        }

        protected String GetFullPath(params object[] args)
        {
            String folder = GetFolderNameAndCheckIfValid();
            if (folder == null)
                return null;

            return folder + GetFilename(args);
        }

        protected String GetFilename(params object[] args)
        {

            if (args == null || args.Length == 0)
                return (this.filenamePattern);
            else
                return (String.Format(this.filenamePattern, args));
        }

        public String GetFolderNameAndCheckIfValid()
        {
            if (this.collectionMetaInfo == null ||
               String.IsNullOrEmpty(this.collectionMetaInfo.DumpFolder) ||
               Directory.Exists(this.collectionMetaInfo.DumpFolder) == false)
            {
                return null;
            }

            String s = this.collectionMetaInfo.DumpFolder.Replace("\\", "/");
            
            if (s.EndsWith("/") == false)
                s += "/";

            s += GetFolderName() + "/";
            
            return s;
        }

        public String GetFolderName()
        {
            return String.Format(DUMP_FOLDER_PATTERN, collectionMetaInfo.CollectionID);
        }

        private String GetOrCreateCollectionFolder()
        {
            String fn = GetFolderNameAndCheckIfValid();
            if (fn == null)
                return null;

            if (Directory.Exists(fn))
            {
                return fn;
            }

            try
            {
                Directory.CreateDirectory(fn);
                return fn;
            
            }catch{
            }
            return null;
        }
    }

    public abstract class OnlyCollectionIDDumpFileInfo : DumpFileInfo
    {

        public OnlyCollectionIDDumpFileInfo(CollectionMetaInfo collectionMetaInfo, String filenamePattern):
            base(collectionMetaInfo, filenamePattern)
        {
        }

        public Stream Open(FileAccess fa)
        {
            if (collectionMetaInfo == null || 
                collectionMetaInfo.DumpFolder == null)
                return null;

            return base.Open(fa, collectionMetaInfo.CollectionID);
        }

        public bool Exist()
        {
            if (collectionMetaInfo == null || 
                collectionMetaInfo.DumpFolder == null)
                return false;

            return base.Exist(collectionMetaInfo.CollectionID);
        }

        public String GetFullPath()
        {
            if (collectionMetaInfo == null || 
                collectionMetaInfo.DumpFolder == null)
                return null;

            return base.GetFullPath(collectionMetaInfo.CollectionID);
        }

        public String GetFilename()
        {
            if (collectionMetaInfo == null || 
                collectionMetaInfo.DumpFolder == null)
                return null;

            return base.GetFilename(collectionMetaInfo.CollectionID);
        }
    }

    public abstract class CollectionIDAndSingleKeyDumpFileInfo : DumpFileInfo
    {
        public CollectionIDAndSingleKeyDumpFileInfo(CollectionMetaInfo collectionMetaInfo, string filenamePattern)
            : base(collectionMetaInfo, filenamePattern)
        {
        }

        public Stream Open(FileAccess fa,String key)
        {
            if (collectionMetaInfo == null || 
                collectionMetaInfo.DumpFolder == null ||
                String.IsNullOrEmpty(key))
                return null;

            return base.Open(fa, collectionMetaInfo.CollectionID, key);
        }

        public bool Exist(String key)
        {
            if (collectionMetaInfo == null ||
                collectionMetaInfo.DumpFolder == null ||
                String.IsNullOrEmpty(key))
                return false;

            return base.Exist(collectionMetaInfo.CollectionID, key);
        }

        public String GetFullPath(String key)
        {
            if (collectionMetaInfo == null ||
                collectionMetaInfo.DumpFolder == null ||
                String.IsNullOrEmpty(key))
                return null;

            return base.GetFullPath(collectionMetaInfo.CollectionID, key);
        }

        public String GetFilename(String key)
        {
            if (collectionMetaInfo == null ||
                collectionMetaInfo.DumpFolder == null ||
                String.IsNullOrEmpty(key))
                return null;

            return base.GetFilename(collectionMetaInfo.CollectionID, key);
        }
    }

    public class ProcessedDataPackageDumpFilesInfo : DumpFileInfo
    {
        public ProcessedDataPackageDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "")
        {
        }
    }

    public class ResponseHeaderDumpFilesInfo : CollectionIDAndSingleKeyDumpFileInfo
    {
        public ResponseHeaderDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "H{1}")
        {            
        }        
    }
    public class ResponseBodyDumpFilesInfo : CollectionIDAndSingleKeyDumpFileInfo
    {
        public ResponseBodyDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "B{1}")
        {

        }
    }
    
    public class ScreenshotsDumpFilesInfo : CollectionIDAndSingleKeyDumpFileInfo
    {
        public static String ScreenShotsFilePattern = "TC_{0}_*.jpg";

        public ScreenshotsDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "TC_{0}_{1}.jpg")
        {

        }

        public int FilesCount()
        {
            FileInfo[] f = GetFiles();
            if(f == null) return 0;
            return f.Length;
        }

        public FileInfo[] GetFiles()
        {
            if (collectionMetaInfo == null ||
                collectionMetaInfo.DumpFolder == null)
                return null;

            String folder = GetFolderNameAndCheckIfValid();
            if (folder == null)
                return null;

            DirectoryInfo di = new DirectoryInfo(folder);
            return di.GetFiles(String.Format(ScreenShotsFilePattern, collectionMetaInfo.CollectionID));
        }
    }

    public class DownloadDumpFilesInfo : OnlyCollectionIDDumpFileInfo
    {
        public DownloadDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "download_{0}.dat")
        {

        }
    }
    public class SerializedResultsFilesInfo : OnlyCollectionIDDumpFileInfo
    {
        public SerializedResultsFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "serializedResults{0}.xml")
        {

        }
    }
    public class SourceDumpFilesInfo : OnlyCollectionIDDumpFileInfo
    {
        public SourceDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "source_{0}.src")
        {

        }        
    }
    public class BrokenSourceDumpFilesInfo : OnlyCollectionIDDumpFileInfo
    {
        public BrokenSourceDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "source_{0}.src_b")
        {

        }       
    }
    /*
    public class RenderDumpFilesInfo : OnlyCollectionIDDumpFileInfo
    {
        public RenderDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "renderdump_{0}.dat")
        {

        }
    }
    */
    public class MarkersDumpFilesInfo : OnlyCollectionIDDumpFileInfo
    {
        public MarkersDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "markers_{0}.dat")
        {

        }
    }
    public class PerformanceDumpFilesInfo : OnlyCollectionIDDumpFileInfo
    {
        public PerformanceDumpFilesInfo(CollectionMetaInfo collectionMetaInfo)
            : base(collectionMetaInfo, "perfdump_{0}.dat")
        {

        }
    }
}
