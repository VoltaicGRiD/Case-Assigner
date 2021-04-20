<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Case_Assigner._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">

        <h2>Case View</h2>
        <p class="lead">Add case: </p>
        <p class="lead">
            <asp:TextBox ID="CaseTextBox" runat="server" Height="22px" Width="90%" Font-Size="X-Small"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Height="23px" Text="Add" Font-Size="Small" OnClick="AddCase_Click" Width="58px" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </p>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" Interval="5000" OnTick="Timer1_Tick"></asp:Timer>
                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="SR" DataSourceID="CaseTableSQLSource" OnRowCommand="CaseGridView_RowCommand">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" CommandName="RemoveRow" Text="Remove" />

                            <asp:BoundField DataField="Time Queued" HeaderText="Time Queued" SortExpression="Time Queued" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">

                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />

                            </asp:BoundField>

                            <asp:BoundField DataField="SR" HeaderText="SR" ReadOnly="True" SortExpression="SR" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px">

                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="180px" />

                            </asp:BoundField>

                            <asp:BoundField DataField="Site Name" HeaderText="Site Name" SortExpression="Site Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="300px">

                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="300px" />

                            </asp:BoundField>

                            <asp:BoundField DataField="Product" HeaderText="Product" SortExpression="Product" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200">

                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" />

                            </asp:BoundField>

                            <asp:BoundField DataField="Unified Support" HeaderText="Unified Support" SortExpression="Unified Support" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px">

                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="180px" />

                            </asp:BoundField>

                            <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px">

                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" />

                            </asp:BoundField>
                            <asp:ButtonField ButtonType="Button" CommandName="AssignCase" Text="Assign" />
                            <asp:BoundField DataField="Assigned To" HeaderText="Assigned To" SortExpression="Assigned To" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px"> 

                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                            </asp:BoundField>

                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
                    
                    <asp:SqlDataSource ID="CaseTableSQLSource" runat="server" ConnectionString="<%$ ConnectionStrings:CaseAssignerDBAzureConnString %>" SelectCommand="SELECT * FROM [CaseTable] WHERE IsNew=1"></asp:SqlDataSource>
             
            <h2>CSM View</h2>
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="Id" DataSourceID="CaseCountView" OnRowCommand="CSMGridView_RowCommand" AllowSorting="True">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" CommandName="RemoveCSM" Text="Change Status" />
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                            <ItemStyle HorizontalAlign="Center" Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Case Count" HeaderText="Cases" SortExpression="Case Count">
                            <ItemStyle Width="75px" />
                            </asp:BoundField>
                            <asp:ButtonField ButtonType="Button" Text="-" CommandName="ReduceCount">
                            <ItemStyle Font-Size="Large" />
                            </asp:ButtonField>
                            <asp:ButtonField ButtonType="Button" Text="+" CommandName="IncreaseCount">
                            <ItemStyle Font-Size="Large" />
                            </asp:ButtonField>
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="CaseCountView" runat="server" ConnectionString="<%$ ConnectionStrings:CaseAssignerDBAzureConnString %>" SelectCommand="SELECT * FROM [CaseCountView]"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="CSMNameView" runat="server" ConnectionString="<%$ ConnectionStrings:CaseAssignerDBAzureConnString %>" SelectCommand="SELECT * FROM [CSMNameView]"></asp:SqlDataSource>
               </ContentTemplate>
            </asp:UpdatePanel>
    </div>

    <script src="Scripts/jquery-3.3.1.min.js"></script>
    <script src="Scripts/jquery.signalR-2.2.2.min.js"></script>
    <script src="/signalr/hubs" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {

            var hub = $.connection.caseHub;

            hub.client.logCase = function () {
                //$.ajax({
                //    type: "POST",
                //    url: 'Default.aspx/Reload',
                //    data: "",
                //    contentType: "application/json; charset=utf-8",
                //    dataType: "json",
                //    success: function (msg) {
                //        $("#divResult").html("success");
                //    },
                //    error: function (e) {
                //        $("#divResult").html("Something Wrong.");
                //    }
                //});
                //__doPostBack('#GridView1', "");
            };

            $.connection.hub.start();

        });

    </script>
</asp:Content>



