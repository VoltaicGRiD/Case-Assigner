using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Case_Assigner
{
    public partial class TriageTool : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {

        }

        protected void DropDownList1_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (DropDownList1.SelectedIndex > 1)
                TempBox.Enabled = true;
            else
                TempBox.Enabled = false;
        }

        protected void FinalizeClick( object sender, EventArgs e )
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("• Triage Comm Completed: ");
            sb.AppendLine();
            if (string.IsNullOrWhiteSpace(TechBox.Text))
            {
                sb.AppendLine("• Appropriate Technical Resources Engaged (Y/N): Y");
            }
            else
            {
                sb.AppendLine("• Appropriate Technical Resources Engaged (Y/N): Y");
                sb.AppendLine("    - " + TechBox.Text);
            }
            sb.AppendLine("• Current Customer Impact: ");
            sb.AppendLine();
            sb.AppendLine(BIBox.Text);
            sb.AppendLine();
            if (DropDownList1.SelectedValue == "Warm" || DropDownList1.SelectedValue == "Hot")
            {
                sb.AppendLine("• Customer Temp: " + DropDownList1.SelectedValue);
                sb.AppendLine("    - " + TempBox.Text);
            }
            else
            {
                sb.AppendLine("• Customer Temp: " + DropDownList1.SelectedValue);
            }
            sb.AppendLine("• Current Status & Action Plan: ");
            sb.AppendLine();
            try
            {
                string[] data = CSBox.Text.Split('.');
                string CS = data[data.Length];
                sb.AppendLine(CS);
            }
            catch
            {
                sb.AppendLine(CSBox.Text);
            }
            sb.AppendLine();
            sb.AppendLine(APBox.Text);
            sb.AppendLine();
            if (!string.IsNullOrWhiteSpace(RoadBox.Text))
            {
                sb.AppendLine("• Roadblocks (Y/N): Y");
                sb.AppendLine("    - " + RoadBox.Text);
            }
            else
            {
                sb.AppendLine("• Roadblocks (Y/N): N");
            }
            if (!string.IsNullOrWhiteSpace(PFEBox.Text))
            {
                sb.AppendLine("• PFE Discussed (Y/N): Y");
                sb.AppendLine("    - " + PFEBox.Text);
            }
            else
            {
                sb.AppendLine("• PFE Discussed (Y/N): N");
            }
            if (!string.IsNullOrWhiteSpace(EscaBox.Text))
            {
                sb.AppendLine("• Should case be Escalated to CMET (Y/N): Y");
                sb.AppendLine("    - " + EscaBox.Text);
            }
            else
            {
                sb.AppendLine("• Should case be Escalated to CMET (Y/N): N");
            }
            sb.AppendLine("• Participants: " + EngiBox.Text + " (Engineer) | " + CustBox.Text + " (Customer)");
            if (!string.IsNullOrWhiteSpace(QsBox.Text))
            {
                sb.AppendLine("• Questions/Concerns: ");
                sb.AppendLine("    - " + QsBox.Text);
            }
            else
            {
                sb.AppendLine("• Questions/Concerns: ");
                sb.AppendLine("    - None currently");
            }
            sb.AppendLine("• Outcome of the triage: ");

            TemplateBox.Text = sb.ToString();
        }
    }
}