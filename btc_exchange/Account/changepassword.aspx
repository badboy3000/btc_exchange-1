<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="changepassword.aspx.cs" Inherits="btc_exchange.Account.changepassword" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Username:<br />
        <asp:TextBox ID="txtUser" runat="server" Width="438px"></asp:TextBox>
        <br />
        Old Password:<br />
        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" Width="438px"></asp:TextBox>
        <br />
        New Password:<br />
        <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" Width="438px"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="txtNewPass" 
            ErrorMessage="Requires at least 1 number, 1 special character and at least 8 characters long" 
            ForeColor="Red" 
            ValidationExpression="(?=^.{7,}$)(?=.*\d)(?=.*\W+)(?![.\n]).*$"></asp:RegularExpressionValidator>
        <br />
        New Password (Confirm):<br />
        <asp:TextBox ID="txtNewPassConfirm" runat="server" TextMode="Password" Width="438px"></asp:TextBox>
        <asp:CompareValidator ID="CompareValidator1" runat="server" 
            ControlToCompare="txtNewPass" ControlToValidate="txtNewPassConfirm" 
            ErrorMessage="Passwords do not match" ForeColor="Red"></asp:CompareValidator>
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
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
            onclick="btnSubmit_Click" />
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txtUser" ErrorMessage="Username is required" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="txtPass" ErrorMessage="Old password is required" 
            ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
            ControlToValidate="txtNewPass" ErrorMessage="New password is required" 
            ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
            ControlToValidate="txtNewPassConfirm" 
            ErrorMessage="New password confirmation is required" ForeColor="Red"></asp:RequiredFieldValidator>
    </p>
</asp:Content>
