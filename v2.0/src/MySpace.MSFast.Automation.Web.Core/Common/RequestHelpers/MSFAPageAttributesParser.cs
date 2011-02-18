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
using EYF.Web.Common.RequestHelpers;
using System.Web;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Entities.Users;
using EYF.Web.Common;

namespace MySpace.MSFast.Automation.Web.Core.Common.RequestHelpers
{
    public abstract class MSFAPageAttributesParser : PageAttributesParser
    {
        public bool ParsePageAttributes(HttpContext context)
        {
            MSFAContext cc = MSFAContext.Current;

            if (cc.PageAttributes != null)
            {
                IMSFAUser user = cc.User;

                if ((cc.PageAttributes is MSFAPageAttributes) && IsTestUserAttributesAllowed((MSFAPageAttributes)cc.PageAttributes, user))
                {
                    return true;
                }

                if (context.Handler is BasePage)
                {
                    String next = ((BasePage)context.Handler).FailedUserArgumentsTarget();
                    if (String.IsNullOrEmpty(next) == false)
                    {
                        context.Response.Redirect(next);
                        return false;
                    }
                }

                if (user.HasAttribute(UserAttributes.IsRegisteredUser))
                {
                    context.Response.Redirect(GetRegisteredUserDefaultPage());
                }
                else
                {
                    cc.NextPage = context.Request.Url.PathAndQuery;

                    try
                    {
                        context.Response.Redirect(GetNoUserDefaultPage());
                    }
                    catch
                    {
                    }

                }
                return false;
            }
            return true;
        }

        public abstract string GetNoUserDefaultPage();
        public abstract string GetRegisteredUserDefaultPage();

        private bool IsTestUserAttributesAllowed(MSFAPageAttributes ppl, IMSFAUser user)
        {
            return true;
        }
    }
}
