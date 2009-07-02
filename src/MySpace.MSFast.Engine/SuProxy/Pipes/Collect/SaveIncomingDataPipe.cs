//=======================================================================
/* Project: MSFast (MySpace.MSFast.Engine)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.SuProxy.Pipes;
using System.IO;
using MySpace.MSFast.Engine.SuProxy.Pipes.Tracking;
using MySpace.MSFast.SuProxy.Pipes.Utils;
using MySpace.MSFast.Core.Http;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Collect
{
    public class SaveIncomingDataPipe : HttpBreakerPipe
    {
        private BinaryWriter outStreamBody;
        private BinaryWriter outStreamHeader;

        public static String FILE_KEY = "SaveIncomingDataPipe_FileKey";

        public override void SendHeader(String header)
        {
            base.SendHeader(header);

            if (this.PipesChain.ChainState.ContainsKey(HttpTracerPipe.STATE_KEY) == false)
                return;

            byte[] b = Encoding.ASCII.GetBytes(header);
            SaveCache(b, 0, b.Length, false);
        }

        public override void SendBodyData(byte[] buffer, int offset, int length)
        {
            base.SendBodyData(buffer, offset, length);

            if (this.PipesChain.ChainState.ContainsKey(HttpTracerPipe.STATE_KEY) == false)
                return;

            SaveCache(buffer, offset, length, true);
        }

        private void SaveCache(byte[] buffer, int offset, int length, bool isBody)
        {
            if (this.PipesChain.ChainState.ContainsKey(HttpTracerPipe.STATE_KEY) == false)
                return;

            if ( (outStreamBody == null || outStreamHeader == null) && this.Configuration.ContainsKey("DumpFolder"))
            {
                try
                {
                    String filenameBody = null;
                    String filenameHeader = null;
                    String guid = null;
                    
                    do
                    {
                        guid = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                        filenameBody = String.Format("{0}\\B{1}", this.Configuration["DumpFolder"], guid);
                        filenameHeader = String.Format("{0}\\H{1}", this.Configuration["DumpFolder"], guid);

                    } while (File.Exists(filenameBody) && File.Exists(filenameHeader));

                    outStreamBody = new BinaryWriter(File.Create(filenameBody));
                    outStreamHeader = new BinaryWriter(File.Create(filenameHeader));

                    HttpTransaction httpTranc = (HttpTransaction)this.PipesChain.ChainState[HttpTracerPipe.STATE_KEY];
                    httpTranc.FileGUID = guid;
                }
                catch
                {
                }
            }
            try
            {
                if (isBody && outStreamBody != null)
                {
                    outStreamBody.Write(buffer, offset, length);
                }
                else if (outStreamHeader != null)
                {
                    outStreamHeader.Write(buffer, offset, length);
                }
            }
            catch 
            {
            
            }
        }

        public override void Flush()
        {
            base.Flush();
            MakeSureFlushedAndClosed();
        }

        public override void Close()
        {
            base.Close();
            MakeSureFlushedAndClosed();
        }

        private void MakeSureFlushedAndClosed()
        {
            CloseStream(outStreamBody);
            CloseStream(outStreamHeader);
            
            outStreamBody = null;
            outStreamHeader = null;
        }

        private void CloseStream(BinaryWriter stream)
        {
            if (stream != null)
                try{
                    if (stream.BaseStream != null)
                    {
                        if (stream.BaseStream.CanWrite)
                            try { stream.Flush(); }
                            catch { }
                        try { stream.Close(); }
                        catch { }
                    }
                }
                catch{
                }
         }
    }

}
