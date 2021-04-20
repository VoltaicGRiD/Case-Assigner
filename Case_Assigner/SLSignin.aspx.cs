using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Case_Assigner
{
    public partial class SLSignin : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {

        }

        protected void Submit_Click( object sender, EventArgs e )
        {
            if (PassBox.Text == "T3KSH1FTL34D")
            {
                Response.Redirect("~/ShiftLead.aspx");
            }
            else
            {
                Label1.Visible = true;
            }
        }
    }
}