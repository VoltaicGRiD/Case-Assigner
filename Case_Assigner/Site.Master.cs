using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Case_Assigner
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            Session["MyName"] = "";
        }
    }
}