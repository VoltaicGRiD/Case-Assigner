<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSM.aspx.cs" Inherits="Case_Assigner.CSM" MasterPageFile="~/Site.Master" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="jumbotron">
        <h2>Manage my account</h2>
        <p style="font-size: small">Please ensure this information is accurate on each sign-in</p>
        <p style="font-size: small; color: #993333;">Do not leave this page, or this information will reset --- Instead, open another page in a new tab</p>
        <div class="row" style="font-size: medium">
            <div class="column">
                My Name:
                <asp:TextBox ID="NameBox" runat="server" Height="24px" Width="100%" AutoPostBack="false" ></asp:TextBox>  
                <br />
                <asp:Label ID="Label1" runat="server" Font-Size="Small" ForeColor="#993333" Text="There was an error getting name, please enter your name (e.g.: John Doe)" Visible="False"></asp:Label>
                <br />
                <br />
                <asp:Button ID="Button3" runat="server" OnClick="LoadProfileClick" Text="Load Profile" />
                <br />
            </div>
            <div class="column">
                My Schedule:<br />
                <asp:DropDownList ID="DayDropDown" runat="server" Font-Size="Medium" Enabled="False">
                    <asp:ListItem>Sun - Thu</asp:ListItem>
                    <asp:ListItem>Mon - Fri</asp:ListItem>
                    <asp:ListItem>Tue - Sat</asp:ListItem>
                    <asp:ListItem>Wed - Sun</asp:ListItem>
                    <asp:ListItem>Thu - Mon</asp:ListItem>
                    <asp:ListItem>Fri - Tue</asp:ListItem>
                    <asp:ListItem>Sat - Wed</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:DropDownList ID="TimeDropDown" runat="server" Font-Size="Medium" Enabled="False">
                    <asp:ListItem>Shift A (9 - 6)</asp:ListItem>
                    <asp:ListItem>Shift B (10-7)</asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                <asp:Button ID="Button1" runat="server" OnClick="SaveScheduleClick" Text="Save Changes" Enabled="False" />
            </div>
            
            <div class="column">
                My Status:<br />
                <asp:TextBox ID="StatusBox" runat="server" Height="24px" Width="100%" Enabled="False"></asp:TextBox>
                <br />
                <br />
                <asp:CheckBox ID="AwayFromDeskCheck" runat="server" Text="Mark away from desk" OnCheckedChanged="AwayFromDeskChecked" Enabled="False" />
                <br />
                <Label style="font-size: small">Changing this will overwrite your &quot;My Status:&quot;</Label>
                <br />
                <br />
                <asp:Button ID="Button2" runat="server" OnClick="StatusBox_TextChanged" Text="Save" Enabled="False" />
                    </div>
            <div class="column">
                Send request / notification to Shift Leads:                Text:<br />
                <asp:TextBox runat="server" id="notificationText" style="width: 100%; height: 72px;" Rows="5"></asp:TextBox>
                <br />
                Priority (1 - 5):<br />
                <asp:TextBox ID="notificationPriority" TextMode="Number" runat="server" min="0" max="20" step="1"/>
                <br />
                <br />
                <asp:Button ID="NotificationSubmitButton" runat="server" Text="Send Notification" OnClick="NotificationSubmitButton_Click" />
            </div>
        </div>
        <div class="row">
            <h3>My Assigned Cases:</h3>
            <p style="font-size: small">Removing or marking a case as taken may take up to 5 seconds</p>
            <p style="font-size: small; color: #993333;">Please mark a case as taken as soon as you take ownership in the CritSit portal </p>
            <p style="font-size: small; color: #993333;">Please remove a case as soon as you close a case in the portal</p>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Unnamed_Tick"></asp:Timer>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="SR" DataSourceID="MyCaseDataSource" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" OnRowCommand="GridView1_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Time Queued" HeaderText="Time" SortExpression="Time Queued">
                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SR" HeaderText="SR" ReadOnly="True" SortExpression="SR">
                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Site Name" HeaderText="Site" SortExpression="Site Name" >
                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Product" HeaderText="Product" SortExpression="Product" >
                    <ItemStyle HorizontalAlign="Center" Width="180px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Unified Support" HeaderText="Unified Support" SortExpression="Unified Support" >
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" >
                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                    </asp:BoundField>
                    <asp:ButtonField ButtonType="Button" CommandName="MarkTaken" Text="Mark as Taken" />
                    <asp:ButtonField ButtonType="Button" CommandName="RemoveCase" Text="Remove" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            <asp:SqlDataSource ID="MyCaseDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:CaseAssignerDBAzureConnString %>" SelectCommand="SELECT * FROM [CaseTable] WHERE ([Assigned To] LIKE '%' + @Assigned_To + '%')">
                <SelectParameters>
                    <asp:ControlParameter ControlID="NameBox" Name="Assigned_To" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

        </div>
    </div>
</asp:Content>
