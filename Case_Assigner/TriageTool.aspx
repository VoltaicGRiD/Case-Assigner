<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TriageTool.aspx.cs" Inherits="Case_Assigner.TriageTool" MasterPageFile="~/Site.Master"%>
<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div class="jumbotron">
        <div class="row">
            <div class="columnhalf">
                <strong>Triage Templates:</strong><br />
                <br />
                Business Impact:<br />
                <asp:TextBox runat="server" ID="BIBox" Height="59px" Rows="3" Width="100%" Font-Size="Small" TextMode="MultiLine"></asp:TextBox>
                &nbsp;
                <br />
                Current Status:<br />
                <asp:TextBox runat="server" ID="CSBox" Height="100px" Rows="8" Width="100%" Font-Size="Small" TextMode="MultiLine"></asp:TextBox>
                <br />
                Action Plan:<br />
                <asp:TextBox runat="server" ID="APBox" Height="100px" Rows="8" Width="100%" Font-Size="Small" TextMode="MultiLine"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="Label1" runat="server" Text="Customer Name:" Width="49%"></asp:Label>
                &nbsp;
                <asp:Label ID="Label2" runat="server" Text="Engineer Name:"></asp:Label>
                <br />
                <asp:TextBox ID="CustBox" runat="server" Width="49%"></asp:TextBox>
                &nbsp;<asp:TextBox ID="EngiBox" runat="server" Width="49%"></asp:TextBox>
                <br />
                <br />
                Customer Temperature:<br />
                <asp:DropDownList ID="DropDownList1" runat="server">
                    <asp:ListItem>Cold</asp:ListItem>
                    <asp:ListItem>Cool</asp:ListItem>
                    <asp:ListItem>Warm</asp:ListItem>
                    <asp:ListItem>Hot</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="TempBox" runat="server" Width="100%"></asp:TextBox>
                <br />
                <br />
                Appropriate Technical Resources? ( leave empty if &#39;yes&#39; )<br />
                <asp:TextBox ID="TechBox" runat="server" Width="100%"></asp:TextBox>
                <br />
                PFE Discussed? ( leave empty if &#39;no&#39; )<br />
                <asp:TextBox ID="PFEBox" runat="server" Width="100%"></asp:TextBox>
                <br />
                Roadblocks? ( leave empty if &#39;no&#39; )<br />
                <asp:TextBox ID="RoadBox" runat="server" Width="100%"></asp:TextBox>
                <br />
                Should case be Escalated? ( leave empty if &#39;no&#39; )<br />
                <asp:TextBox ID="EscaBox" runat="server" Width="100%"></asp:TextBox>
                <br />
                Any questions or concerns? ( leave empty if no questions )<br />
                <asp:TextBox ID="QsBox" runat="server" Width="100%"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="Button5" runat="server" Text="Compile Triage" OnClick="FinalizeClick" />
            </div>
            <div class="columnhalf">

                <strong>Final Triage Template:<br />
                <br />
                <asp:TextBox ID="TemplateBox" runat="server" Height="558px" Rows="500" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </strong>

            </div>
        </div>
    </div>
</asp:Content>