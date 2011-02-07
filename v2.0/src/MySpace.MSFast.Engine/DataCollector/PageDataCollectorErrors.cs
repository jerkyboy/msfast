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

namespace MySpace.MSFast.Engine.DataCollector
{
		public enum PageDataCollectorErrors : int
		{
			NoError								= 0,
			CantSaveTempFile				    = 0x800000,
			ObjectDisposed						= 0x800001,
			InvalidOrMissingArguments           = 0x800002,
			TestTimeout							= 0x800003,
			TestAlreadyRunning				    = 0x800004,
			CantStartBrowser					= 0x800005,
			CantGetBrowserProcessID	            = 0x800006,
			Unknown								= 0x800007,
            TestAborted							= 0x800008,
			CantSetProxy						= 0x800009,
			CantStartProxy						= 0x800010,
			InvalidConfiguration				= 0x800011,
            InvalidEngineLocation               = 0x800012,
		}
}