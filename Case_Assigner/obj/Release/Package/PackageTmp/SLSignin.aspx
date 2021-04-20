<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SLSignin.aspx.cs" Inherits="Case_Assigner.SLSignin" MasterPageFile="~/Site.Master"%>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div class="jumbotron">
        <asp:TextBox ID="PassBox" runat="server" Width="156px"></asp:TextBox>
        <asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_Click" />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Incorrect password!" ForeColor="#993333" Visible="False"></asp:Label>
    </div>
</asp:Content>

