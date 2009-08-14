using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.DataProcessors;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using MySpace.MSFast.Core.Configuration.Common;

namespace MySpace.MSFast.MSFFiles
{
    public class MSFHandler
    {
        byte[] tmpBuffer = new byte[4096];

        private static readonly Regex HeaderRegex = new Regex("MSFAST([0-9]{4})([0-9]{1,5})(MSFastPerfDump_[0-9]{1,5})", RegexOptions.Compiled);

        public ProcessedDataPackage LoadProcessedDataPackage(Stream fileStream)
        {
            if (fileStream == null)
            {
                throw new NullReferenceException();
            }

            string tempFolder = Path.GetTempPath();

            if (String.IsNullOrEmpty(tempFolder) || Directory.Exists(tempFolder) == false)
            {
                throw new DirectoryNotFoundException("Invalid temp folder " + tempFolder);
            }

            GZipStream gzipStream = null;

            int collectionId = -1;

            try
            {
                gzipStream = new GZipStream(fileStream, CompressionMode.Decompress, true);

                collectionId = Decompress(tempFolder, gzipStream);
            }
            finally
            {                
                if (gzipStream != null)
                {
                    gzipStream.Close();
                    gzipStream.Dispose();
                }
            }
            
            if(collectionId == -1)
            {
                return null;
            }

            return ProcessedDataCollector.CollectAll(tempFolder, collectionId);
        }

        public bool SaveProcessedDataPackage(Stream msfFilestream, ProcessedDataPackage pacakge)
        {
            GZipStream gzipStream = null;

            try
            {
                gzipStream = new GZipStream(msfFilestream, CompressionMode.Compress, true);

                return Compress(gzipStream, pacakge);
            }
            finally
            {
                if (gzipStream != null)
                {
                    gzipStream.Close();
                    gzipStream.Dispose();
                }                
            }
        }

        private int Decompress(string folder, Stream instream)
        {

            if (String.IsNullOrEmpty(folder) || Directory.Exists(folder) == false)
                return -1;

            folder = folder.Replace("\\", "/");

            if (folder.EndsWith("/") == false)
                folder += "/";

            String header = null;
            String filename = null;

            int read = 0;
            int read2 = 0;

            header = ReadString(instream);

            if (String.IsNullOrEmpty(header))
                return -1;

            Match headerMatch = HeaderRegex.Match(header);

            if (headerMatch == null || headerMatch.Success == false)
                return -1;

            int collectionId = -1;
            String outputFolder = null;
            String version = null;

            version = headerMatch.Groups[1].Value;
            collectionId = int.Parse(headerMatch.Groups[2].Value);
            outputFolder = headerMatch.Groups[3].Value;

            folder += outputFolder + "/";
            
            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);

            while (true)
            {
                filename = ReadString(instream);

                if (String.IsNullOrEmpty(filename))
                    return collectionId;

                FileStream fs = File.Open(folder + filename, FileMode.Create);

                read = ReadInt(instream);

                if (read < 0)
                    return -1;

                while ((read -= (read2 = instream.Read(tmpBuffer, 0, Math.Min(read, tmpBuffer.Length)))) >= 0 && read2 != 0)
                {
                    fs.Write(tmpBuffer, 0, read2);
                }

                fs.Flush();
                fs.Close();
                fs.Dispose();

                if (instream.ReadByte() != 0 || instream.ReadByte() != 0)
                    return -1;
            }

            return collectionId;
        }

        private string ReadString(Stream instream)
        {
            int read = ReadInt(instream);

            if (read < 0)
                return null;

            if (instream.Read(tmpBuffer, 0, read) != read)
                return null;

            String s = Encoding.UTF8.GetString(tmpBuffer, 0, read);

            if (instream.ReadByte() != 0 || instream.ReadByte() != 0)
                return null;

            return s;
        }

        private int ReadInt(Stream instream)
        {
            int read = instream.ReadByte();
            read = (read << 8) | instream.ReadByte();
            read = (read << 8) | instream.ReadByte();
            read = (read << 8) | instream.ReadByte();

            return read;
        }

        private bool WriteString(Stream outstream, String strng)
        {
            try
            {
                byte[] b = Encoding.UTF8.GetBytes(strng);

                WriteInt(outstream, b.Length);

                outstream.Write(b, 0, b.Length);

                outstream.WriteByte(0);
                outstream.WriteByte(0);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool WriteInt(Stream outstream, int nt)
        {
            try
            {
                outstream.WriteByte((byte)(nt >> 24));
                outstream.WriteByte((byte)(nt >> 16));
                outstream.WriteByte((byte)(nt >> 8));
                outstream.WriteByte((byte)(nt));
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool Compress(Stream outstream, ProcessedDataPackage package)
        {

            if (package == null)
                return false;

            ProcessedDataPackageDumpFilesInfo pdpdfi = new ProcessedDataPackageDumpFilesInfo(package);

            String collectFrom = pdpdfi.GetFolderNameAndCheckIfValid();

            if (String.IsNullOrEmpty(collectFrom) || Directory.Exists(collectFrom) == false)
                return false;

            DirectoryInfo di = new DirectoryInfo(collectFrom);

            FileInfo[] compressThis = di.GetFiles();

            byte[] tmpBuffer2 = new byte[14];
            byte[] tmpBuffer3 = new byte[4096];

            int read = 0;

            Stream instream = null;

            if (WriteString(outstream, "MSFAST0001" + package.CollectionID + String.Format(DumpFileInfo.DUMP_FOLDER_PATTERN,package.CollectionID)) == false)
                return false;

            foreach (FileInfo compress in compressThis)
            {
                instream = compress.Open(FileMode.Open, FileAccess.Read);

                if (WriteString(outstream, compress.Name) == false) return false;
                if (WriteInt(outstream, (int)compress.Length) == false) return false;

                while ((read = instream.Read(tmpBuffer3, 0, tmpBuffer3.Length)) != 0)
                {
                    outstream.Write(tmpBuffer3, 0, read);
                }

                instream.Close();
                instream.Dispose();

                outstream.WriteByte(0);
                outstream.WriteByte(0);
            }

            outstream.Flush();
            outstream.Close();
            outstream.Dispose();

            return true;
        }


    }

}
