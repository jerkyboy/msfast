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
using BDika.Web.Core.Common.RequestHelpers;

namespace BDika.Web.Application.Common.RequestHelpers
{
    public class BDikaApplicationPageAttributesParser : BDikaPageAttributesParser
    {
        public override string GetNoUserDefaultPage()
        {
            return BDika.Web.Application._Default.GetURL();
        }

        public override string GetRegisteredUserDefaultPage()
        {
            return BDika.Web.Application._Default.GetURL();
        }
    }
}
