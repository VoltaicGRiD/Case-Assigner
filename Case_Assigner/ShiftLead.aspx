<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShiftLead.aspx.cs" Inherits="Case_Assigner.ShiftLead" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div class="jumbotron">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick"></asp:Timer>
                <h3>Shift Lead Notifications:</h3>
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SLNotificationSource" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" OnRowCommand="NotificationRowCommand">
                    <Columns>
                        <asp:BoundField DataField="From" HeaderText="From" SortExpression="From">
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Text" HeaderText="Text" SortExpression="Text">
                            <ItemStyle Width="600px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority">
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:ButtonField ButtonType="Button" CommandName="RemoveAlert" Text="Remove" />
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
                <asp:SqlDataSource ID="SLNotificationSource" runat="server" ConnectionString="<%$ ConnectionStrings:CaseAssignerDBAzureConnString %>" SelectCommand="SELECT * FROM [SLNotifications] ORDER BY [Priority]"></asp:SqlDataSource>


             

                <h3>Query Runner:</h3>
                Table Names: [CaseTable] , [CSMTable] , [SLNotifications]<br />
                Function Examples ( remove curly braces { } )<br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; { UPDATE [CaseTable] SET Name=___ WHERE ___ = ___ }<br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; { DELETE FROM [CSMTable] WHERE ___ = ___ }<br />
                &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; { INSERT INTO [SLNotifications] (From, Text, Priority) VALUES (_From_, _Text_, _Priority_) }<br />
                <asp:TextBox ID="TextBox1" runat="server" Width="381px" />
                <asp:Button ID="Button1" runat="server" OnClick="SendQueryClick" Text="Send Query" />
                <br />
                Any messages will be displayed here:<br />
                <asp:Label ID="QueryErrLabel" runat="server"></asp:Label>

                <h3>All Cases:</h3>
                <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="SR" DataSourceID="FullCaseViewSource">
                    <Columns>
                        <asp:BoundField DataField="Time Queued" HeaderText="Time" SortExpression="Time">
                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SR" HeaderText="SR" ReadOnly="True" SortExpression="SR">
                            <ItemStyle HorizontalAlign="Center" Width="160px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Site Name" HeaderText="Site" SortExpression="Site Name">
                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product" HeaderText="Product" SortExpression="Product">
                            <ItemStyle HorizontalAlign="Center" Width="160px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unified Support" HeaderText="Unified Support" SortExpression="Unified Support">
                            <ItemStyle HorizontalAlign="Center" Width="160px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Assigned To" HeaderText="Assigned To" SortExpression="Assigned To">
                            <ItemStyle HorizontalAlign="Center" Width="160px" />
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
                <asp:SqlDataSource ID="FullCaseViewSource" runat="server" ConnectionString="<%$ ConnectionStrings:CaseAssignerDBAzureConnString %>" SelectCommand="SELECT * FROM [CaseTable]"></asp:SqlDataSource>

                   <h3>Shift Fixer: (WIP - Do not use)</h3>
                CSM Name:<br />
                <asp:TextBox ID="TextBox2" runat="server" Width="110px"></asp:TextBox>
                <br />
                <br />
                <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                    <asp:ListItem>Sunday</asp:ListItem>
                    <asp:ListItem>Monday</asp:ListItem>
                    <asp:ListItem>Tuesday</asp:ListItem>
                    <asp:ListItem>Wednesday</asp:ListItem>
                    <asp:ListItem>Thursday</asp:ListItem>
                    <asp:ListItem>Friday</asp:ListItem>
                    <asp:ListItem>Saturday</asp:ListItem>
                </asp:CheckBoxList>
                <br />
                <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                    <asp:ListItem>Shift A (9 - 6)</asp:ListItem>
                    <asp:ListItem>Shift B (10 - 7)</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <asp:Button ID="Button2" runat="server" OnClick="SubmitShiftChangeClick" Text="Submit" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
