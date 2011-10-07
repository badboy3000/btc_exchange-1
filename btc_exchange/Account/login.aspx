<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="btc_exchange.Account.login" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    Username:<br />
    <asp:TextBox ID="txtUser" runat="server" Width="438px" TabIndex="1"></asp:TextBox>
    <br />
    Password:<br />
    <asp:TextBox ID="txtPass" runat="server" TextMode="Password" Width="438px" 
            TabIndex="2"></asp:TextBox>
        <br />
                  <recaptcha:RecaptchaControl
            ID="recaptcha"
            runat="server"
            PublicKey="6Ldar8USAAAAACFRvd2bme2i2GhHF_iaDUJRwtU0"
            PrivateKey="6Ldar8USAAAAAE_y3UzyteENxlUjUUNZ_W_zSDsz"
            Theme="clean" TabIndex="3"
            />
        <asp:Label ID="lblCaptchaErr" runat="server" ForeColor="Red"></asp:Label>
    <br />
    <asp:Button ID="btnLogin" runat="server" onclick="btnLogin_Click" 
        Text="Log In" TabIndex="4" />
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
</p>
</asp:Content>
