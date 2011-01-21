using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BDika.Entities.Results;
using EYF.Web.Common;
using BDika.Providers.Results;

namespace BDika.Web.Application.Controls.Results
{
    public partial class ResultsGraph : BaseControl<ResultsGraph>
    {
        public ResultsID ResultsID;
        public String RESULTS_URL;

        protected void Page_Load(object sender, EventArgs e)
        {

            RESULTS_URL = ResultsProvider.GetResultsXMLContextLocation(this.ResultsID);
        }
    }
}