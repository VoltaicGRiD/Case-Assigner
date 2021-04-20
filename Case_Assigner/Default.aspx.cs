using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.SignalR;

namespace Case_Assigner
{
    public partial class _Default : Page
    {
        List<string> CSMList = new List<string>();

        protected void Page_Load( object sender, EventArgs e )
        {
          
            if (!IsPostBack)
            {
                
            }

            //List<Case> thisCaseList = c.GetCases();
            //Debug.WriteLine(thisCaseList.Count);

            //foreach (Case c in thisCaseList)
            //{
            //    Debug.WriteLine(c.SR);
            //}

            //CaseTable = new Table();
            //for (int i = 0; i < thisCaseList.Count; i++)
            //{
            //    TableRow tr = new TableRow();
            //    tr.ID = thisCaseList[i].SR;
            //    TableCell c1 = new TableCell();
            //    TableCell c2 = new TableCell();
            //    TableCell c3 = new TableCell();
            //    TableCell c4 = new TableCell();
            //    TableCell c5 = new TableCell();
            //    TableCell c6 = new TableCell();
            //    c1.Text = thisCaseList[i].TimeQueued;
            //    c2.Text = thisCaseList[i].SR;
            //    c3.Text = thisCaseList[i].SiteName;
            //    c4.Text = thisCaseList[i].Product;
            //    c5.Text = thisCaseList[i].UnifiedSupport;
            //    c6.Text = thisCaseList[i].Country;
            //    tr.Cells.Add(c1);
            //    tr.Cells.Add(c2);
            //    tr.Cells.Add(c3);
            //    tr.Cells.Add(c4);
            //    tr.Cells.Add(c5);
            //    tr.Cells.Add(c6);
            //    CaseTable.Rows.Add(tr);
            //}
        }

        protected void AddCase_Click( object sender, EventArgs e )
        {
            SqlConnection connection = null;

            try
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<CaseHub>();

                string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                string[] data = new string[6];

                if (!string.IsNullOrEmpty(CaseTextBox.Text))
                {
                    data = CaseTextBox.Text.Split('\t');
                }

                CaseTextBox.Text = "";

                string TimeQueued = data[0];
                long SR = long.Parse(data[1]);
                string SiteName = data[2];
                string Product = data[3];
                string UnifiedSupport = data[4];
                string Country = data[5];

                using (connection = new SqlConnection(connectionStr))
                {
                    String query = "INSERT INTO [CaseTable] (IsNew, SR, [Time Queued], [Site Name], Product, [Unified Support], Country) VALUES (@New, @SR, @TimeQueued, @SiteName, @Product, @UnifiedSupport, @Country)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@New", 1);
                        command.Parameters.AddWithValue("@SR", SR);
                        command.Parameters.AddWithValue("@TimeQueued", TimeQueued);
                        command.Parameters.AddWithValue("@SiteName", SiteName);
                        command.Parameters.AddWithValue("@Product", Product);
                        command.Parameters.AddWithValue("@UnifiedSupport", UnifiedSupport);
                        command.Parameters.AddWithValue("@Country", Country);

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
                context.Clients.All.logCase();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        protected void CaseGridView_RowCommand( object sender, GridViewCommandEventArgs e )
        {
            SqlConnection connection = null;

            var context = GlobalHost.ConnectionManager.GetHubContext<CaseHub>();

            if (e.CommandName == "RemoveRow")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                try
                {
                    string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                    using (connection = new SqlConnection(connectionStr))
                    {
                        String query = String.Format("UPDATE [CaseTable] SET IsNew=0 WHERE SR='{0}'", GridView1.Rows[rowIndex].Cells[2].Text);
                        Debug.WriteLine(query);

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
                    context.Clients.All.logCase();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            else if (e.CommandName == "AssignCase")
            {
                string Day = DateTime.Now.DayOfWeek.ToString().Substring(0, 3);
                int Hour = DateTime.Now.Hour;

                try
                {
                    int rowIndex = int.Parse(e.CommandArgument.ToString());
                    string Name = string.Empty;

                    string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                    using (connection = new SqlConnection(connectionStr))
                    {
                        string query = string.Empty;

                        // A shift only
                        if (Hour <= 9) 
                        {
                            switch(Day)
                            {
                                case "Mon":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Mon=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Mon = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Tue":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Tue=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Tue = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Wed":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Wed=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Wed = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Thu":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Thu=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Thu = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Fri":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Fri=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Fri = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Sat":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Sat=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Sat = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Sun":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Sun=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift A%') AND (Sun = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                            }
                        }
                        // All shifts
                        else if (Hour >= 10 && Hour <= 4)
                        {
                            switch (Day)
                            {
                                case "Mon":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Mon=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Mon = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Tue":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Tue=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Tue = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Wed":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Wed=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Wed = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Thu":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Thu=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Thu = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Fri":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Fri=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Fri = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Sat":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Sat=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Sat = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Sun":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Sun=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Sun = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                            }
                        }
                        // B shift only
                        else if (Hour >= 4 && Hour < 5)
                        {
                            switch (Day)
                            {
                                case "Mon":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (SHIFT LIKE '%Shift B%') AND (Mon=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift B%') AND (Mon = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Tue":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (SHIFT LIKE '%Shift B%') AND (Tue=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift B%') AND (Tue = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Wed":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (SHIFT LIKE '%Shift B%') AND (Wed=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift B%') AND (Wed = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Thu":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (SHIFT LIKE '%Shift B%') AND (Thu=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift B%') AND (Thu = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Fri":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (SHIFT LIKE '%Shift B%') AND (Fri=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift B%') AND (Fri = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Sat":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (SHIFT LIKE '%Shift B%') AND (Sat=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift B%') AND (Sat = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Sun":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (SHIFT LIKE '%Shift B%') AND (Sun=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Shift LIKE '%Shift B%') AND (Sun = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                            }
                        }
                        else
                        {
                            switch (Day)
                            {
                                case "Mon":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Mon=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Mon = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Tue":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Tue=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Tue = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Wed":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Wed=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Wed = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Thu":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Thu=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Thu = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Fri":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Fri=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Fri = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Sat":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Sat=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Sat = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                                case "Sun":
                                    query = "SELECT TOP 1 Name FROM [CSMTable] WHERE (Sun=1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') AND [Case Count] = (SELECT MIN([Case Count]) FROM[CSMTable] WHERE (Sun = 1) AND (Status NOT LIKE '%Break%' AND Status NOT LIKE '%Lunch%' AND Status NOT LIKE '%Away%') ) ORDER BY NEWID()";
                                    break;
                            }
                        }

                        Debug.WriteLine(query);

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            try
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        
                                        Name = reader.GetString(reader.GetOrdinal("Name"));
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

                        if (!string.IsNullOrEmpty(Name))
                        {
                            string updateQuery1 = String.Format("UPDATE [CSMTable] SET [Case Count] = [Case Count] + 1 WHERE [Name] LIKE '{0}'", Name);
                            string updateQuery2 = String.Format("UPDATE [CaseTable] SET [Assigned To]='{0}' WHERE [SR] LIKE '{1}'", Name, GridView1.Rows[rowIndex].Cells[2].Text);

                            using (SqlCommand command1 = new SqlCommand(updateQuery1, connection))
                            {
                                connection.Open();
                                try
                                {
                                    int result = command1.ExecuteNonQuery();
                                }
                                catch (SqlException err)
                                {
                                    Debug.WriteLine(err.ToString());
                                }
                                finally
                                {
                                    connection.Close();
                                }
                            }

                            using (SqlCommand command2 = new SqlCommand(updateQuery2, connection))
                            {
                                connection.Open();
                                try
                                {
                                    int result = command2.ExecuteNonQuery();
                                }
                                catch (SqlException err)
                                {
                                    Debug.WriteLine(err.ToString());
                                }
                                connection.Close();
                            }
                        }
                    }

                    GridView1.DataBind();
                    GridView2.DataBind();
                    context.Clients.All.logCase();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        [WebMethod]
        public void Reload()
        {
            Debug.WriteLine("RELOADING");
            GridView1.DataBind();
        }

        public void Timer1_Tick ( object sender, EventArgs e )
        {
            GridView1.DataBind();
            GridView2.DataBind();
        }

        protected void CSMGridView_RowCommand( object sender, GridViewCommandEventArgs e )
        {
            SqlConnection connection = null;

            var context = GlobalHost.ConnectionManager.GetHubContext<CaseHub>();

            if (e.CommandName == "RemoveCSM")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                try
                {
                    string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                    using (connection = new SqlConnection(connectionStr))
                    {
                        string query = string.Empty;

                        if (GridView2.Rows[rowIndex].Cells[2].Text == "Here")
                        {
                            query = String.Format("UPDATE [CSMTable] SET Status = 'Away' WHERE [Name] LIKE '{0}'", GridView2.Rows[rowIndex].Cells[1].Text);
                        }
                        else
                        {
                            query = String.Format("UPDATE [CSMTable] SET Status = 'Here' WHERE [Name] LIKE '{0}'", GridView2.Rows[rowIndex].Cells[1].Text);
                        }

                        Debug.WriteLine(query);

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

                    GridView2.DataBind();
                    context.Clients.All.logCase();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                       
                    }
                }
            }

            if (e.CommandName == "ReduceCount")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                try
                {
                    string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                    using (connection = new SqlConnection(connectionStr))
                    {
                        string query = String.Format("UPDATE [CSMTable] SET [Case Count] = [Case Count] - 1 WHERE [Name] LIKE '{0}'", GridView2.Rows[rowIndex].Cells[1].Text);

                        Debug.WriteLine(query);

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

                    GridView2.DataBind();
                    context.Clients.All.logCase();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            if (e.CommandName == "IncreaseCount")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                try
                {
                    string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                    using (connection = new SqlConnection(connectionStr))
                    {
                        string query = String.Format("UPDATE [CSMTable] SET [Case Count] = [Case Count] + 1 WHERE [Name] LIKE '{0}'", GridView2.Rows[rowIndex].Cells[1].Text);

                        Debug.WriteLine(query);

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

                    GridView2.DataBind();
                    context.Clients.All.logCase();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        protected void ClearAllCaseView( object sender, EventArgs e )
        {
            SqlConnection connection = null;

            try
            {
                string connectionStr = "Data Source=caseassignerdb.database.windows.net;Initial Catalog=CaseAssignerDB;Persist Security Info=True;User ID=dustin.conlon;Password=Artemis_1997";

                using (connection = new SqlConnection(connectionStr))
                {
                    string query = "UPDATE [CaseTable] SET IsNew = 0 WHERE IsNew = 1";

                    Debug.WriteLine(query);

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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}