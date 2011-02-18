//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Web.Core)
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
using System.Linq;
using System.Text;
using EYF.Web.Context;

namespace MySpace.MSFast.Automation.Web.Core.Context
{
    public class MSFACookieJar : CookieJar
    {
        public static String EncryptedWebUser
        {
            get
            {
                return CookieJar.GetCookie("u");
            }
            set
            {
                CookieJar.SetCookie("u", 0, value);
            }
        }        
    }
}


