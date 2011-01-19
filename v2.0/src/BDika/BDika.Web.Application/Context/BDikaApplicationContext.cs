using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BDika.Web.Core.Context;

namespace BDika.Web.Application.Context
{
    public class BDikaApplicationContext : BDikaContext
    {
        public override String GetNextPage(String nextp)
        {            
            return BDika.Web.Application._Default.GetURL();
        }
    }
}
