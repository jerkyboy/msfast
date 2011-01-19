using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Web.Context;

namespace BDika.Web.Core.Context
{
    public class BDikaCookieJar : CookieJar
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


