using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Web.Common.RequestHelpers;
using System.Web;
using BDika.Web.Core.Context;
using BDika.Entities.Users;
using EYF.Web.Common;

namespace BDika.Web.Core.Common.RequestHelpers
{
    public abstract class BDikaPageAttributesParser : PageAttributesParser
    {
        public bool ParsePageAttributes(HttpContext context)
        {
            BDikaContext cc = BDikaContext.Current;

            if (cc.PageAttributes != null)
            {
                IBDikaUser user = cc.User;

                if ((cc.PageAttributes is BDikaPageAttributes) && IsTestUserAttributesAllowed((BDikaPageAttributes)cc.PageAttributes, user))
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

        private bool IsTestUserAttributesAllowed(BDikaPageAttributes ppl, IBDikaUser user)
        {
            if (ppl.IsRequieredUserAttributesSet == false && ppl.IsRequieredMissingUserAttributesSet == false)
            {
                return true;
            }

            if (ppl.IsRequieredUserAttributesSet && (user.UserAttributes & ppl.RequieredUserAttributes) != ppl.RequieredUserAttributes)
            {
                return false;
            }

            if (ppl.IsRequieredUserPermissionsSet && (user.UserPermissions & ppl.RequieredUserPermissions) != ppl.RequieredUserPermissions)
            {
                return false;
            }

            if (ppl.IsRequieredMissingUserAttributesSet)
            {
                foreach (UserAttributes at in Enum.GetValues(typeof(UserAttributes)))
                {
                    if ((user.UserAttributes & ppl.RequieredMissingUserAttributes) == ppl.RequieredMissingUserAttributes)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


    }
}
