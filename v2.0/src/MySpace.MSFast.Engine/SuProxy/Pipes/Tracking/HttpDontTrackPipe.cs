using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.SuProxy.Pipes;
using MySpace.MSFast.Core.Http;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Tracking
{
    public class HttpDontTrackPipe : HttpPipe
    {
        public override void SendData(byte[] buffer, int offset, int length)
        {
            object ht = null;

            this.PipesChain.ChainState.TryGetValue(HttpTracerPipe.STATE_KEY, out ht);

            if(ht != null)
            {
                ((HttpTransaction)ht).IsTrackable = false;
            }

            base.SendData(buffer, offset, length);
        }
    }
}
