using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Text;

namespace Case_Assigner
{
    public partial class CSM : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            //if (!IsPostBack)
            //{
            //    try
            //    {
            //        NameBox.Text = Environment.UserName.Split('.')[0].ToUpperInvariant() + " " + Environment.UserName.Split('.')[1].ToUpperInvariant();
            //    }
            //    catch
            //    {
            //        try
            //        {
            //            NameBox.Text = Session["MyName"].ToString();

            //            if (String.IsNullOrWhiteSpace(NameBox.Text))
            //            {
            //                Label1.Visible = true;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Label1.Visible = true;
            //        }
            //    }
            //}
        }

        private void TimeThreadFunction()
        {
            
        }

        protected void SendQuery(string query)
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

                GridView1.DataBind();
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

        protected void SaveScheduleClick( object sender, EventArgs e )
        {
            if (!string.IsNullOrWhiteSpace(NameBox.Text))
            {
                string Query = "UPDATE [CSMTable] SET Mon={0}, Tue={1}, Wed={2}, Thu={3}, Fri={4}, Sat={5}, Sun={6}, Shift='{7}' WHERE Name LIKE '{8}'";
                string Shift = TimeDropDown.SelectedValue;
                string Name  = NameBox.Text.Trim();

                switch (DayDropDown.SelectedValue)
                {
                    case "Sun - Thu":
                        SendQuery(String.Format(Query, 1, 1, 1, 1, 0, 0, 1, Shift, Name));
                        break;
                    case "Mon - Fri":
                        SendQuery(String.Format(Query, 1, 1, 1, 1, 1, 0, 0, Shift, Name));
                        break;
                    case "Tue - Sat":
                        SendQuery(String.Format(Query, 0, 1, 1, 1, 1, 1, 0, Shift, Name));
                        break;
                    case "Wed - Sun":
                        SendQuery(String.Format(Query, 0, 0, 1, 1, 1, 1, 1, Shift, Name));
                        break;
                    case "Thu - Mon":
                        SendQuery(String.Format(Query, 1, 0, 0, 1, 1, 1, 1, Shift, Name));
                        break;
                    case "Fri - Tue":
                        SendQuery(String.Format(Query, 1, 1, 0, 0, 1, 1, 1, Shift, Name));
                        break;
                    case "Sat - Wed":
                        SendQuery(String.Format(Query, 1, 1, 1, 0, 0, 1, 1, Shift, Name));
                        break;
                    default:
                        Debug.WriteLine("Error formatting shift string");
                        break;
                }
            }

            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('Please ensure your name is typed in');", true);
            }
        }

        protected void StatusBox_TextChanged( object sender, EventArgs e )
        {
            SqlConnection connection = null;

            try
            {
                string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                using (connection = new SqlConnection(connectionStr))
                {
                    string query = "UPDATE [CSMTable] SET Status=@status WHERE Name=@name";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        try
                        {
                            command.Parameters.AddWithValue("@status", StatusBox.Text);
                            command.Parameters.AddWithValue("@name", NameBox.Text.Trim());
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
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        protected void AwayFromDeskChecked( object sender, EventArgs e )
        {
            if (AwayFromDeskCheck.Checked == true)
            {
                SendQuery("UPDATE [CSMTable] SET Status='Away' WHERE Name=" + NameBox.Text.Trim());
            }
            else
            {
                SendQuery("UPDATE [CSMTable] SET Status='" + StatusBox.Text + "' WHERE Name='" + NameBox.Text.Trim() + "'");
            }
        }

        protected void Unnamed_Tick( object sender, EventArgs e )
        {
            int oldRowCount = GridView1.Rows.Count;
            Debug.WriteLine(oldRowCount);
            GridView1.DataBind();
            int newRowCount = GridView1.Rows.Count;
            Debug.WriteLine(newRowCount);

            if (newRowCount > oldRowCount)
            {
                Debug.WriteLine("New Row!");
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "Func", "<script>if (\"Notification\" in window) {let ask = Notification.requestPermission();ask.then(permission => {if (permission === \"granted\"){let msg = new Notification(\"New Case:\", {body: \"You were assigned a new case\"});msg.addEventListener(\"click\", event => {alert(\"Click Recieved\");});}});}</script>", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "displayalertmessage", "ShowAlert();", true);
            }
        }

        protected void GridView1_RowCommand( object sender, GridViewCommandEventArgs e )
        {
            if (e.CommandName == "RemoveCase")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                string SR = GridView1.Rows[rowIndex].Cells[1].Text;

                SendQuery("UPDATE [CSMTable] SET [Case Count]=[Case Count] - 1 WHERE Name='" + NameBox.Text + "'");
                SendQuery("DELETE FROM [CaseTable] WHERE SR='" + SR + "'");
            }

            else if (e.CommandName == "MarkTaken")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                string SR = GridView1.Rows[rowIndex].Cells[1].Text;

                SendQuery("UPDATE [CaseTable] SET IsNew=0 WHERE SR='" + SR + "'");

                ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('You marked this case as taken successfully!');", true);
            }
        }

        protected void NotificationSubmitButton_Click( object sender, EventArgs e )
        {
            int prio = int.Parse(notificationPriority.Text);

            if (prio > 5)
                prio = 5;
            else if (prio < 1)
                prio = 1;

            if (!string.IsNullOrWhiteSpace(notificationText.Text) && !string.IsNullOrWhiteSpace(notificationPriority.Text)) {
                SendQuery("INSERT INTO [SLNotifications] (Text, Priority, From) VALUES ('" + notificationText.Text + "', " + prio + ", '" + NameBox.Text + "')");
            }
        }

        protected void SetSessionName( object sender, EventArgs e )
        {
            HttpContext.Current.Session["MyName"] = NameBox.Text;

            Debug.WriteLine(HttpContext.Current.Session["MyName"].ToString());
        }

        protected void LoadProfileClick( object sender, EventArgs e )
        {
            SqlConnection connection = null;
            string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

            bool[] dates = new bool[7];
            string shift = String.Empty;
            string status = String.Empty;

            using (connection = new SqlConnection(connectionStr))
            {
                string query = "SELECT * FROM [CSMTable] WHERE Name LIKE '" + NameBox.Text + "'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dates[0] = reader.GetBoolean(reader.GetOrdinal("Sun"));
                                dates[1] = reader.GetBoolean(reader.GetOrdinal("Mon"));
                                dates[2] = reader.GetBoolean(reader.GetOrdinal("Tue"));
                                dates[3] = reader.GetBoolean(reader.GetOrdinal("Wed"));
                                dates[4] = reader.GetBoolean(reader.GetOrdinal("Thu"));
                                dates[5] = reader.GetBoolean(reader.GetOrdinal("Fri"));
                                dates[6] = reader.GetBoolean(reader.GetOrdinal("Sat"));

                                shift = reader.GetString(reader.GetOrdinal("Shift"));

                                status = reader.GetString(reader.GetOrdinal("Status"));
                            }
                            else
                            {
                                Debug.WriteLine("DC Error!");
                            }
                        }
                    }
                    catch (SqlException err)
                    {
                        Debug.WriteLine(err.ToString());
                    }
                    connection.Close();
                }
            }

            string DayMenuStr = String.Empty;

            for (int i = 0; i < 7; i++)
            {
                if (dates[i] == true)
                {
                    switch (i)
                    {
                        case 0:
                            DayMenuStr = "Sun - Thu";
                            break;
                        case 1:
                            DayMenuStr = "Mon - Fri";
                            break;
                        case 2:
                            DayMenuStr = "Tue - Sat";
                            break;
                        case 3:
                            DayMenuStr = "Wed - Sun";
                            break;
                        case 4:
                            DayMenuStr = "Thu - Mon";
                            break;
                        case 5:
                            DayMenuStr = "Fri - Tue";
                            break;
                        case 6:
                            DayMenuStr = "Sat - Wed";
                            break;
                        default:
                            DayMenuStr = "Please enter";
                            break;
                    }

                    break;
                }
            }

            DayDropDown.Text = DayMenuStr;
            DayDropDown.Enabled = true;
            TimeDropDown.Text = shift;
            TimeDropDown.Enabled = true;
            Button1.Enabled = true;

            if (status == "away" || status == "Away")
            {
                AwayFromDeskCheck.Checked = true;
            }
            else
            {
                StatusBox.Text = status;
            }

            StatusBox.Enabled = true;
            AwayFromDeskCheck.Enabled = true;
            Button2.Enabled = true;

            Debug.WriteLine(dates[0]);
            Debug.WriteLine(dates[1]);
            Debug.WriteLine(dates[2]);
            Debug.WriteLine(dates[3]);
            Debug.WriteLine(dates[4]);
            Debug.WriteLine(dates[5]);
            Debug.WriteLine(dates[6]);

            Debug.WriteLine(shift);
        }

        protected void TriageButtonClick( object sender, EventArgs e )
        {

        }
    }
}