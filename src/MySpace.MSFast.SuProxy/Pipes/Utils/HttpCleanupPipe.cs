﻿//=======================================================================
/* Project: MSFast (MySpace.MSFast.SuProxy)
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
using MySpace.MSFast.Core.Logger;

namespace MySpace.MSFast.SuProxy.Pipes.Utils
{
	public class HttpCleanupPipe : HttpPipe
	{
        private static readonly MSFastLogger log = MSFastLogger.GetLogger(typeof(HttpCleanupPipe));

		public override void StartReceive()
		{
			if (IsClosed) return;
			base.StartReceive();
			Cleanup();
		}

		public override void HandleError(Exception e)
		{
            if (log.IsErrorEnabled) log.Error("SuProxy Error ", e);
			base.HandleError(e);
			Cleanup();
		}

		private void Cleanup()
		{
			this.PipesChain.ChainState.HttpProxlet.Release();
		}
	}
}
