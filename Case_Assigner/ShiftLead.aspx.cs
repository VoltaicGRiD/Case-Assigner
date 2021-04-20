using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Case_Assigner
{
    public partial class ShiftLead : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {

        }

        protected void Timer1_Tick( object sender, EventArgs e )
        {
            GridView1.DataBind();
            GridView2.DataBind();
        }

        protected void SendQuery( string query )
        {
            SqlConnection connection = null;

            try
            {
                string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                using (connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        try
                        {
                            int result = command.ExecuteNonQuery();
                        }
                        catch (SqlException err)
                        {
                            Debug.WriteLine(err.ToString());
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        protected void NotificationRowCommand( object sender, GridViewCommandEventArgs e )
        {
            if (e.CommandName == "RemoveAlert")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                SendQuery("DELETE FROM [SLNotifications] WHERE Text LIKE '" + GridView1.Rows[rowIndex].Cells[0].Text + "'");
            }
        }

        protected void Chart1_Load( object sender, EventArgs e )
        {

        }

        protected void SendQueryClick( object sender, EventArgs e )
        {
            SqlConnection connection = null;
            string query = TextBox1.Text;

            try
            {
                string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                using (connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        try
                        {
                            int result = command.ExecuteNonQuery();
                        }
                        catch (SqlException err)
                        {
                            Debug.WriteLine(err.ToString());
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                QueryErrLabel.Text = ex.ToString();
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        protected void SubmitShiftChangeClick( object sender, EventArgs e )
        {
            
        }
    }
}