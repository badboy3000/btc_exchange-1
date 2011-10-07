<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="btc_exchange.Account.register" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Desired Username:<br />
        <asp:TextBox ID="txtUser" runat="server" Width="438px"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
            ControlToValidate="txtUser" 
            ErrorMessage="Username must be alphanumeric (no spaces or special characters)" 
            ForeColor="Red" ValidationExpression="^[a-zA-Z0-9]+$"></asp:RegularExpressionValidator>
&nbsp;
        <br />
        Email address:<br />
        <asp:TextBox ID="txtEmail" runat="server" Width="438px"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
            ControlToValidate="txtEmail" ErrorMessage="Invalid email address" 
            ForeColor="Red" 
            ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:RegularExpressionValidator>
&nbsp;
        <br />
        Password:<br />
        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" Width="438px"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="txtPass" 
            ErrorMessage="Requires at least 1 number, 1 special character and at least 8 characters long" 
            ForeColor="Red" 
            ValidationExpression="(?=^.{7,}$)(?=.*\d)(?=.*\W+)(?![.\n]).*$"></asp:RegularExpressionValidator>
&nbsp;
        <br />
        Password (Again):<br />
        <asp:TextBox ID="txtPassConfirm" runat="server" TextMode="Password" 
            Width="438px"></asp:TextBox>
        <asp:CompareValidator ID="CompareValidator1" runat="server" 
            ControlToCompare="txtPass" ControlToValidate="txtPassConfirm" 
            ErrorMessage="Passwords do not match" ForeColor="Red"></asp:CompareValidator>
&nbsp;
        <br />
          <recaptcha:RecaptchaControl
            ID="recaptcha"
            runat="server"
            PublicKey="6Ldar8USAAAAACFRvd2bme2i2GhHF_iaDUJRwtU0"
            PrivateKey="6Ldar8USAAAAAE_y3UzyteENxlUjUUNZ_W_zSDsz"
            Theme="clean"
            />
        <asp:Label ID="lblCaptchaErr" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <asp:Button ID="btnCreate" runat="server" onclick="btnCreate_Click" 
            Text="Create User" />
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txtUser" ErrorMessage="Username is required" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="txtEmail" ErrorMessage="Email address is required" 
            ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
            ControlToValidate="txtPass" ErrorMessage="Password is required" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
            ControlToValidate="txtPassConfirm" 
            ErrorMessage="Password confirmation is required" ForeColor="Red"></asp:RequiredFieldValidator>
    </p>
</asp:Content>
